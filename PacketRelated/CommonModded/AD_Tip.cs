using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AD_Tip : PacketHandler
{
    public override int GetNumber()
    {
        return (int)COMMON_PK.R_97_SENDTO_TIP;
    }

    public float tipDownOffset = -2f;

    public override void Func()
    {
        var rec = new R_97_SENDTO_TIP(SubGameSocket.m_bytebuffer);
        Debug.Log("[R_97_SENDTO_TIP] id : " + rec.stUSER.szID + ", msg : " + (long)rec.stTIP
            + " tip user have money " + (long)rec.stHAVEMONEY.stHAVEMONEY
            + " tip user gap money " + (long)rec.stHAVEMONEY.stGAPMONEY
            + " tp time " + rec.nTIME);

        var player = ResourcePool.Find<GamePlayer>(p => p.roomIdx == rec.stUSER.nSERIAL.ConvertToRoomIdx());
        var npcPos = ResourceContainer.Get<SpriteRenderer>("NPCTip").transform.position + new Vector3(0, tipDownOffset, 0);

        ResourceContainer.Get<ADGameMain>().SetPlayerGapMoney(player, rec.stHAVEMONEY.stGAPMONEY);
        ResourceContainer.Get<ADGameMain>().SetPlayerHaveMoney(player, rec.stHAVEMONEY.stHAVEMONEY);

        #region temp
        //if (player.roomIdx == 0)
        //{
        //    ResourceContainer.Get<ADMyInfoTag>("MyInfo").haveMoney = rec.stHAVEMONEY.stHAVEMONEY;
        //    ResourceContainer.Get<ADMyInfoTag>("MyInfo").gapMoney = rec.stHAVEMONEY.stGAPMONEY;
        //}
        //else
        //{
        //    Debug.Log("custom method, player start gap " + player.Gap
        //    + " player end gap " + (long)rec.stHAVEMONEY.stGAPMONEY);
        //    player.lbGab.NumberTween(player.Gap, rec.stHAVEMONEY.stGAPMONEY, g => g.ToStringWithKMB(false, 3, false), 0.25f);
        //    player.lbGab.color = rec.stHAVEMONEY.stGAPMONEY >= 0 ? Color.green : Color.red;
        //    if (rec.stHAVEMONEY.stGAPMONEY == 0)
        //    {
        //        player.lbGab.color = Color.white;
        //    }

        //}

        //player.Have = rec.stHAVEMONEY.stHAVEMONEY;
        //player.Gap = rec.stHAVEMONEY.stGAPMONEY;
        #endregion

        if (rec.stHAVEMONEY.stHAVEMONEY < ResourceContainer.Get<ADAnteDependSetting>().chipValueInThisRoom[eAD_BUTTONLIST._BTN_BETTING_1]
            && ResourceContainer.Get<ADChipBettingManager>().AllInActiveDic.ContainsKey(player.roomSerial) == false)
        {
            //Debug.Log("user " + user.Nick + " have money " + user.Have
            //    + "current room least chip value is " + ResourceContainer.Get<ADAnteDependSetting>().chipValueInThisRoom[eAD_BUTTONLIST._BTN_BETTING_1]);

            ResourceContainer.Get<ADChipBettingManager>().AllInActiveDic.Add(player.roomSerial, true);
            var mySerial = cGlobalInfos.GetIntoRoomInfo_97().nSERIAL;
            var myIdx = cGlobalInfos.GetIntoRoomInfo_97().nSERIAL.ConvertToRoomIdx();
            ResourceContainer.Get<ADGameMain>().PlayAllInEffect(player.roomIdx, true, player.roomSerial == mySerial ? true : false);
            // when all in effect activated, bettingboard function comes in
            //Debug.LogError("can bet, my serial " + mySerial
            //    + " tip serial " + player.roomSerial);
            ResourceContainer.Get<ADChipBettingManager>().SetEnableBettingBoards(player.roomSerial != mySerial ? true : false);
        }

        long tip = rec.stTIP;
        
        StartCoroutine(TipRoutine(player.transform.position, npcPos, rec.stTIP, (float)rec.nTIME / 1000));
    }

    IEnumerator TipRoutine(Vector3 start, Vector3 end, long gold, float time)
    {
        var spine = ResourcePool.Pop<GameTipItem, long>(gold);
        var pot = ResourceContainer.Get<SpriteRenderer>("NPCTip");
        pot.GetComponent<ResourceTag>().StopAllCoroutines();
        pot.AlphaTween(1f, 0.1f);

        spine.transform.position = start;
        // Sound.Instance.EffPlay("GOLD_MOVE");
        Sound.Instance.EffPlay("E_LV1BET");
        yield return spine.Move(end, time);



        yield return spine.Wait(0.2f);
        pot.AlphaTween(0f, 0.5f);
        spine.Back();
    }


}
