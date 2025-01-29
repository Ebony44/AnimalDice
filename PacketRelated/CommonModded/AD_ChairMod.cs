using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wooriline;
/// <summary>
/// Serial : 방에서 유저의 절대위치
/// Idx : 방에서 유저의 상대위치(나의 위치는 고정되어 있음)
/// 
/// 
/// player
/// </summary>
public class AD_ChairMod : PacketHandler
{

    public override int GetNumber()
    {
        return (int)COMMON_PK.R_97_CHAIRMOD;
    }

    public override void Func()
    {
        var rec = new R_97_CHAIRMOD(SubGameSocket.m_bytebuffer);
        Debug.Log("[R_97_CHAIRMOD] fromIdx : " + rec.stFROM.stUSER.nSERIAL + ", toIdx : " + rec.stTO.stUSER.nSERIAL);


        int toIdx = GameUtils.ConvertSerialToPosition(rec.stTO.stUSER.nSERIAL);
        int fromIdx = GameUtils.ConvertSerialToPosition(rec.stFROM.stUSER.nSERIAL); // 밖에서 입장할 경우 99로 들어옴
        var user = rec.stTO;


        if (toIdx == fromIdx && toIdx != -1) // 위치는 그대로, 상태만 바뀌었을 경우(디스컨넥)
        {
            Debug.Log("[R_97_CHAIRMOD] 디스컨넥");
            //예전에는 state를 바꾸긴 하는데 아무것도 안함.
        }
        else if (toIdx == -1 && fromIdx != -1) // 자리에 있던 유저가 퇴장할 경우
        {
            Debug.Log("[R_97_CHAIRMOD] 유저 퇴장");
            var userOut = new ADUserOut(fromIdx);
            ActionPlayer.Play(userOut);
        }
        else if (toIdx != -1 && fromIdx == -1) // 유저가 방에 입장할 경우
        {

            // remove when server side ready...
            var tempMod = 0;

            Debug.Log("[R_97_CHAIRMOD] 유저 입장. nick : " + user.stUSER.szID + ", have money : " + user.stHAVEMONEY.stHAVEMONEY.ToNumberString()
                + " user photo url " + user.stPHOTO.szPHOTO);
            var userIn = new ADUserIn(toIdx, user.stUSER.szID, user.stHAVEMONEY.stHAVEMONEY, user.stHAVEMONEY.stGAPMONEY, user.stPHOTO.szPHOTO, user.stUSER.nSERIAL, user.nSEX == 0,
                vip: (user.stPHOTO.nVIPTYPE + tempMod) );
            ActionPlayer.Play(userIn);
        }
        else
        {
            Debug.LogError("[R_97_CHAIRMOD] Error!! \nstTo serial : " + rec.stTO.stUSER.nSERIAL + ", idx : " + toIdx + "\nstFrom serial : " + rec.stFROM.stUSER.nSERIAL + ", idx : " + fromIdx);
        }
    }

    [TestMethod(false)]
    public void TestUserIn(int idx = 1, string nick = "test", long have = 10000, long gab = 0)
    {
        var userin = new ADUserIn(idx, nick, have, 0, null, 1, true,0);
        ActionPlayer.Play(userin);
    }

    [TestMethod(false)]
    public void FindUser(int idx = 1)
    {

        var pp = ResourcePool.Find<GamePlayer>(p => p.roomIdx == idx);
        Debug.Log(pp.name);
    }


    [TestMethod(false)]
    public void UserOut(int idx = 1)
    {

        var pp = ResourcePool.Find<GamePlayer>(p => p.roomIdx == idx);
        var userOut = new ADUserOut(idx);
        ActionPlayer.Play(userOut);
    }
}

public class ADUserOut : IAction
{
    public string Log => "";

    public string ID => "UserOut";

