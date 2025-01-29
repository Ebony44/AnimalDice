using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Wooriline;

public class AD_AddSubMoney : PacketHandler
{
    public override int GetNumber()
    {
        return (int)COMMON_PK.R_97_ADD_SUBMONEY;
    }
    public override void Func()
    {
        // TODO: transfer into AD_AddSubMoney.cs

        var rec = new R_97_ADD_SUBMONEY(SubGameSocket.m_bytebuffer);
        Debug.Log("[R_97_ADD_SUBMONEY] have money : " + (long)rec.stHAVEMONEY.stHAVEMONEY
            + " submoney " + (long)rec.stSUBMONEY
            + " user is " + rec.stUSER.szID);

        var player = ResourcePool.Find<GamePlayer>(p => p.roomSerial == rec.stUSER.nSERIAL);
        // player.Have = rec.stHAVEMONEY.stHAVEMONEY;
        // player.Gap = rec.stHAVEMONEY.stGAPMONEY;

        var mySerial = cGlobalInfos.GetIntoRoomInfo_97().nSERIAL;

        

        var box = ResourceContainer.Get<Transform>("TBPos");
        var moneyPos = ResourceContainer.Get<TextMeshProUGUI>("MyMoney");
        var me = ResourcePool.Find<GamePlayer>(g => g.roomIdx == 0);

        if (rec.stUSER.nSERIAL.Equals(mySerial))
        {
            TimeContainer.ContainClear("openUnabledBox");
            var tempBox = ResourceContainer.Get<GameTimeBonusBox>();
            var delayTime = tempBox.box.SkeletonData.FindAnimation(GameTimeBonusBox.E_BOXKIND.silver.ToString() + "_open").Duration;
            // Debug.Log("delay time is " + delayTime);
            TimeContainer t1 = new TimeContainer("openUnabledBox", delayTime);

            StartCoroutine(OpenUnabledBoxRoutine(t1, rec.stSUBMONEY, moneyPos.transform.position));

            ResourceContainer.Get<ADMyInfoTag>("MyInfo").haveMoney = rec.stHAVEMONEY.stHAVEMONEY;
            ResourceContainer.Get<ADMyInfoTag>("MyInfo").gapMoney = rec.stHAVEMONEY.stGAPMONEY;


        }
        else
        {
            //player.lbGab.NumberTween(player.Gap, rec.stHAVEMONEY.stGAPMONEY, g => g.ToStringWithKMB(false, 3, false), GameUtils.st_globalTweenTime);
            //player.lbGab.color = rec.stHAVEMONEY.stGAPMONEY >= 0 ? Color.green : Color.red;
            //if (rec.stHAVEMONEY.stGAPMONEY == 0)
            //{
            //    player.lbGab.color = Color.white;
            //}
            // player.Gap = rec.stHAVEMONEY.stGAPMONEY;
            ResourceContainer.Get<ADGameMain>().SetPlayerGapMoney(player, rec.stHAVEMONEY.stGAPMONEY);
            ResourceContainer.Get<ADGameMain>().SetPlayerHaveMoney(player, rec.stHAVEMONEY.stHAVEMONEY);
            player.lbHave.color = new Color(0f, 0f, 0f, 0f);

            var temp = SpineEffect.Instance.spineList;
            var tempIndex = temp.FindIndex(x => x.animationName.Equals("gold_open"));
            // var tempIndex = 
            Debug.Log("spine list's 5th item is " + SpineEffect.Instance.spineList[5].animationName);
            
            SpineEffect.Play(tempIndex, player.transform.position, "WorldForward", 6050);

        }


        // ResourceContainer.Get<CoinEffManager>().CoinMoveEff(box.position, me.lbHave.transform.position);
        //ResourceContainer.Get<GamePanelGiftQuest>().UpdateSubMoney((long)rec.stSUBMONEY);
        //ResourceContainer.Get<GameTimeBonusBox>().UpdateSubMoney((long)rec.stSUBMONEY);
        

    }

