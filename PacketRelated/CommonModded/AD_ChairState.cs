using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AD_ChairState : PacketHandler
{
    public override int GetNumber()
    {
        return (int)COMMON_PK.R_97_CHAIRSTATE;
    }
    public override void Func()
    {
        var rec = new R_97_CHAIRSTATE(SubGameSocket.m_bytebuffer);
        Debug.Log("[AD_ChairState] lCHAIRS.count : " + rec.lCHAIRS.Count);

        GameUtils.SetMaxPlayer(rec.lCHAIRS.Count);
        GameUtils.st_playerWinlosecache = new int[rec.lCHAIRS.Count];
        foreach (var chair in rec.lCHAIRS)
        {
            Debug.Log("[AD_ChairState] nick : " + chair.stUSER.szID
                + " user have money " + (long)chair.stHAVEMONEY.stHAVEMONEY
                + " user gap money " +  (long)chair.stHAVEMONEY.stGAPMONEY
                + " user photo url " + chair.stPHOTO.szPHOTO);
            if (chair.stUSER.szID == cGlobalInfos.GetMainMyInfo().szNICK)
            {
                GameUtils.SetMySerial(chair.stUSER.nSERIAL);
                break;
            }
            // GameUtils.SetMySerial(0);
        }

        foreach (var chair in rec.lCHAIRS)
        {
            if (chair.nSTATE == 0)
                continue;

            var serial = chair.stUSER.nSERIAL;
            var nick = chair.stUSER.szID;
            long have = chair.stHAVEMONEY.stHAVEMONEY;
            long gap = chair.stHAVEMONEY.stGAPMONEY;
            var photo = chair.stPHOTO.szPHOTO;
            var roomIdx = serial.ConvertToRoomIdx();
            var pos = ResourceContainer.Get<GameUserPosition>(roomIdx);

            // remove when server side ready...
            var tempMod = 0;

            Debug.Log("load textrue, user serial is  " + chair.stUSER.nSERIAL);

            var player = ResourcePool.Pop<GamePlayer, GamePlayerInitData>(new GamePlayerInitData(
                    nick, serial, have, gap, photo, roomIdx, chair.nEXIT == 1, chair.nSEX == 0, chair.stPHOTO.nVIPTYPE + tempMod)
            );
            
            // player.SwitchNameAndGap();

            player.transform.position = chair.nSTATE == 5 ? pos.hideUserPos.position : pos.userPos.position;
            player.vip.transform.localScale = Vector3.one;

            var mySerial = cGlobalInfos.GetIntoRoomInfo_97().nSERIAL;
            // Debug.LogError("my serial " + cGlobalInfos.GetIntoRoomInfo_97().nSERIAL);
            Debug.LogError("see nstate " + chair.nSTATE
                + " my serial is " + cGlobalInfos.GetIntoRoomInfo_97().nSERIAL
                + " current serial is " + chair.stUSER.nSERIAL);
            if (serial.Equals(mySerial))
            {
                ResourceContainer.Get<TextMeshProUGUI>("MyID").text = nick;
                ResourceContainer.Get<ADMyInfoTag>("MyInfo").haveMoney = have;
                ResourceContainer.Get<ADMyInfoTag>("MyInfo").gapMoney = gap;
                // ResourceContainer.Get<TextMeshProUGUI>("MyMoney").text = have.ToStringWithKMB();
                // ResourceContainer.Get<TextMeshProUGUI>("MyGap").text =   gap.ToStringWithKMB();
                player.transform.localScale = Vector3.one * 1.1f;
                DeactivateCommonFunctions(ref player);
                
            }
            else
            {
                // player.Have = have;
                // 
                // player.lbHave.SetAlpha(0f, false);
                // player.lbNickname.StopAllCoroutines();
                // player.lbNickname.color = new Color(0.8627451f, 0.4705882f, 0.07843138f);
                // player.lbNickname.text = have.ToStringWithKMB(false, 3, false);
                // player.lbNickname.rectTransform.sizeDelta = new Vector2(140f * 1.2f,
                // 22f * 1.2f);
                // player.lbNickname.fontSize = 16;
                // // player.lbNickname.NumberKMBTween(player.Have, have, GameUtils.st_globalTweenTime);
                // // player.Gap = gap;
                // player.lbGab.text = string.Empty;
                // player.lbHave.color = new Color(0f, 0f, 0f, 0f);
                // player.lbHave.text = string.Empty;

                player.lbHave.SetAlpha(0f, false);
                player.lbHave.color = new Color(0f, 0f, 0f, 0f);
                // player.lbHave.text = string.Empty;
                // player.lbHave.text = gap.ToStringWithKMB(false, 3, false);


                // player.lbGab.gameObject.SetActive(true);
                player.lbGab.gameObject.SetActive(false);

                player.lbGab.text = gap.ToStringWithKMB(false, 3, false);
                player.lbGab.color = gap >= 0 ? Color.green : Color.red;
                if (gap == 0)
                {
                    player.lbGab.color = Color.white;
                }

                player.lbNickname.SetAlpha(0f, false);
                player.lbGab.SetAlpha(1f, false);


            }

        }

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

        player.lbHave.color = new Color(0f, 0f, 0f, 0f);

    }

}