    public List<string> CancelID => null;
    private TimeContainer moveTime;
    private int _idx;
    public ADUserOut(int idx)
    {
        _idx = idx;
        // moveTime = 2.5f;
        moveTime = new TimeContainer("userOut" + _idx, 0.5f);
    }
    public IEnumerator ActionRoutine()
    {
        var pp = ResourceContainer.Get<GameUserPosition>(_idx);
        var player = ResourcePool.Find<GamePlayer>(p => p.roomIdx == _idx);



        // yield return player.Move(pp.hideUserPos, moveTime);
        // player.gameObject.GetComponent<Image>().SetAlpha(1f, true);

        // player.gameObject.GetComponent<Image>().SetAlpha(1f);
        // yield return null;
        // player.gameObject.GetComponent<Image>().SetAlpha(1f, true);

        yield return player.gameObject.GetComponent<Image>().AlphaTween(0f, moveTime, true);
        player.Clear();
        // player.Clear();
        player.roomSerial = -1;
        
        //player.userPhoto.texture = player.userDefaultPhoto;
        

        // TimeContainer.ContainClear("userOut");
        player.userPhoto.SetAlpha(0f, true);
        player.transform.localPosition = ResourceContainer.Get<GameObject>("ADUserOutPos").transform.localPosition;
        // player.userPhoto.gameObject.SetActive(false);




    }

    public IEnumerable<TimeContainer> GetAllTimeContainerList()
    {
        yield return moveTime;
    }
}

public class ADUserIn : IAction
{
    public string Log => "";

    public string ID => "UserIn";

    public List<string> CancelID => null;

    private TimeContainer.Stack moveTime;


    private int _idx;
    private string _nick;
    private long _have;
    private long _gab;
    private string _url;
    private int _serial;

    private bool _isMale;

    private int _vip;

    private TimeContainer.StackSum _all;

    public ADUserIn(int idx, string nick, long have, long gab, string url, int serial, bool isMale, int vip)
    {
        _idx = idx;
        _nick = nick;
        _have = have;
        _gab = gab;
        _url = url;
        _serial = serial;
        //moveTime = 0.5f;
        moveTime = new TimeContainer.Stack(11, 0.5f, "USER_IN_CHAIRMOD");
        _isMale = isMale;
        _vip = vip;

        _all = new TimeContainer.StackSum(moveTime);
    }


