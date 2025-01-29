using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ADHistoryPopup : ResourceItemBase
{
    public ADHistoryListController historyScroll;

    public const int HISTORY_ITEM_LIMIT = 100;

    #region variables for dice percent part
    public List<Sprite> dicePercentBarSprites;
    public List<Image> dicePercentBars;

    public List<Sprite> currentDiceSprites;
    public List<Image> currentDice;

    public List<eAD_DICE> currentDiceValues = new List<eAD_DICE>(3);
    

    // 1. get total dice count
    public Dictionary<eAD_DICE, int> diceCount = new Dictionary<eAD_DICE, int>(capacity: 6);

    public bool bIsOpened = false;

    public bool bHasHistoryReceived = false;

    #endregion

    #region

    #endregion


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        // InitDiceCount(ref diceCount);
        diceCount.Add(eAD_DICE._DICE1_GE, 0);
        diceCount.Add(eAD_DICE._DICE2_MUL, 0);
        diceCount.Add(eAD_DICE._DICE3_BAK, 0);
        diceCount.Add(eAD_DICE._DICE4_HO, 0);
        diceCount.Add(eAD_DICE._DICE5_DAK, 0);
        diceCount.Add(eAD_DICE._DICE6_SAE, 0);
        foreach (var percent in dicePercentBars)
        {
            percent.fillAmount = 0f;
        }
        CoroutineChain.Start
            .Wait(0.5f)
            .Call(() =>
           {
               if(bHasHistoryReceived == false)
               {
                   ChangeCurrentDie(0, eAD_DICE._DICE5_DAK, true);
                   ChangeCurrentDie(1, eAD_DICE._DICE5_DAK, true);
                   ChangeCurrentDie(2, eAD_DICE._DICE5_DAK, true);
               }
           });
        historyScroll.SetCellDataCapacity(100);
    }

    public void ChangeCurrentDie(int currentDieIndex, eAD_DICE die, bool bEnableAlpha = false)
    {
        Debug.Log("dice " + currentDieIndex + " change into " + (int)die);
        if(currentDice[currentDieIndex].color.a.Equals(1) == false && bEnableAlpha == true)
        {
            currentDice[currentDieIndex].SetAlpha(1f, true);
        }
        currentDice[currentDieIndex].sprite = currentDiceSprites[(int)die - 1];
    }
    #region test methods
    [TestMethod]
    public void TestHistoryList()
    {
        // dice_per
        //    public int nTABLEPOS;
        //public int nPER;
        //historys
        //    public stINT64 stGAME_IDX;
        //public int nDICE1;
        //public int nDICE2;
        //public int nDICE3;

        var tempRec = new R_09_HISTORY();
        // TODO: what about double betting? -> deal with double bet
        // -> must calculate
        for (int i = 0; i < 5; ++i)
        {
            st09_DICE_PER tempPer = new st09_DICE_PER();
            tempPer.nTABLEPOS = (int)eADBetPlace._BET_ANIMAL_GE;
            tempPer.nPER = (int)13 + i; // 33percent

            tempRec.lPER.Add(tempPer);
        }
        for (int i = 0; i < 11; ++i)
        {
            st09_ModHistory tempHis = new st09_ModHistory();
            tempHis.stGAME_IDX = (long)(0 + i);
            tempHis.nDICE1 = (int)eAD_DICE._DICE1_GE;
            tempHis.nDICE2 = (int)eAD_DICE._DICE3_BAK;
            tempHis.nDICE3 = (int)eAD_DICE._DICE4_HO;

            
            tempRec.lHISTORYS.Add(tempHis);
        }
        
        // Initialize(tempRec);
        
        // historyScroll.CellData = tempRec.lHISTORYS;
        
        // tempRec.lPER.Add()
        // tempRec.lHISTORYS
    }

    private long testCount = 0;
    [TestMethod]
    public void TestAddHistoryList(eAD_DICE dice1, eAD_DICE dice2, eAD_DICE dice3, int iterationCount)
    {
        for (int i = 0; i < iterationCount; i++)
        {
            // var tempRec = new R_09_HISTORY();
            st09_HISTORY tempHis = new st09_HISTORY();
            // st09_ModHistory tempHis = new st09_ModHistory();
            tempHis.stGAME_IDX = testCount;// (long)(0 + i);
            var tempInt = UnityEngine.Random.Range(1, 7);
            tempHis.nDICE1 = UnityEngine.Random.Range(1, 7); // (int)dice1;
            tempHis.nDICE2 = UnityEngine.Random.Range(1, 7); // (int)dice2;
            tempHis.nDICE3 = UnityEngine.Random.Range(1, 7); // (int)dice3;

            // tempHis.nDICE1 = (int)dice1;
            // tempHis.nDICE2 = (int)dice2;
            // tempHis.nDICE3 = (int)dice3;
            // tempRec.lHISTORYS.Add()

            tempHis.stRESULTMONEY = 0;

            testCount++;
            var temp = historyScroll.CellData;
            historyScroll.CellData.Add(tempHis);
            if (bIsOpened == false)
            {
            }
            ReorderScroll();
            historyScroll.ReloadData(false);


            SetPercentage();
        }
        
    }

    #endregion

    public void Initialize(R_09_HISTORY rec)
    {
        // historyScroll.CellData = rec.lHISTORYS;

        // PanelOpen();

    }

    #region methods for dice percent part
    [TestMethod]
    public void SetPercentage()
    {
        // 1. from cellItem, get dice infos
        var tempDic = new Dictionary<eAD_DICE, int>();
        
        tempDic = GetDicFromCells(historyScroll.CellData);

        // 2. get dice count
        var tempInt = GrabTotalDiceCount(in tempDic);

        // 3. calculate which are top 3
        #region 
        
        // if(historyScroll.CellData)
        
        for (int i = ((int)eAD_DICE._DICE1_GE - 1); i <= (int)eAD_DICE._DICE6_SAE - 1; i++)
        {
            var tempFloat = 0f;
            // var tempFloat = (tempDic[(eAD_DICE)i] / (float)tempInt);
            // var tempFloat = (tempDic[(eAD_DICE)(i + 1)] / (float)100); // if total is below 100...
            //var tempFloat = (tempDic[(eAD_DICE)(i + 1)] / (float)tempInt);
            
            if (historyScroll.CellData.Count >= 100) // if more than or equal 100
            {
                tempFloat = (tempDic[(eAD_DICE)(i + 1)] / (float)100);
            }
            else
            {
                var itemCount = 0;
                foreach (var item in tempDic)
                {
                    if (item.Value != 0 )
                    {
                        itemCount++;
                    }
                }
                // tempFloat = (tempDic[(eAD_DICE)(i + 1)] / ((float)tempInt)) * (itemCount);
                tempFloat = tempDic[(eAD_DICE)(i + 1)] / ((float)historyScroll.CellData.Count);
                //tempFloat = (tempDic[(eAD_DICE)(i + 1)] / ((float)itemCount));
            }

            if (tempFloat >= 1f)
            {
                tempFloat = 1f;
            }
            dicePercentBars[i].fillAmount = tempFloat;
            
        }

        #region coloring
        var tempList = tempDic.OrderByDescending(x => x.Value).ToList();
        // top 3 will be red
        for (int i = 0; i < 3; i++)
        {
            var dicePercentIndex = (int)(tempList[i].Key - 1);
            dicePercentBars[dicePercentIndex].sprite = dicePercentBarSprites[1];
        }
        // check third is equal to 2nd or 1st

        var standardValue = tempList[2].Value;
        if (tempList[1].Value.Equals(tempList[2].Value))
        {
            standardValue = tempList[1].Value;
            if (tempList[0].Value.Equals(tempList[1].Value))
            {
                standardValue = tempList[0].Value;
            }
        }

        // check ohter 3 are equal to the third from top
        for (int i = 3; i < 6; ++i)
        {
            var dicePercentIndex = (int)(tempList[i].Key - 1);
            if (tempList[i].Value >= standardValue)
            {
                
                dicePercentBars[dicePercentIndex].sprite = dicePercentBarSprites[1];
            }
            else
            {
                dicePercentBars[dicePercentIndex].sprite = dicePercentBarSprites[0];
            }
        }
        #endregion


        #endregion
    }
    public Dictionary<eAD_DICE,int> GetDicFromCells(in List<st09_HISTORY> cellDatas)
    {
        var tempDic = new Dictionary<eAD_DICE, int>();
        InitDiceCount(ref tempDic);
        var tempCellDatas = historyScroll.CellData;
        for (int i = 0; i < tempCellDatas.Count; i++)
        {
            var tempDice1 = (eAD_DICE)tempCellDatas[i].nDICE1;
            var tempDice2 = (eAD_DICE)tempCellDatas[i].nDICE2;
            var tempDice3 = (eAD_DICE)tempCellDatas[i].nDICE3;
            
            tempDic[tempDice1]++;
            tempDic[tempDice2]++;
            tempDic[tempDice3]++;

            // historyScroll.CellData
        }

        return tempDic;
    }
    //public int GrabTotalDiceCount()
    //{
    //    int tempInt = 0;
    //    foreach(var item in diceCount)
    //    {
    //        tempInt += item.Value;
    //    }
    //    return tempInt;
    //}
    #endregion

    #region panel related
    public void OnPanelOpen()
    {
        bIsOpened = !bIsOpened;
        if(bIsOpened == false)
        {
            historyScroll.ReloadData(true);
        }
    }
    public void UpdateContents()
    {
        if(historyScroll.CellData.Count >= 100)
        {
            // var 
        }
    }

    public int currentGameIndex = -1;
    /// <summary>
    /// from result_dice step...
    /// </summary>
    /// <param name="dice1"></param>
    /// <param name="dice2"></param>
    /// <param name="dice3"></param>
    /// <param name="winMoney"></param>
    public void AddHistory(int gameIndex, eAD_DICE dice1, eAD_DICE dice2, eAD_DICE dice3, long winMoney)
    {
        // var tempRec = new R_09_HISTORY();
        st09_HISTORY tempHis = new st09_HISTORY();
        // st09_ModHistory tempHis = new st09_ModHistory();
        // tempHis.stGAME_IDX = currentGameIndex;// (long)(0 + i);
        tempHis.stGAME_IDX = gameIndex;
        var tempInt = UnityEngine.Random.Range(1, 7);
        tempHis.nDICE1 = (int)dice1;// UnityEngine.Random.Range(1, 7); 
        tempHis.nDICE2 = (int)dice2;// UnityEngine.Random.Range(1, 7); 
        tempHis.nDICE3 = (int)dice3;// UnityEngine.Random.Range(1, 7); 
        // tempRec.lHISTORYS.Add()
        // currentGameIndex++;

        tempHis.stRESULTMONEY = winMoney;

        // var temp = historyScroll.CellData;
        if(historyScroll.CellData.Count >= HISTORY_ITEM_LIMIT)
        {
            DeleteExceedData();
        }
        historyScroll.CellData.Add(tempHis);

        ReorderScroll();

        historyScroll.ReloadData(false);

        SetPercentage();
        // currentDice[0].sprite = currentDiceSprites[(int)dice1 - 1];
        // currentDice[1].sprite = currentDiceSprites[(int)dice2 - 1];
        // currentDice[2].sprite = currentDiceSprites[(int)dice3 - 1];

        foreach (var die in currentDice)
        {
            
        }

    }

    public void AddHistoryWithoutUpdate(int gameIndex, eAD_DICE dice1, eAD_DICE dice2, eAD_DICE dice3, long winMoney)
    {

        st09_HISTORY tempHis = new st09_HISTORY();


        tempHis.stGAME_IDX = gameIndex;
        // var tempInt = UnityEngine.Random.Range(1, 7);
        tempHis.nDICE1 = (int)dice1;// UnityEngine.Random.Range(1, 7); 
        tempHis.nDICE2 = (int)dice2;// UnityEngine.Random.Range(1, 7); 
        tempHis.nDICE3 = (int)dice3;// UnityEngine.Random.Range(1, 7); 
        // tempRec.lHISTORYS.Add()
        // currentGameIndex++;
        Debug.LogError("[AddHistory], " + " game history index is " + gameIndex
            + " dice /1/ " + dice1.ToString()
            + " /2/ is " + dice2.ToString()
            + " /3/ is " + dice3.ToString()
            + " cell data count " + historyScroll.CellData.Count);

        tempHis.stRESULTMONEY = winMoney;

        historyScroll.CellData.Add(tempHis);

    }

    public void UpdateHistory()
    {
        var temp = historyScroll.CellData;

        var standardIndex = 1;

        var assertionIndex = 200;
        var assertionCount = 0;
        while (HISTORY_ITEM_LIMIT < historyScroll.CellData.Count)
        {
            DeleteExceedData();
            assertionCount++;
            if (assertionIndex <= assertionCount)
            {
                Debug.LogError("assertion failed");
                break;
            }
        }
        //for (int i = 0; i < standardIndex; i++)
        //{
        //    DeleteExceedData();
        //}

        //if (historyScroll.CellData.Count > 1)
        //{
        //    DeleteExceedData();
        //}
        // historyScroll.CellData.Add(tempHis);

        ReorderScroll();

        historyScroll.ReloadData(false);

        SetPercentage();
    }
    /// <summary>
    /// from result step...
    /// </summary>
    /// <param name="gameIndex"></param>
    /// <param name="winMoney"></param>
    public void ModifyMoneySprite(int gameIndex, long winMoney)
    {
        Debug.Log("history modifying index is " + gameIndex);
        var tempData2 = historyScroll.CellData;
        var tempData = historyScroll.CellData.Find(x => (int)x.stGAME_IDX == gameIndex);
        var tempData3 = historyScroll.CellData.Find(x => (int)x.stGAME_IDX == (gameIndex - 1));
        if (tempData != null)
        {
            tempData.stRESULTMONEY = winMoney;
        }
        else
        {
            if(tempData3 != null)
            {

                Debug.LogError("something goes weird, caught in tempdata3");
            }
            else
            {

                Debug.LogError("something goes wrong, gameindex  " + gameIndex
                        + " latest history index " + (int)historyScroll.CellData[0].stGAME_IDX);
            }
        }

        historyScroll.ReloadData(false);
    }
    
    [TestMethod]
    public void ReorderScroll()
    {
        var temp = historyScroll.CellData;
        var tempDataList = historyScroll.CellData.OrderByDescending(i => (long)i.stGAME_IDX).ToList();
        for (int i = 0; i < historyScroll.CellData.Count; i++)
        {
            historyScroll.CellData[i] = tempDataList[i];
        }
        
        // historyScroll.CellData = historyScroll.CellData.OrderByDescending( i => (long)i.stGAME_IDX).ToList();

        
        // historyScroll.CellData = historyScroll.CellData.OrderBy(i => (long)i.stGAME_IDX).ToList();
        
        //temp = historyScroll.CellData;
    }

    #endregion


    #region common method
    public void InitDiceCount(ref Dictionary<eAD_DICE, int> needToInitDic)
    {
        for (int i = 1; i <= diceCount.Count; i++)
        {
            if(needToInitDic.ContainsKey((eAD_DICE)(i) ) == false)
            {
                needToInitDic.Add((eAD_DICE)(i),0);
            }
        }
        // needToInitDic.Add(eAD_DICE._DICE1_GE, 0);
        // needToInitDic.Add(eAD_DICE._DICE2_MUL, 0);
        // needToInitDic.Add(eAD_DICE._DICE3_BAK, 0);
        // needToInitDic.Add(eAD_DICE._DICE4_HO, 0);
        // needToInitDic.Add(eAD_DICE._DICE5_DAK, 0);
        // needToInitDic.Add(eAD_DICE._DICE6_SAE, 0);
    }
    public int GrabTotalDiceCount(in Dictionary<eAD_DICE, int> paramDiceCounts)
    {
        var tempInt = 0;
        foreach(var die in paramDiceCounts)
        {
            tempInt += die.Value;
        }
        return tempInt;
    }
    public void SetCurrentGameIndex(int gameIndex)
    {
        currentGameIndex = gameIndex;
    }
    public void SetCurrentDiceValues(eAD_DICE dice1, eAD_DICE dice2, eAD_DICE dice3)
    {
        currentDiceValues.Add(dice1);
        currentDiceValues.Add(dice2);
        currentDiceValues.Add(dice3);
        // currentDiceValues[1] = dice2;
        // currentDiceValues[2] = dice3;
    }
    public void ResetCurrentDiceValue()
    {
        currentDiceValues.Clear();
    }
    [TestMethod]
    public void DeleteExceedData()
    {
        if (historyScroll.CellData.Count >= 100)
        {
            // var temp = historyScroll.CellData.OrderBy(x => x.stGAME_IDX);
            // var temp2 = temp.First();
            // var temp5 = historyScroll.CellData.Min(x => x.stGAME_IDX);

            // var temp3 = historyScroll.CellData.OrderBy(x => (long)x.stGAME_IDX);
            // var temp4 = historyScroll.CellData.OrderBy(x => x.stGAME_IDX > -1);


            // var temp6 = temp3.First();

            // var temp2 = historyScroll.CellData.OrderBy(x => x.stGAME_IDX > -1).First();

            // historyScroll.CellData.Remove(temp2);

            historyScroll.CellData.RemoveAt(historyScroll.CellData.Count - 1);

        }
    }

    #endregion

}
