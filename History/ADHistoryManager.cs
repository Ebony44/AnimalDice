using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADHistoryManager : ResourceItemBase
{

    Dictionary<eADBetPlace, int> statistics = new Dictionary<eADBetPlace, int>() { };
    #region result dice part variable
    public int previousHistoryDiceCount = 3;
    #endregion

    public void IncrementHistoryWhenBet(List<eADBetPlace> bettings)
    {
        foreach(var betItem in bettings)
        {
            if (statistics.ContainsKey(betItem))
            {
                statistics[betItem]++;
            }

        }
    }

    public void CheckDicePercentage()
    {
        
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
