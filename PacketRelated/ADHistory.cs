using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADHistory : PacketHandler
{
    // practically, this packet isn't used...
    public override int GetNumber()
    {
        return (int)ANIMALDICE_PK.R_09_HISTORY;
    }
    public override void Func()
    {
        var rec = new R_09_HISTORY(SubGameSocket.m_bytebuffer);
        Debug.Log("[R_09_HISTORY], history count " + rec.lHISTORYS.Count);

        ResourceContainer.Get<ADHistoryPopup>().bHasHistoryReceived = true;

        int tempLatestIndex = 0;
        var historyPopup = ResourceContainer.Get<ADHistoryPopup>();
        foreach (var history in rec.lHISTORYS)
        {
            if(history.stGAME_IDX > tempLatestIndex)
            {
                tempLatestIndex = (int)history.stGAME_IDX;
            }
            //Debug.Log("[R_09_HISTORY], history index " + (int)history.stGAME_IDX
            //    + " history result money " + (long)history.stRESULTMONEY
            //    + " dice 1 " + ((eAD_DICE)history.nDICE1).ToString()
            //    + " dice 2 " + ((eAD_DICE)history.nDICE2).ToString()
            //    + " dice 3 " + ((eAD_DICE)history.nDICE3).ToString());
            // ResourceContainer.Get<ADHistoryPopup>().
            // AddHistory((int)history.stGAME_IDX, (eAD_DICE)history.nDICE1, (eAD_DICE)history.nDICE2, (eAD_DICE)history.nDICE3, history.stRESULTMONEY);
            
            

            historyPopup.AddHistoryWithoutUpdate((int)history.stGAME_IDX, (eAD_DICE)history.nDICE1, (eAD_DICE)history.nDICE2, (eAD_DICE)history.nDICE3, history.stRESULTMONEY);
            historyPopup.UpdateHistory();

        }
        
        historyPopup.SetCurrentGameIndex(tempLatestIndex);
        ResourceContainer.Get<ADHistoryPopup>().ReorderScroll();
        if(rec.lHISTORYS.Count == 0)
        {
            ResourceContainer.Get<ADHistoryPopup>().ChangeCurrentDie(0, eAD_DICE._DICE5_DAK, true);
            ResourceContainer.Get<ADHistoryPopup>().ChangeCurrentDie(1, eAD_DICE._DICE5_DAK, true);
            ResourceContainer.Get<ADHistoryPopup>().ChangeCurrentDie(2, eAD_DICE._DICE5_DAK, true);
        }
        else
        {
            var temp = rec.lHISTORYS.Find(x => x.stGAME_IDX == tempLatestIndex);
            ResourceContainer.Get<ADHistoryPopup>().ChangeCurrentDie(0, (eAD_DICE)temp.nDICE1, true);
            ResourceContainer.Get<ADHistoryPopup>().ChangeCurrentDie(1, (eAD_DICE)temp.nDICE2, true);
            ResourceContainer.Get<ADHistoryPopup>().ChangeCurrentDie(2, (eAD_DICE)temp.nDICE3, true);
        }

        //throw new System.NotImplementedException();
    }
}