    [TestMethod]
    public void TestAddSubmoney()
    {
        // var rec = new R_97_ADD_SUBMONEY(new ByteBuffer[] { });
        var box = ResourceContainer.Get<RectTransform>("TBPos");
        var me = ResourcePool.Find<GamePlayer>(g => g.roomIdx == 0);
        ResourceContainer.Get<CoinEffManager>().CoinMoveEff(box.position, me.lbHave.transform.position);
        // ResourceContainer.Get<GamePanelGiftQuest>().UpdateSubMoney(0);
        // ResourceContainer.Get<GameTimeBonusBox>().UpdateSubMoney(0);


        TimeContainer.ContainClear("openUnabledBox");
        var tempBox = ResourceContainer.Get<GameTimeBonusBox>();
        var delayTime = tempBox.box.SkeletonData.FindAnimation(GameTimeBonusBox.E_BOXKIND.silver.ToString() + "_open").Duration;
        // Debug.Log("delay time is " + delayTime);
        TimeContainer t1 = new TimeContainer("openUnabledBox", delayTime);
        var temp = SpineEffect.Instance.spineList;
        var tempIndex = temp.FindIndex(x => x.animationName.Equals("gold_open"));
        // var tempIndex = 
        Debug.Log("spine list's 5th item is " + SpineEffect.Instance.spineList[5].animationName);
        SpineEffect.Play(tempIndex, me.transform.position, "WorldForward", 6050);

        StartCoroutine(OpenUnabledBoxRoutine(t1, 1234, Vector3.zero));

    }
    [TestMethod]
    public void TestAddSubmoney(int index, int animIndex, int gold)
    {
        //player.lbGab.NumberTween(player.Gap, rec.stHAVEMONEY.stGAPMONEY, g => g.ToStringWithKMB(false, 3, false), GameUtils.st_globalTweenTime);
        //player.lbGab.color = rec.stHAVEMONEY.stGAPMONEY >= 0 ? Color.green : Color.red;
        //if (rec.stHAVEMONEY.stGAPMONEY == 0)
        //{
        //    player.lbGab.color = Color.white;
        //}
        //player.lbHave.color = new Color(0f, 0f, 0f, 0f);


        var player = ResourcePool.Find<GamePlayer>(p => p.roomIdx == index);
        // SpineEffect.Play(animIndex, player.transform.position, "WorldForward", 6050);

        TimeContainer.ContainClear("openUnabledBox");
        var tempBox = ResourceContainer.Get<GameTimeBonusBox>();
        var delayTime = tempBox.box.SkeletonData.FindAnimation(GameTimeBonusBox.E_BOXKIND.silver.ToString() + "_open").Duration;
        TimeContainer t1 = new TimeContainer("openUnabledBox", delayTime);
        StartCoroutine(OpenUnabledBoxRoutine(t1, gold, Vector3.zero));

    }

    public IEnumerator OpenUnabledBoxRoutine(TimeContainer t1, long subMoney, Vector3 toCoinPos)
    {
        var tempBox = ResourceContainer.Get<GameTimeBonusBox>();
        tempBox.OpenBoxAndMoveCoin(toCoinPos);
        tempBox.SetCurrentValue(2);
        
        tempBox.gameObject.SetActive(true);
        // tempBox.OpenBox();
        tempBox.receiveButton.SetActive(false);
        tempBox.timer.SetActive(false);
        var tempMoney = subMoney.ToStringWithKMB(false, 3, false);
        ResourceContainer.Get<GamePanelGiftQuest>().UpdateSubMoney( subMoney);
        ResourceContainer.Get<GameTimeBonusBox>().UpdateSubMoney(subMoney);

        tempBox.isOpen = false;

        yield return null;
    }
}

public class ADAddSubmoneyRoutine : IAction
{
    public string Log => "[R_97_ADD_SUBMONEY]";

    public string ID => "R_97_ADD_SUBMONEY_Action";

    public List<string> CancelID => new List<string> { "R_97_ADD_SUBMONEY_Action" };

    public IEnumerator ActionRoutine()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<TimeContainer> GetAllTimeContainerList()
    {
        throw new System.NotImplementedException();
    }
}