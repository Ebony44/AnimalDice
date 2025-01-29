using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADResultPartInfoStoring : ResourceItemBase
{
    // 1. betting place
    public List<eADBetPlace> winBetPlace = new List<eADBetPlace>(capacity: 5);
    // 2. retrieve user translation
    // public Dictionary<int, Dictionary<eAD_BUTTONLIST, int>> winUsers = new Dictionary<int, Dictionary<eAD_BUTTONLIST, int>>(capacity: 11);

    // bet place -> user index -> how many chips -> use this dictionary for result part...
    public Dictionary<eADBetPlace, Dictionary<int, long>> winningUsersChips = new Dictionary<eADBetPlace, Dictionary<int, long>>();

    

    public Dictionary<eADBetPlace, long> winPlaceMoney = new Dictionary<eADBetPlace, long>();

    // 4. total Chips
    public Dictionary<eADBetPlace, Dictionary<eAD_BUTTONLIST, int>> totalChips = new Dictionary<eADBetPlace, Dictionary<eAD_BUTTONLIST, int>>();

    // board state
    public st09_TABLE_PART boardState;

    #region Setter variables
    // public Dictionary<eAD_BUTTONLIST, int> myRewardChips = new Dictionary<eAD_BUTTONLIST, int>();
    // public Dictionary<eADBetPlace, Dictionary<eAD_BUTTONLIST, int>> losePlaceChips = new Dictionary<eADBetPlace, Dictionary<eAD_BUTTONLIST, int>>();
    // public Dictionary<eADBetPlace, long> myBettingMoney = new Dictionary<eADBetPlace, long>(capacity: 21);
    #endregion

    #region test variables for job systems
    public int countThatUsed = 0;
    public bool bHasCopiedWinLoseChipInfo = false;
    #endregion

    public SkeletonGraphic winLoseSpine;

    #region result step effect related
    // public AnimationCurve dishAnimCurve;
    #endregion

    #region depending on chip counts
    public int currentLoseChips = 0;
    public int currentWinChips = 0;
    #endregion

    public SkeletonDataAsset multiplierSpine;

    public SkeletonDataAsset resultBoardSpine;

    public bool bHasMultiplierPlayed = false;

    public bool bHasResultBoardEffectPlayed = false;

    protected override void Start()
    {
        base.Start();
        // delete it after test
        // TestWinnerBetPlace();
        
        // delete it after packets are connected to server
        
        //CoroutineChain.Start
        //    .Wait(0.1f)
        //    .Call(() => TestWinnerBetPlace());

        // var skeletonanimation = GetComponent<SkeletonGraphic>();
        winLoseSpine.AnimationState.Start += delegate { winLoseSpine.Skeleton.SetToSetupPose(); };
        //

    }
    #region Setter methods
    public void SetWinBoardState(st09_TABLE_PART tablePart)
    {
        boardState = tablePart;

    }
    public void SetWinnerPlace(List<eADBetPlace> winBetPlace)
    {
        this.winBetPlace = winBetPlace;
        foreach(var place in winBetPlace)
        {
            winPlaceMoney.Add(place, 0);
        }
        
    }
    

    #endregion

    public void IncrementChipsOnBoard(Dictionary<eADBetPlace, Dictionary<eAD_BUTTONLIST, int>> targetDic, eADBetPlace betPlace, eAD_BUTTONLIST chipKind)
    {
        if (targetDic.ContainsKey(betPlace) == false)
        {

            targetDic.Add(betPlace, new Dictionary<eAD_BUTTONLIST, int> { { chipKind, 1 } });

        }
        else
        {
            if (targetDic[betPlace].ContainsKey(chipKind) == false)
            {
                targetDic[betPlace].Add(chipKind, 1);
            }
            else
            {
                targetDic[betPlace][chipKind]++;
            }

        }
    }

    public void IncrementTotalChipsOnBoard(eADBetPlace betPlace, eAD_BUTTONLIST chipKind)
    {
        // IncrementChipsOnBoard(totalChips, betPlace, chipKind);

    }
    public void IncrementTotalChipsOnBoard(int betPlace, int chipKind)
    {
        IncrementTotalChipsOnBoard((eADBetPlace)betPlace, (eAD_BUTTONLIST)chipKind);
        
    }
    
    #region test methods
    [TestMethod]
    public void TestIncrement(int iterationCount)
    {
        for (int i =0; i<iterationCount;++i)
        {
            var tempBetPlace = Random.Range(12, 17);
            var chipKind = Random.Range(1, 4);
            IncrementTotalChipsOnBoard((eADBetPlace)tempBetPlace, (eAD_BUTTONLIST)chipKind);
        }
    }

    [TestMethod]
    public void TestWinnerBetPlace()
    {
        winBetPlace.Clear();
        for (int i = 0; i < 3; ++i)
        {
            var tempRandomBettingPlaceIndex = UnityEngine.Random.Range(0, 21);
            var tempRandomBetPlace = ResourceContainer.Get<ADChipBettingManager>().betPlaceSizeList[tempRandomBettingPlaceIndex].betPlace;
            if(winBetPlace.Contains(tempRandomBetPlace) == false )
            {
                winBetPlace.Add(tempRandomBetPlace);
            }
            else
            {
                i--;
                continue;
            }
        }
        foreach (var place in winBetPlace)
        {
            winPlaceMoney.Add(place, 0);
        }

        // TODO: for test purpose, setting winner user's money as bigger than betting board's total money(chip)
        // winningUsersChips[]


    }



    [TestMethod]
    public (int,bool) TestMinusFromChipToWinInfo(eADBetPlace betPlace, eAD_BUTTONLIST bettingChip)
    {
        bool isCalculated = false;
        bool tempIsBetPlaceEmpty = ResourceContainer.Get<ADChipBettingManager>().CheckIsEmpty(winningUsersChips, betPlace);
        if(tempIsBetPlaceEmpty == false)
        {
            for (int i = 0; i < winningUsersChips[betPlace].Count; i++)
            {
                if (ResourceContainer.Get<ADChipBettingManager>().CheckIsEmpty(winningUsersChips[betPlace], i) == false)
                {
                    var tempMoney = ResourceContainer.Get<ADAnteDependSetting>().chipValueInThisRoom[bettingChip];
                    if (winningUsersChips[betPlace][i] >= tempMoney)
                    {

                        winningUsersChips[betPlace][i] -= tempMoney;
                        isCalculated = true;
                        return (i,isCalculated);
                    }
                }
            }
        }
        return (99,isCalculated);
    }


    #endregion

    #region common methods
    public void ResetForNewGame()
    {
        //winPlaceChips.Clear();
        winBetPlace.Clear();
        winningUsersChips.Clear();
        
        // totalChips.Clear();

        ResourcePool.ClearAll<ADResultGoldText>();
        // .. more?
        currentLoseChips = 0;
        currentWinChips = 0;

        bHasMultiplierPlayed = false;
        bHasResultBoardEffectPlayed = false;
}

    /// <summary>
    /// Dictionary setting for avoiding given key not found exception
    /// must call it after RESULT_DICE packet... or after any result dice decided
    /// </summary>
    public void InitWinningDic()
    {
        for (int j = 0; j < winBetPlace.Count; j++)
        {
            if (winningUsersChips.ContainsKey(winBetPlace[j]) == false)
            {
                winningUsersChips.Add(winBetPlace[j], new Dictionary<int, long>() { { 0, 0 } });

                for (int k = 0; k < 11; k++)
                {
                    if (winningUsersChips[winBetPlace[j]].ContainsKey(k) == false)
                    {

                        winningUsersChips[winBetPlace[j]].Add(k, 0);
                    }
                }
            }
            else
            {
                for (int k = 0; k < 11; k++)
                {
                    if (winningUsersChips[winBetPlace[j]].ContainsKey(k) == false)
                    {

                        winningUsersChips[winBetPlace[j]].Add(k, 0);
                    }
                }
            }
        }
        
        // return false;
    }
    public void SetWinningDic(eADBetPlace winBetPlace, int userIndex, long winMoney)
    {
        // public Dictionary<eADBetPlace, Dictionary<int, long>>
        winningUsersChips[winBetPlace][userIndex] = winMoney;
        

    }

    public void SetWinBetPlace(List<int> winBetPlaceList) // it will use on RESULT_DICE
    {
        Debug.Log("winning user set up!");
        foreach (var item in winBetPlaceList)
        {
            winBetPlace.Add((eADBetPlace)item);
        }
        
        InitWinningDic();
    }

    #endregion
    #region spine handle
    [TestMethod(false)]
    public void TestSpineWin(float alphaTime, int winAnimIndex)
    {
        TimeContainer t1 = new TimeContainer("ADspineAlphaTime", alphaTime);
        TimeContainer.ContainClear("ADspineAlphaTime");
        if(winLoseSpine.Skeleton != null)
        {
            //winLoseSpine.skeletonDataAsset = null;
            // winLoseSpine.Clear();
            winLoseSpine.AnimationState.SetEmptyAnimation(0, 0);
            winLoseSpine.AlphaTween(1f, t1);
            // winLoseSpine.Skeleton.a = 1f;
        }
        // winLoseSpine.AnimationState.SetAnimation(0, "win", false);
        // Sound.Instance.EffPlay("AD_Win");

        Sound.Instance.EffPlayWithForce("AD_Win");

        if (winAnimIndex == 2)
        {
            winLoseSpine.Play("win2", false);
        }
        else if(winAnimIndex == 3)
        {
            winLoseSpine.Play("win3", false);
        }

    }
    [TestMethod(false)]
    public void TestSpineLose(float alphaTime)
    {
        TimeContainer t1 = new TimeContainer("ADspineAlphaTime", alphaTime);
        TimeContainer.ContainClear("ADspineAlphaTime");
        if (winLoseSpine.Skeleton != null)
        {
            // winLoseSpine.skeletonDataAsset = null;
            // winLoseSpine.Clear();
            winLoseSpine.AnimationState.SetEmptyAnimation(0, 0);

            winLoseSpine.AlphaTween(1f, t1);
            // winLoseSpine.Skeleton.a = 1f;
        }
        // winLoseSpine.AnimationState.SetAnimation(1, "lose", false);
        winLoseSpine.Play("lose", false);
        // Sound.Instance.EffPlay("AD_Lose"); 
        Sound.Instance.EffPlayWithForce("AD_Lose");
    }
    [TestMethod(false)]
    public void TestSpineClear()
    {
        TimeContainer t1 = new TimeContainer("ADspineAlphaTime", 1f);
        winLoseSpine.AlphaTween(0f, t1);
        CoroutineChain.Start
            .Wait(t1.time)
            .Call(() => winLoseSpine.AnimationState.SetEmptyAnimation(0, 0));
        

        // winLoseSpine.Skeleton.a = 0f;
    }
    [TestMethod(false)]
    public void TestBlock(bool bEnable)
    {
        ResourceContainer.Get<UnityEngine.UI.Image>("ADTipBlock").gameObject.SetActive(bEnable);
        // ResourceContainer.Get<UnityEngine.UI.Image>("ADTipBlock").gameObject.SetActive(true);
    }



    #region result board spine effect
    public void PlayAnimalBoard(int effectIndex)
    {
        var tempManager = ResourceContainer.Get<ADChipBettingManager>();
        var tempBoard = tempManager.GetBetBoard((eADBetPlace)effectIndex);

        var tempData = new EffectData(
                asset: ResourceContainer.Get<ADResultPartInfoStoring>().resultBoardSpine,
                animationName: ("animal_" + (effectIndex / 10).ToString()),
                // animationName: ("win_board_" + (betPlace / 10).ToString()),
                scaleFactor: 1f,
                tag: "ADResultSpine"
                );
        tempData.hideType = ESpineHide.Alpha;
        var item = ResourcePool.Pop<ADSpineBoardResultEffectItem, EffectData>(tempData);
        item.SetSortingLayer("WorldForward", 5052);
        item.transform.position = tempBoard.transform.position;

        item.Play();
        // item.PlayWithLoop();




        if (ResourceContainer.Get<ADResultPartInfoStoring>().bHasResultBoardEffectPlayed == false)
        {
            Sound.Instance.EffPlayWithForce("AD_Triumphant1", 1);
            ResourceContainer.Get<ADResultPartInfoStoring>().bHasResultBoardEffectPlayed = true;
        }
    }
    public void PlayAnimalEdgeBoard(int effectIndex)
    {
        var tempManager = ResourceContainer.Get<ADChipBettingManager>();
        var tempBoard = tempManager.GetBetBoard((eADBetPlace)effectIndex);
        #region
        var tempData2 = new EffectData(
                asset: ResourceContainer.Get<ADResultPartInfoStoring>().resultBoardSpine,
                // animationName: ("animal_" + (effectIndex / 10).ToString()),
                animationName: "win_board_1",
                scaleFactor: 1f,
                tag: "ADResultSpine"
                );
        tempData2.hideType = ESpineHide.Alpha;
        var item2 = ResourcePool.Pop<ADSpineBoardResultEffectItem, EffectData>(tempData2);
        // item2.SetSortingLayer("WorldForward", 5051);
        item2.SetSortingLayer("WorldForward", 29);
        item2.transform.position = tempBoard.transform.position;

        item2.Play();
        // item2.PlayWithLoop();
        #endregion
        if (ResourceContainer.Get<ADResultPartInfoStoring>().bHasResultBoardEffectPlayed == false)
        {
            Sound.Instance.EffPlayWithForce("AD_Triumphant1", 1);
            ResourceContainer.Get<ADResultPartInfoStoring>().bHasResultBoardEffectPlayed = true;
        }
    }
    public void PlayGapSideBoard(int effectIndex, int betBoardIndex)
    {
        var tempManager = ResourceContainer.Get<ADChipBettingManager>();
        var tempBoard = tempManager.GetBetBoard((eADBetPlace)betBoardIndex);

        var tempData = new EffectData(
                asset: ResourceContainer.Get<ADResultPartInfoStoring>().resultBoardSpine,
                // animationName: ("animal_" + (betPlace / 10).ToString()),
                animationName: ("win_board_" + effectIndex.ToString()),
                scaleFactor: 1f,
                tag: "ADResultSpine"
                );
        tempData.hideType = ESpineHide.Alpha;
        var item = ResourcePool.Pop<ADSpineBoardResultEffectItem, EffectData>(tempData);
        // item.SetSortingLayer("WorldForward", 5051);
        item.SetSortingLayer("WorldForward", 29);
        item.transform.position = tempBoard.transform.position;
        // item.PlayWithLoop();
        item.Play();
        if (ResourceContainer.Get<ADResultPartInfoStoring>().bHasResultBoardEffectPlayed == false)
        {
            Sound.Instance.EffPlayWithForce("AD_Triumphant1", 1);
            ResourceContainer.Get<ADResultPartInfoStoring>().bHasResultBoardEffectPlayed = true;
        }
    }

    public int CategorizeGapSideBoard(int boardIndex)
    {
        var betPlaceSize = ResourceContainer.Get<ADChipBettingManager>().CategorizeBetPlace((eADBetPlace)boardIndex);
        switch (betPlaceSize)
        {
            case eAD_BettingPlaceSize.BIG_BOARD:
                return 1;

            case eAD_BettingPlaceSize.SMALL_UPLOW_BOARD:
                return 2;

            case eAD_BettingPlaceSize.GAP_VERTICAL_BOARD:
                return 4;

            case eAD_BettingPlaceSize.GAP_HORIZONTAL_BOARD:
                return 3;

            case eAD_BettingPlaceSize._PLACESIZE_MAX:
                return -1;
            default:
                return -1;

        }
    }
    #endregion


    #endregion

}

public class WinUserInfo
{
    // public Dictionary<int, Dictionary<eADBetPlace, Dictionary<eAD_BUTTONLIST, int>>> winningUsersChips = new Dictionary<int, Dictionary<eADBetPlace, Dictionary<eAD_BUTTONLIST, int>>>();
    public Dictionary<eADBetPlace, Dictionary<eAD_BUTTONLIST, int>> winningUsersChips = new Dictionary<eADBetPlace, Dictionary<eAD_BUTTONLIST, int>>();
    public eADBetPlace betPlace;
    public eAD_BUTTONLIST chipKind;
    public int userIndex;
}