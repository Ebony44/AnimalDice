using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wooriline;

public class ADResultWin : PacketHandler
{
    public override int GetNumber()
    {
        return (int)ANIMALDICE_PK.R_09_RESULT_WIN;
    }

    public override void Func()
    {
        var rec = new R_09_RESULT_WIN(SubGameSocket.m_bytebuffer);
        
        if (GameUtils.st_maxPlayer == -1 || GameUtils.CheckMySerial(-1))
        {
            Debug.Log("loading isn't over yet");
            return;
        }

        Debug.Log("[R_09_RESULT_WIN] " + ", time : " + rec.nTIME / 1000f + ", moveTime : " + rec.nMOVETIME / 1000f);
        // chip (that's from betting board) move to dealer time?
        var winTables = rec.lWINPART;
        var winUsers = rec.lWINNER;

        // var loserResultMoney = new List<long>();
        // var loserHaveMoney = new List<long>();
        // var loserBettingMoney = new List<long>();

        //foreach (var loser in rec.lLOSEPART)
        //{

        //}

        // movetime -> 2 times, move to board(from dealer)
        // and move to players(from board)

        var winAction = new ADResultWinAction(rec.nTIME / 1000f, rec.nMOVETIME / 1000f, rec.nMOVETIME / 1000f, winTables, winUsers);

        ActionPlayer.Play(winAction);

        // throw new System.NotImplementedException();
    }
    public class ADResultWinAction : IAction
    {
        public string Log => "[R_09_ADResultWinAction]";

        public string ID => "ADResultWinAction";

        public List<string> CancelID => new List<string> { "ADResultWinAction" };

        TimeContainer.Stack _totalTime;
        TimeContainer.Stack _moveToBoardTime;
        TimeContainer.Stack _moveToPlayersTime;

        TimeContainer.StackSum _all;

        List<st09_TABLE_PART> _winTableParts;
        List<st09_WINLOSE> _winUsers;


        public ADResultWinAction(float totalTime, float moveToBoardTime, float moveToPlayersTime, List<st09_TABLE_PART> winTableParts, List<st09_WINLOSE> winUsers)
        {
            // _totalTime = totalTime;
            _totalTime = new TimeContainer.Stack(1, totalTime, "AD_RESULT_WIN_TOTAL_TIME"); // scale and transform
            _moveToBoardTime = new TimeContainer.Stack(1, moveToBoardTime, "AD_RESULT_WIN_MOVE_TO_BOARD_TIME"); // alpha and transform
            _moveToPlayersTime = new TimeContainer.Stack(1, moveToPlayersTime, "AD_RESULT_WIN_MOVE_TO_PLAYER_TIME"); // alpha and transform

            _winTableParts = winTableParts;
            _winUsers = winUsers;


            _all = new TimeContainer.StackSum(_totalTime, _moveToBoardTime, _moveToPlayersTime);

        }

        public IEnumerator ActionRoutine()
        {
            var mySerial = cGlobalInfos.GetIntoRoomInfo_97().nSERIAL;
            // 1.mark chips (from winningBetPlace)

            for (int i = 0; i < _winUsers.Count; i++)
            {
                var currentTable = _winUsers[i];
                // currentTable.nTABLEPOS
                var winBetPlace = (eADBetPlace)currentTable.nTABLEPOS;
                var userIndex = currentTable.stUSER.nSERIAL.ConvertToRoomIdx();
                var winMoney = currentTable.stRESULTMONEY;
                long betMoney = currentTable.stBETMONEY;
                ResourceContainer.Get<ADResultPartInfoStoring>().SetWinningDic(winBetPlace, userIndex, (winMoney + betMoney) );
                Debug.Log("win user is " + _winUsers[i].stUSER.szID
                    + " earned money " + (long)_winUsers[i].stRESULTMONEY
                    + " bet money " + (long)_winUsers[i].stBETMONEY
                    + " win bet place " + ((eADBetPlace)_winUsers[i].nTABLEPOS).ToString());

            
            }
            ResourceContainer.Get<ADChipBettingManager>().bCanMarkChips = true;
            yield return new WaitForSeconds(0.3f); // for testing only, delete it after test
            ResourceContainer.Get<ADChipBettingManager>().bCanMarkChips = false;


            // 2. move to board (from dealer)
            ResourceContainer.Get<ADChipBettingManager>().bCanMoveToBoard = true;
            ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().CreateChipFromResultStep();

            // 2.1. play sound... chip count will set on component system
            ResourceContainer.Get<ADChipBettingManager>().PlayBettingSound(
            chipsForMovingCount: ResourceContainer.Get<ADResultPartInfoStoring>().currentWinChips,
            bIsMyBetting: false);

            foreach (var winUser in _winUsers) // write result money to my betting label
            {
                if(winUser.stUSER.nSERIAL.Equals(mySerial))
                {
                    //var myBettingBoard = ResourceContainer.Get<ADChipBettingManager>().GetBetBoard((eADBetPlace)winUser.nTABLEPOS);
                    ResourceContainer.Get<ADChipBettingManager>().SetMyBettingMoney((eADBetPlace)winUser.nTABLEPOS, winUser.stRESULTMONEY);
                    Debug.Log("[R_09_RESULT_WIN] table position " + ((eADBetPlace)winUser.nTABLEPOS).ToString() 
                        + " result money " + (long)winUser.stRESULTMONEY);
                }
            }
            
            
            
            yield return ResourceContainer.Get<ADChipBettingManager>().Wait(_moveToBoardTime.Pop());

            ResourceContainer.Get<ADChipBettingManager>().bCanMoveToBoard = false;

            // yield return new WaitForSeconds(0.15f); // for testing only, delete it after test
            yield return new WaitForSeconds(1f); // staying time before move to each player

            // 3. move to players (from board)
            ResourceContainer.Get<ADChipBettingManager>().bCanMoveToWinPlayer = true;

            // 3.1. play sound... chip count will set on component system
            ResourceContainer.Get<ADChipBettingManager>().PlayBettingSound(
            chipsForMovingCount: ResourceContainer.Get<ADResultPartInfoStoring>().currentWinChips,
            bIsMyBetting: false);

            // 4. turn off table highlight

            ResourceContainer.Get<ADChipBettingManager>().DisableAllBettingBoardHighlight();

            Debug.Log("[R_09_RESULT_WIN] light off betting boards");

            // 4.1 turn off multiplier spine
            ResourcePool.ClearAll<ADSpineMultiplierEffectItem>(ef =>
            {
                ef.tag.Equals("ADMultiplier");
                return ef;
            });

            yield return ResourceContainer.Get<ADChipBettingManager>().Wait(_moveToPlayersTime.Pop());


            ResourceContainer.Get<ADChipBettingManager>().bCanMoveToWinPlayer = false;

            // yield return new WaitForSeconds(0.15f); // for testing only, delete it after test

            yield return new WaitForSeconds(ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().timeStandard * (4/5) ); // when moving....start alpha off and deleting
            // 5. disable betting money label and destroy chips
            ResourceContainer.Get<ADChipBettingManager>().DisableAllMyBettingMoneyLabel();

            Debug.Log("[R_09_RESULT_WIN] disable all my betting label");



            ResourceContainer.Get<ADChipBettingManager>().ClearAllChips();

            // ResourceContainer.Get<ADGameMain>().ClearAll();

            // throw new System.NotImplementedException();
        }

        public IEnumerable<TimeContainer> GetAllTimeContainerList()
        {
            return _all;
            // throw new System.NotImplementedException();
        }
    }
}
