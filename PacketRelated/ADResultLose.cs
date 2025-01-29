using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wooriline;

public class ADResultLose : PacketHandler
{
    public override int GetNumber()
    {
        return (int)ANIMALDICE_PK.R_09_RESULT_LOSE;
    }

    public override void Func()
    {
        var rec = new R_09_RESULT_LOSE(SubGameSocket.m_bytebuffer);
        Debug.Log("[R_09_RESULT_LOSE] " + ", time : " + rec.nTIME / 1000f + ", moveTime : " + rec.nMOVETIME / 1000f);
        // chip (that's from betting board) move to dealer time?
        // possible error handling -> chip move to dealer from board -> chip move from dealer to board

        var loseTables = rec.lLOSEPART;
        // var loserResultMoney = new List<long>();
        // var loserHaveMoney = new List<long>();
        // var loserBettingMoney = new List<long>();

        //foreach(var loser in rec.lLOSEPART)
        //{

        //}
        var loseAction = new ADResultLoseAction(rec.nTIME / 1000f, rec.nMOVETIME / 1000f, rec.lLOSEPART);

        ActionPlayer.Play(loseAction);

        // move time only use board to dealer

        
    }
    public void StoreLoserInfo(in List<st09_TABLE_PART> tableList)
    {
        var tempResult = ResourceContainer.Get<ADResultPartInfoStoring>();
    }
}
public class ADResultLoseAction : IAction
{
    public string Log => "[R_09_ADResultLoseAction]";

    public string ID => "ADResultLoseAction";

    public List<string> CancelID => new List<string> { "ADResultLoseAction" };

    TimeContainer.Stack _totalTime;
    TimeContainer.Stack _moveTime;

    TimeContainer.StackSum _all;

    public List<st09_TABLE_PART> _loseTableParts;

    public ADResultLoseAction(float totalTime, float moveTime, List<st09_TABLE_PART> loseTableParts)
    {
        // _totalTime = totalTime;
        // _moveTime = moveTime;
        _totalTime = new TimeContainer.Stack(1, totalTime, "AD_RESULT_LOSE_TOTAL_TIME"); // scale and transform
        _moveTime = new TimeContainer.Stack(2, moveTime, "AD_RESULT_LOSE_MOVE_TIME"); // alpha and transform

        _all = new TimeContainer.StackSum(_totalTime, _moveTime);

        _loseTableParts = loseTableParts;

    }

    public IEnumerator ActionRoutine()
    {
        // yield return new WaitForSeconds( )

        ResourceContainer.Get<ADChipBettingManager>().bCanMoveToDealer = true;

        // play sound... chip count will set on component system
        yield return new WaitForSeconds(0.01f);
        Debug.Log("losing chip count is " + ResourceContainer.Get<ADResultPartInfoStoring>().currentLoseChips);
        ResourceContainer.Get<ADChipBettingManager>().PlayBettingSound(
            chipsForMovingCount: ResourceContainer.Get<ADResultPartInfoStoring>().currentLoseChips,
            bIsMyBetting: false);

        yield return ResourceContainer.Get<ADChipBettingManager>().Wait( ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().timeStandard * (4/5) ); // disappear when reach 1/5 of distance
        ResourceContainer.Get<ADChipBettingManager>().bDestroyingOnlyLoseChipsWithAlpha = true;

        foreach (var item in ResourceContainer.Get<ADResultPartInfoStoring>().winBetPlace)
        {
            Debug.Log("win bet place is " + item.ToString());
        }

        // disable my betting money text on board
        foreach (var loseBetPlace in _loseTableParts)
        {
            ResourceContainer.Get<ADChipBettingManager>().EnableMyBettingMoneyLabel((eADBetPlace)loseBetPlace.nTABLEPOS, false);
        }
        //


        // ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().timeStandard = _moveTime.Pop().time;
        // ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().Wait(_moveTime.Pop());

        yield return ResourceContainer.Get<ADChipBettingManager>().Wait(_moveTime.Pop());
        ResourceContainer.Get<ADChipBettingManager>().bCanMoveToDealer = false;
        
        yield return ResourceContainer.Get<ADChipBettingManager>().Wait(ResourceContainer.Get<ADChipBettingManager>().destroyingTime);
        ResourceContainer.Get<ADChipBettingManager>().bDestroyingOnlyLoseChipsWithAlpha = false;


        // throw new System.NotImplementedException();
    }

    public IEnumerable<TimeContainer> GetAllTimeContainerList()
    {
        return _all;
    }
}