    public IEnumerator ActionRoutine()
    {
        Debug.Log("LEFTING from user in, Tc is " + "userOut" + _idx);
        TimeContainer.ContainClear("userOut" + _idx);


        var pp = ResourceContainer.Get<GameUserPosition>(_idx);
        //var player = ResourcePool.Pop<GamePlayer,GamePlayerInitData>(new GamePlayerInitData(_nick, _serial, _have, _gab, _url,_idx,false));
        var player = ResourcePool.Find<GamePlayer>(p => p.roomIdx == _idx);
        
        

        player.OnPop(new GamePlayerInitData(_nick, _serial, _have, _gab, _url, _idx, false, _isMale, _vip));
        player.userPhoto.SetAlpha(0f);
        player.vip.transform.localScale = Vector3.one;

        // player.SwitchNameAndGap();

        // TimeContainer.ContainClear("USER_IN_CHAIRMOD");
        // TimeContainer tempTc = new TimeContainer("USER_IN_CHAIRMOD", 0.02f);
        // yield return ResourceContainer.Get<ADGameMain>().Wait(tempTc);

        yield return new WaitForSeconds(0.02f);

        player.gameObject.GetComponent<Image>().SetAlpha(0f, true);
        

        //
        var mySerial = cGlobalInfos.GetIntoRoomInfo_97().nSERIAL;
        if (_serial.Equals(mySerial))
        {
            ResourceContainer.Get<TextMeshProUGUI>("MyID").text = _nick;
            ResourceContainer.Get<ADMyInfoTag>("MyInfo").haveMoney = _have;
            ResourceContainer.Get<ADMyInfoTag>("MyInfo").gapMoney = _gab;
            // ResourceContainer.Get<TextMeshProUGUI>("MyMoney").text = _have.ToStringWithKMB();
            // ResourceContainer.Get<TextMeshProUGUI>("MyGap").text = _gab.ToStringWithKMB();

            player.transform.localScale = Vector3.one * 1.1f;
            
            DeactivateCommonFunctions(ref player);
        }
        else
        {
            // // player.lbHave.NumberKMBTween(prev, _have, GameUtils.st_globalTweenTime);

            // player.Have = _have;
            // 
            // player.lbHave.SetAlpha(0f, false);
            // player.lbNickname.StopAllCoroutines();
            // player.lbNickname.color = new Color(0.8627451f, 0.4705882f, 0.07843138f);
            // player.lbNickname.text = _have.ToStringWithKMB(false, 3, false);
            // player.lbNickname.rectTransform.sizeDelta = new Vector2(140f * 1.2f,
            //     22f * 1.2f);
            // player.lbNickname.fontSize = 16;
            // // player.lbNickname.NumberKMBTween(player.Have, _have, GameUtils.st_globalTweenTime);
            // // player.Gap = _gab;
            // player.lbGab.text = string.Empty;
            // player.lbHave.color = new Color(0f, 0f, 0f, 0f);
            // player.lbHave.text = string.Empty;

            player.lbHave.SetAlpha(0f, false);
            player.lbHave.color = new Color(0f, 0f, 0f, 0f);
            // player.lbHave.text = string.Empty;
            // player.lbHave.text = _gab.ToStringWithKMB(false, 3, false);
            player.lbGab.text = _gab.ToStringWithKMB(false, 3, false);
            player.lbGab.color = _gab >= 0 ? Color.green : Color.red;
            if(_gab == 0)
            {
                player.lbGab.color = Color.white;
            }
            

            

        }

        //

        // player.transform.position = pp.hideUserPos.position;
        // yield return player.Move(pp.userPos.position, moveTime);
        player.transform.position = pp.userPos.position;
        // yield return player.gameObject.GetComponent<Image>().AlphaTween(1f, moveTime, false);
        var images = player.GetComponentsInChildren<Image>();
        foreach (var item in images)
        {
            if (item.gameObject.name.Equals("Image"))
            {
                item.AlphaTween(1f, moveTime.Pop(), false);
            }
        }
        if(player.vip.isActiveAndEnabled)
        {
            player.vip.GetComponent<Image>().AlphaTween(1f, moveTime.Pop(), false);
        }
        yield return player.userPhoto.AlphaTween(1f, moveTime.Pop(), false);
        player.lbNickname.SetAlpha(0f, false);
        // player.lbGab.gameObject.SetActive(true);
        player.lbGab.gameObject.SetActive(false);

        player.lbGab.SetAlpha(1f, false);
        player.lbHave.color = new Color(0f, 0f, 0f, 0f);
        var gapBackImages = player.GetComponentsInChildren<Image>();
        foreach (var item in gapBackImages)
        {
            if (item.gameObject.name.Equals("Image"))
            {
                item.SetAlpha(0.65f, false);
            }
        }
    }

    public IEnumerable<TimeContainer> GetAllTimeContainerList()
    {
        return _all;
    }

    private void DeactivateCommonFunctions(ref GamePlayer player)
    {

        Debug.Log("AD_CHAIRMOD, myserial, deactivate any label");
        var images = player.GetComponentsInChildren<Image>();
        foreach (var item in images)
        {
            if (item.gameObject.name.Equals("Image"))
            {
                item.gameObject.SetActive(false);
            }
        }
        player.lbNickname.text = string.Empty;
        player.lbGab.text = string.Empty;
        player.lbHave.text = string.Empty;
        player.lbHave.SetAlpha(0f, false);
        
        // player.lbNickname.gameObject.GetComponent<Button>().interactable = false;

        // player.lbHave.color = new Color(0f, 0f, 0f, 0f);

    }

}

