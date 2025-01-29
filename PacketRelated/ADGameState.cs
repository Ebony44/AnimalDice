using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wooriline;

public class ADGameState : PacketHandler
{
    public override int GetNumber()
    {
        return (int)ANIMALDICE_PK.R_09_GAMESTATE;
    }
    public override void Func()
    {
        var rec = new R_09_GAMESTATE(SubGameSocket.m_bytebuffer);
        Debug.Log("[R_09_GAMESTATE], table parts count " + rec.lPART.Count
            + " user state count " + rec.lUSERSTATE.Count);
        var step = rec.nSTEP;
        var time = ((float)rec.nTIME) / 1000f;
        var tablePart = rec.lPART;
        int currentGameIndex = (int)rec.stGAME_IDX;

        // ResourceContainer.Get<ADBettingTimeCounter>().SetNumber(rec.nSEC);
        // ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().SpawnChipToRandomSpawnPos()

        var gameState = new ADGameStateAction(
            rec.lUSERSTATE,
            tablePart,
            currentGameIndex
            );
        ActionPlayer.Play(gameState);
    }
}

public class ADGameStateAction : IAction
{
    public string Log => "[R_09_ADGameStateAction]";

    public string ID => "ADGameStateAction";

    public List<string> CancelID => new List<string> { "ADGameStateAction" };

    // public int nSTEP;
    // public int nTIME;
    // public int nRESTTIME;
    // public stINT64 stGAME_IDX;
    // public List<st09_STATE_PLAYER> lUSERSTATE;
    // public List<st09_TABLE_PART> lPART;
    
    List<st09_TABLE_PART> _tableParts;
    List<st09_STATE_PLAYER> _userStates;
    int _currentGameIndex;



    public ADGameStateAction(List<st09_STATE_PLAYER> userStates, List<st09_TABLE_PART> tableParts, int currentGameIndex)
    {
        _userStates = userStates;
        _tableParts = tableParts;
        _currentGameIndex = currentGameIndex;
    }

    public IEnumerator ActionRoutine()
    {

        // 0. setting up current game index

        ResourceContainer.Get<ADChipBettingManager>().currentGameIndex = _currentGameIndex;

        
        var mySerial = cGlobalInfos.GetIntoRoomInfo_97().nSERIAL;
        var spawner = ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>();

        // 1. user money setting
        foreach (var user in _userStates)
        {

            Debug.Log("[R_09_GAMESTATE], current user " + user.stUSER.szID
                + " current have money " + (long)user.stHAVEMONEY.stHAVEMONEY
                + " current gap money " + (long)user.stHAVEMONEY.stGAPMONEY);
            var currentUser = ResourcePool.Find<GamePlayer>(p => p.roomSerial == user.stUSER.nSERIAL);
            ResourceContainer.Get<ADGameMain>().SetPlayerGapMoney(currentUser, user.stHAVEMONEY.stGAPMONEY);

            //if (mySerial == user.stUSER.nSERIAL)
            //{
            //    ResourceContainer.Get<ADMyInfoTag>("MyInfo").haveMoney = user.stHAVEMONEY.stHAVEMONEY;
            //    ResourceContainer.Get<ADMyInfoTag>("MyInfo").gapMoney = user.stHAVEMONEY.stGAPMONEY;
            //    currentUser.lbGab.text = string.Empty;
            //    currentUser.lbHave.color = new Color(0f, 0f, 0f, 0f);
            //}
            //else
            //{

            //    currentUser.Have = ((long)user.stHAVEMONEY.stHAVEMONEY);

            //    currentUser.lbHave.SetAlpha(0f, false);
            //    // currentUser.lbNickname.StopAllCoroutines();
            //    // 
            //    // currentUser.lbNickname.NumberTween(currentUser.Gap, g => g.ToStringWithKMB(false, 3, false), GameUtils.st_globalTweenTime);

            //    // currentUser.StopAllCoroutines();
            //    //currentUser.lbGab.NumberTween(currentUser.Gap, g => g.ToStringWithKMB(false, 3, false), GameUtils.st_globalTweenTime);

            //    // currentUser.lbGab.text = ((long)user.stHAVEMONEY.stGAPMONEY).ToStringWithKMB(false, 3, false);
            //    // currentUser.lbGab.color = user.stHAVEMONEY.stGAPMONEY > 0 ? Color.green : Color.red;



            //    if (user.stHAVEMONEY.stGAPMONEY == 0)
            //    {
            //        currentUser.lbGab.color = Color.white;
            //    }

            //    currentUser.lbNickname.SetAlpha(0f, false);
            //    currentUser.lbGab.SetAlpha(1f, false);



            //    // currentUser.Gap = ((long)user.stHAVEMONEY.stGAPMONEY);
            //    // currentUser.lbGab.text = string.Empty;
            //    currentUser.lbHave.color = new Color(0f, 0f, 0f, 0f);


            //}

        }

        // 2.1.
        // if bet button set is late, wait until button set packet is done.
        while (ResourceContainer.Get<ADAnteDependSetting>().chipValueInThisRoom[eAD_BUTTONLIST._BTN_BETTING_1].Equals(-1))
        {
            yield return null;
        }

        // 2. setting up betting board
        foreach (var table in _tableParts)
        {
            var currentDic = ResourceContainer.Get<ADChipBettingManager>().GetChipsFromMoney(table.stTOTALBETMONEY);
            Debug.Log("[R_09_GAMESTATE], current table position " + (eADBetPlace)table.nTABLEPOS
                + " current total money " + (long)table.stTOTALBETMONEY
                );

            
            
            foreach (var chips in currentDic)
            {

                spawner.SpawnChipsToRandomSpawnPos(chips.Key, (eADBetPlace)table.nTABLEPOS, chips.Value);
                // spawner.StartCoroutine(spawner.SpawnChipRoutine(chips.Key, (eADBetPlace)table.nTABLEPOS, chips.Value));
                //Debug.Log("[R_09_GAMESTATE], current chip kind " + chips.Key
                //+ " chip count " + chips.Value
                //+ " table position " + (eADBetPlace)table.nTABLEPOS);
            }
            
            ResourceContainer.Get<ADChipBettingManager>().SetMyBettingMoney((eADBetPlace)table.nTABLEPOS, table.stMYBETMONEY);
            
        }
        
        // 2. 

        // throw new System.NotImplementedException();
        yield return null;
    }

    public IEnumerable<TimeContainer> GetAllTimeContainerList()
    {
        // return null;
        yield break;
        // throw new System.NotImplementedException();
    }
}
