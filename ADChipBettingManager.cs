using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using OneLine;
using Unity.Entities;
using Spine.Unity;

public class ADChipBettingManager : ResourceItemBase// MonoSingleton<ADChipManager>
{
    #region test purpose only
    //public Transform testBettingStartPos;
    //public Transform testBettingEndPos;
    
    //public float range = 8f;

    //public eAD_BUTTONLIST chipKind;

    //public eADBetPlace chipBettingPos;

    #endregion


    public bool bCanMove = true; // always true for moving?... from ADChipMovementSystem

    public float chipMovingTime = 1f;


    #region component system related, Handling

    public float destroyingTime = 0.16f;

    private const int currentChipDepth = 31;
    public Dictionary<eADBetPlace, int> bettingChipDepths = new Dictionary<eADBetPlace, int>();

    #endregion

    #region result part, componentsystem handling
    [Header("boolean member variables for componentsystem handling")]
    public bool bCanMoveToDealer = false; // from ADChipMovementSystem
    public bool bCanMarkChips = false; // decide which chips move to which player... from ADChipMarkSystem
    public bool bCanMoveToBoard = false; // create lacking board chips at this boolean state... from ADChipCreatingSystem
    public bool bCanMoveToWinPlayer = false; // move marked chips to players from ADChipMovementSystem

    public bool bDestroyingChipEntities = false;

    public bool bDestroyingAllChipEntitiesWithAlpha = false;
    public bool bDestroyingOnlyLoseChipsWithAlpha = false;

    
    public bool bDestroyChipImmediately = false;

    #endregion

    #region variables for Packet sending
    public long gameIndex;
    // public eADBetPlace bettingPosition;
    public eAD_BUTTONLIST currentButtonIndex;
    #endregion

    #region UI Related
    public Button[] bettingChipButtons;
    public GameObject[] chipButtonHighlights;
    #endregion

    #region betting opening, ending spines
    public SkeletonGraphic bettingOpenEndSpine;
    public bool bBettingSpinePlayed = false;
    #endregion

    [OneLineWithHeader]
    [HideLabel]
    public List<BetPlaceSizeDataStore> betPlaceSizeList;

    public Dictionary<eADBetPlace, long> myBettingMoney = new Dictionary<eADBetPlace, long>(capacity: 21);

    public int currentGameIndex = -1;

    // my bet money(for client side betting block...)
    public long myTotalBettingMoney = 0;
    public bool[] bBettingAsPreviousArray = new bool[11];

    #region sound for previous betting
    private List<Coroutine> bettingSoundPlayRoutines = new List<Coroutine>(11);
    private List<TimeContainer> bettingSoundTCs = new List<TimeContainer>(11);
    private Dictionary<int, int> bettingPrevChipCounts = new Dictionary<int, int>();
    #endregion

    #region variable for betting sound handle
    private List<TimeContainer> TestTimeStack = new List<TimeContainer>(5);
    #endregion

    #region All in related data
    public SkeletonDataAsset AllInSpine;
    public Dictionary<int, bool> AllInActiveDic = new Dictionary<int, bool>(11);
    #endregion

    #region Purformance Handle related variables
    // public Coroutine bettingSkipRoutine;
    [HideInInspector] public int totalBettingCount = 0;

    public bool bIsBettingRoutineOff = true;
    public bool bIsBettingTime = false;
    // over or equal to 4 in 0.4 sec.
    [SerializeField]
    private float bettingDecimateTime = 0.5f;
    [SerializeField]
    private int bettingDemcimateCount = 3;

    // skip count
    [HideInInspector] public int bettingSkipCount = 0;
    private int bettingSkipLimitCount = 4;
    private int bettingMaximumLimitCount = 6;
    

    private int totalSoundCount = 0;
    public bool bIsSoundRoutineOff = true;

    [SerializeField]
    private float soundDecimateTime = 0.5f;
    [SerializeField]
    private int soundDemcimateCount = 4;
    // skip count
    private int soundSkipCount = 0;
    private int soundSkipLimitCount = 3;




    #endregion

    #region cached
    // GetChipsFromMoney
    // Dictionary<eAD_BUTTONLIST, int> chipDic = new Dictionary<eAD_BUTTONLIST, int>(capacity: 4);
    #endregion
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        InitializePrevChips();
        
        // ADChipMovementSystem.ExecutingSystemType
        CoroutineChain.Start
            .Wait(0.01f)
            .Call(() => OnChipSelecting(eAD_BUTTONLIST._BTN_BETTING_1));

        TestTimeStack.Add(new TimeContainer("BettingSoundDelayingTime", 0.5f));
        forbiddenButton.gameObject.SetActive(false);

    }

    public void InitializePrevChips()
    {
        for (int i = 0; i < 11; i++)
        {
            bettingSoundPlayRoutines.Add(null);
            bettingSoundTCs.Add(0);
            bettingPrevChipCounts.Add(i, 0);
        }
    }

    #region my bet, others bet
    public void SetMyBettingMoney(eADBetPlace betPlace, long betMoney)
    {
        
        if (myBettingMoney.ContainsKey(betPlace) == false)
        {
            myBettingMoney.Add(betPlace, betMoney);
        }
        else
        {
            myBettingMoney[betPlace] += betMoney;
        }
        var betBoard = GetBetBoard(betPlace);
        betBoard.myBettingTotalText.text = myBettingMoney[betPlace].ToStringWithKMB();
        if(myBettingMoney[betPlace] > 0)
        {
            EnableMyBettingMoneyLabel(betPlace, true);
        }
        
        // ResourceContainer.Get<ADBettingBoardTag>().
    }
    public void EnableMyBettingMoneyLabel(eADBetPlace betPlace, bool enable)
    {
        var betBoard = GetBetBoard(betPlace);
        TimeContainer.ContainClear("bettingBoardAlphaTween");
        betBoard.myBetTotalTextObject.SetActive(true);
        var image = betBoard.myBettingLabel;
        if (enable)
        {

            TimeContainer t1 = new TimeContainer("bettingBoardAlphaTween", 0.01f);
            if(image.color.a != 1f)
            {
                image.AlphaTween(1f, t1, true);
            }
        }
        else
        {
            // betBoard.myBetTotalTextObject.SetActive(false);

            TimeContainer t1 = new TimeContainer("bettingBoardAlphaTween", 0.01f);
            if (image.color.a != 0f)
            {
                image.AlphaTween(0f, t1, true);
            }
            // betBoard.myBettingTotalText.text = string.Empty;
        }
    }

    public void EnableMyBettingMoneyLabel(eADBetPlace betPlace, bool enable, float time)
    {
        var betBoard = GetBetBoard(betPlace);
        TimeContainer.ContainClear("bettingBoardAlphaTween");
        // var image = betBoard.myBetTotalTextObject.GetComponent<Image>();
        var image = betBoard.myBettingLabel;
        betBoard.myBetTotalTextObject.SetActive(true);
        if (enable)
        {
            // "bettingBoardAlphaTween"
            TimeContainer t1 = new TimeContainer("bettingBoardAlphaTween", time);

            if (image.color.a != 1f)
            {
                // betBoard.myBetTotalTextObject.GetComponent<Image>().AlphaTween(1f, t1, true);
                image.AlphaTween(1f, t1, true);
            }
        }
        else
        {
            // betBoard.myBetTotalTextObject.SetActive(false);
            TimeContainer t1 = new TimeContainer("bettingBoardAlphaTween", time);
            
            if (image.color.a != 0f)
            {
                image.AlphaTween(0f, t1, true);
            }
            // betBoard.myBettingTotalText.text = string.Empty;
        }
    }

    public void DisableAllMyBettingMoneyLabel()
    {
        // Debug.Log("disable all betting board money text, dictionary count is " + myBettingMoney.Count);
        foreach (var item in myBettingMoney)
        {
            // Debug.Log("disable betting board " + item.Key.ToString());
            EnableMyBettingMoneyLabel(item.Key, false);
            
        }
        myBettingMoney.Clear();
        
        
    }

    public void DisableAllMyBettingMoneyLabel(float time)
    {
        foreach (var item in myBettingMoney)
        {
            // Debug.Log("disable betting board " + item.Key.ToString());
            EnableMyBettingMoneyLabel(item.Key, false, time);

        }
        myBettingMoney.Clear();
    }
    [TestMethod]
    public void DisableAllBettingBoardHighlight()
    {
        TimeContainer.ContainClear("allMyBettingBoardHighLight");
        TimeContainer.ContainClear("TABLE_HIGHLIGHT_ON");
        TimeContainer.ContainClear("TABLE_HIGHLIGHT_OFF");


        foreach (var bettingBoard in betPlaceSizeList)
        {
            TimeContainer t1 = new TimeContainer("allMyBettingBoardHighLight", 0.5f);
            GetBetBoard(bettingBoard.betPlace).highlightBettingBoard.AlphaTween(0f, t1);
        }
    }
    public void Rec_MyBetting(eADBetPlace betPlace, eAD_BUTTONLIST chipKind, long betMoney)
    {
        #region set betting money for betting board text
        SetMyBettingMoney(betPlace, betMoney);
        #endregion
        if (eAD_BUTTONLIST._BTN_BETTING_1 <= chipKind && chipKind <= eAD_BUTTONLIST._BTN_BETTING_4)
        {
            

            var currentChips = GetChipsFromMoney(betMoney);
            Debug.Log("R_09_BET, my betting, current bet pos " + betPlace.ToString()
                + " betmoney " + betMoney
                + " chipkind " + chipKind.ToString()
                + " current chip count " + currentChips.Count);

            IncrementMyTotalBettingChip(betMoney);

            foreach (var item in currentChips) // from chip kind's count
            {

                
                if(item.Value != 0)
                {
                    ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().SpawnChipsToSpawnPos(
                        // chipStartPos: bettingChipButtons[4].transform.position,
                        chipStartPos: bettingChipButtons[(int)(chipKind - 1 - eAD_BUTTONLIST._BTN_NONE)].transform.position,
                        chipMidPos: Vector3.zero,
                        chipTargetPos: GetTablePositionFromBetPlace(betPlace),
                        paramBettingPlace: betPlace,
                        paramChipKind: item.Key,
                        bettingCount: item.Value
                        );
                }

            }

            #region obsolete....

            // Debug.Log("R_09_BET, my betting, chipkind " + chipKind.ToString());
            //ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().SpawnChipsToSpawnPos(bettingChipButtons[(int)(chipKind - 1 - eAD_BUTTONLIST._BTN_NONE)].transform.position,
            //    new float3(0, 0, 0),
            //    chipTargetPos: GetBetBoard(betPlace).transform.position,
            //    paramChipKind: chipKind,
            //    paramBettingPlace: betPlace);

            //ResourceContainer.Get<ADResultPartInfoStoring>().IncrementTotalChipsOnBoard(betPlace, chipKind);
            #endregion
            // PlayBettingSound(1, true);
        }
        else
        {
            var currentChips = GetChipsFromMoney(betMoney);
            Debug.Log("R_09_BET, my previous betting, current bet pos " + betPlace.ToString()
                + " betmoney " + betMoney
                + " chipkind " + chipKind.ToString()
                + " current chip count " + currentChips.Count);
            // int tempChipCount = 0;

            IncrementMyTotalBettingChip(betMoney);
            foreach (var item in currentChips) // from chip kind's count
            {
                
                // IncrementMyTotalBettingChip(item.Key,item.Value);
                ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().SpawnChipsToSpawnPos(
                    chipStartPos: bettingChipButtons[4].transform.position,
                    chipMidPos: Vector3.zero,
                    chipTargetPos: GetTablePositionFromBetPlace(betPlace),
                    paramBettingPlace: betPlace,
                    paramChipKind: item.Key,
                    bettingCount: item.Value
                    );

                // ResourceContainer.Get<ADResultPartInfoStoring>().IncrementTotalChipsOnBoard(betPlace, item.Key);
                // tempChipCount++;
            }

            #region sound related, obsolete
            //// PlayBettingSound(tempChipCount, true); // only play it once, do not play per betting boards
            //if (bBettingAsPreviousArray[0] == false)
            //{
            //    // PlayBettingSound(tempChipCount, true); // only play it once, do not play per betting boards
            //    // bBettingAsPreviousArray[0] = true;
            //}
            //bettingPrevChipCounts[0] += 1;
            //if (bettingSoundPlayRoutines[0] == null)
            //{
            //    bettingSoundTCs[0] = new TimeContainer("ADPrevChipStackingForSound", 0.75f);
                
            //    bettingSoundPlayRoutines[0] = StartCoroutine(PlayPrevBettingSound(bettingSoundTCs[0], 0));
            //}
            //else
            //{
            //    Debug.Log("sound eff, soundTC's t " + bettingSoundTCs[0].t);
            //    bettingSoundTCs[0].t -= 0.11f;
            //    if(bettingSoundTCs[0].t < 0)
            //    {
            //        bettingSoundTCs[0].t = 0;
            //    }
            //}
            #endregion

        }

        
    }

    
    public void OnBetting(string id)
    {

        var bettingPosObject = ResourceContainer.Get<ADBettingBoardTag>(id);//.GetComponent<ADBettingBoardTag>();
        eADBetPlace bettingPos = eADBetPlace._BET_ANIMAL_GE;
        var anteSetting = ResourceContainer.Get<ADAnteDependSetting>();


        if (bettingPosObject != null)
        {

            Debug.Log("betting place is " + bettingPosObject.bettingPlace.ToString());
            bettingPos = bettingPosObject.bettingPlace;
        }
        

        if (myTotalBettingMoney >= (long)cGlobalInfos.GetIntoRoomInfo_97().stMAX_BETMONEY )// client side block
        {
            
            return;
        }

        if (bettingChipButtons[(int)(currentButtonIndex - eAD_BUTTONLIST._BTN_BETTING_1)].interactable == false)// server side block
        {
            
            return;
        }
        Debug.Log("gamesocket is " + GameSocket.isActive);
        // if server is dead? 
        //if (GameSocket.isActive == false)
        //{
        //    if (eAD_BUTTONLIST._BTN_BETTING_1 <= currentButtonIndex && currentButtonIndex <= eAD_BUTTONLIST._BTN_BETTING_4)
        //    {
        //        // myTotalBettingMoney += GetMoneyFromChip(currentButtonIndex);
        //        // IncrementMyTotalBettingChip(currentButtonIndex); // Jones: obsolete for btn 1 ~ btn 4's functions are technically eqaul to btn 5 now...
        //    }

        //    Req_Betting(bettingPos, currentButtonIndex); // only this thing
        //    return; // ...is this enough?
        //}
        

        // pre-Error handle?, when select nothing...
        if (currentButtonIndex <= eAD_BUTTONLIST._BTN_NONE || currentButtonIndex >= eAD_BUTTONLIST._BTN_MAX)
        {
            return;
            // currentButtonIndex = eAD_BUTTONLIST._BTN_BETTING_1;
        }
        if (anteSetting.chipValueInThisRoom[eAD_BUTTONLIST._BTN_BETTING_1] == -1 ) // if values are not initialized
        {
            PlayForbiddenSpine();
            return;
        }

        if (eAD_BUTTONLIST._BTN_BETTING_1 <= currentButtonIndex && currentButtonIndex <= eAD_BUTTONLIST._BTN_BETTING_4)
        {
            // myTotalBettingMoney += GetMoneyFromChip(currentButtonIndex);
            // IncrementMyTotalBettingChip(currentButtonIndex);
        }

        // if lower than my money
        Debug.Log("AD_Betting before send, My Have money is " + ResourceContainer.Get<ADMyInfoTag>("MyInfo").haveMoney
            + "  current ante selected " + anteSetting.chipValueInThisRoom[(eAD_BUTTONLIST)currentButtonIndex]);
        if (ResourceContainer.Get<ADMyInfoTag>("MyInfo").haveMoney < anteSetting.chipValueInThisRoom[eAD_BUTTONLIST._BTN_BETTING_1])
        {
            PlayForbiddenSpine();
            return;
        }

        Req_Betting(bettingPos, currentButtonIndex); // only this thing

        // delete below code when packet connecting's done
        //ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().SpawnChipsToSpawnPos(bettingChipButtons[(int)(currentButtonIndex -1 -eAD_BUTTONLIST._BTN_NONE)].transform.position, 
        //    new float3(0, 0, 0), 
        //    chipTargetPos: bettingPosObject.transform.position, 
        //    paramChipKind: currentButtonIndex, 
        //    paramBettingPlace: bettingPos);
        // 

        // ResourceContainer.Get<ADResultPartInfoStoring>().IncrementTotalChipsOnBoard(bettingPos,currentButtonIndex);

    }
    

    public void Rec_BettingFromOthers(Vector3 chipStartPos, eADBetPlace betPlace, eAD_BUTTONLIST chipKind, long betMoney, int playerIndex)
    {
        if (eAD_BUTTONLIST._BTN_BETTING_1 <= chipKind && chipKind <= eAD_BUTTONLIST._BTN_BETTING_4)
        {

            //var endPos = GetTablePositionFromBetPlace(betPlace);
            //ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().SpawnChipsToSpawnPos(chipStartPos,
            //    new float3(0, 0, 0),
            //    chipTargetPos: endPos,
            //    paramChipKind: chipKind,
            //    paramBettingPlace: betPlace);
            var currentChips = GetChipsFromMoney(betMoney);
            foreach (var item in currentChips) // from chip kind's count
            {


                if (item.Value != 0)
                {
                    //ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().SpawnChipsToSpawnPos(
                    //    // chipStartPos: bettingChipButtons[4].transform.position,
                    //    chipStartPos: bettingChipButtons[(int)(chipKind - 1 - eAD_BUTTONLIST._BTN_NONE)].transform.position,
                    //    chipMidPos: Vector3.zero,
                    //    chipTargetPos: GetTablePositionFromBetPlace(betPlace),
                    //    paramBettingPlace: betPlace,
                    //    paramChipKind: item.Key,
                    //    bettingCount: item.Value
                    //    );
                    ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().SpawnChipsToSpawnPos(
                    chipStartPos: chipStartPos,
                    chipMidPos: Vector3.zero,
                    chipTargetPos: GetTablePositionFromBetPlace(betPlace),
                    paramBettingPlace: betPlace,
                    paramChipKind: item.Key,
                    bettingCount: item.Value
                    );
                }

            }

        }
        else
        {
            var currentChips = GetChipsFromMoney(betMoney);
            int tempChipCount = 0;
            foreach (var item in currentChips) // from chip kind's count
            {

                ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().SpawnChipsToSpawnPos(
                    chipStartPos: chipStartPos,
                    chipMidPos: Vector3.zero,
                    chipTargetPos: GetTablePositionFromBetPlace(betPlace),
                    paramBettingPlace: betPlace,
                    paramChipKind: item.Key,
                    bettingCount: item.Value
                    );

                // ResourceContainer.Get<ADResultPartInfoStoring>().IncrementTotalChipsOnBoard(betPlace, item.Key);
                tempChipCount++;
            }
            #region sound related, obsolete
            // PlayBettingSound(tempChipCount, false);
            //if (bBettingAsPreviousArray[playerIndex] == false)
            //{
            //    // PlayBettingSound(tempChipCount, true); // only play it once, do not play per betting boards
            //    // bBettingAsPreviousArray[playerIndex] = true;
            //}

            //bettingPrevChipCounts[playerIndex] += 1;
            //if (bettingSoundPlayRoutines[playerIndex] == null)
            //{
            //    bettingSoundTCs[playerIndex] = new TimeContainer("ADPrevChipStackingForSound", 0.75f);
                
            //    bettingSoundPlayRoutines[playerIndex] = StartCoroutine(PlayPrevBettingSound(bettingSoundTCs[playerIndex], playerIndex));
            //}
            //else
            //{
            //    bettingSoundTCs[playerIndex].t -= 0.11f;
            //    if (bettingSoundTCs[0].t < 0)
            //    {
            //        bettingSoundTCs[0].t = 0;
            //    }
            //}
            #endregion


        }
    }

    
    // private 
    //private IEnumerator PlayPrevBettingSound(TimeContainer t1, int playerIndex)
    //{
        
    //    while(t1.t < 1)
    //    {
    //        yield return null;
    //        t1.t += Time.deltaTime / t1.time;
    //        if(t1.t > 1f)
    //        {
    //            t1.t = 1f;
    //        }
    //    }
    //    PlayBettingSound(bettingPrevChipCounts[playerIndex], false);
    //    Debug.Log("sound eff wating complete, prev chip sound played, betting chip count " + bettingPrevChipCounts[playerIndex]);
    //    // Sound.Instance.EffPlay("");
    //    bettingSoundPlayRoutines[playerIndex] = null;
    //    bettingSoundTCs[playerIndex] = null;
    //    bettingPrevChipCounts[playerIndex] = 0;
        
    //}

    #endregion

    public void OnChipSelecting(int index)
    {
        
        Debug.Log(index + " " + eAD_BUTTONLIST._BTN_NONE);
        // currentButtonIndex = (eAD_BUTTONLIST)(index - eAD_BUTTONLIST._BTN_NONE);
        currentButtonIndex = (eAD_BUTTONLIST)(index);
        if ((currentButtonIndex) > 0) // first based
        {
            chipButtonHighlights[(int)(currentButtonIndex -1 -eAD_BUTTONLIST._BTN_NONE)].SetActive(true);
            
            chipButtonHighlights[(int)(currentButtonIndex - 1 - eAD_BUTTONLIST._BTN_NONE)].transform.localScale = Vector3.one * 1.2f;
            bettingChipButtons[(int)(currentButtonIndex - 1 - eAD_BUTTONLIST._BTN_NONE)].transform.localScale = Vector3.one * 1.2f;

            for (int i = 0; i < chipButtonHighlights.Length; ++i)
            {
                if (i == (int)(currentButtonIndex -1 -eAD_BUTTONLIST._BTN_NONE))
                {
                    continue;
                }
                chipButtonHighlights[i].SetActive(false);
                chipButtonHighlights[i].transform.localScale = Vector3.one;
                bettingChipButtons[i].transform.localScale = Vector3.one;
            }
        }
        else
        {
            currentButtonIndex = (eAD_BUTTONLIST)(eAD_BUTTONLIST._BTN_BETTING_1 - eAD_BUTTONLIST._BTN_NONE);
            chipButtonHighlights[0].SetActive(true);
            
        }

    }

    public void OnChipSelecting(eAD_BUTTONLIST index)
    {

        Debug.Log(index + " " + eAD_BUTTONLIST._BTN_NONE);
        // currentButtonIndex = (eAD_BUTTONLIST)(index - eAD_BUTTONLIST._BTN_NONE);
        currentButtonIndex = (eAD_BUTTONLIST)(index);
        if ((currentButtonIndex) > 0) // first based
        {
            chipButtonHighlights[(int)(currentButtonIndex - 1 - eAD_BUTTONLIST._BTN_NONE)].SetActive(true);

            chipButtonHighlights[(int)(currentButtonIndex - 1 - eAD_BUTTONLIST._BTN_NONE)].transform.localScale = Vector3.one * 1.2f;
            bettingChipButtons[(int)(currentButtonIndex - 1 - eAD_BUTTONLIST._BTN_NONE)].transform.localScale = Vector3.one * 1.2f;

            for (int i = 0; i < chipButtonHighlights.Length; ++i)
            {
                if (i == (int)(currentButtonIndex - 1 - eAD_BUTTONLIST._BTN_NONE))
                {
                    continue;
                }
                chipButtonHighlights[i].SetActive(false);
                chipButtonHighlights[i].transform.localScale = Vector3.one;
                bettingChipButtons[i].transform.localScale = Vector3.one;
            }
        }
        else
        {
            currentButtonIndex = (eAD_BUTTONLIST)(eAD_BUTTONLIST._BTN_BETTING_1 - eAD_BUTTONLIST._BTN_NONE);
            chipButtonHighlights[0].SetActive(true);

        }

    }


    public void OnPrevButtonTapped(eAD_BUTTONLIST index)
    {
        Req_Betting(betPosIdx: eADBetPlace._BET_MAX, (eAD_BUTTONLIST)index);
        bettingChipButtons[4].interactable = false;
        bettingChipButtons[4].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);

    }
    public void Req_Betting(eADBetPlace betPosIdx, eAD_BUTTONLIST betBtnIdx)
    {
        #region test purpose stopwatch
        //ResourceContainer.Get<ADGameMain>().bettingWatch.Start();
        //Debug.Log("my betting start, stopwatch started");
        #endregion
        var packet = new S_09_REQ_BET();
        Debug.Log("[S_09_REQ_BET] idx :" + betPosIdx.ToString());
        packet.stGAME_IDX = currentGameIndex;
        packet.nBETPOS = (int)betPosIdx;
        packet.nBTNIDX = (int)betBtnIdx;
        packet.send();
    }


    [TestMethod]
    public eAD_BettingPlaceSize CategorizeBetPlace(eADBetPlace betPlace)
    {
        
        var tempPlaceSize = eAD_BettingPlaceSize._PLACESIZE_MAX;
        tempPlaceSize = betPlaceSizeList.Find(x => x.betPlace == betPlace).bettingPlaceSize;

        return tempPlaceSize;
    }

    #region button related
    public void ButtonSet(List<stBUTTONSET> buttonstates)
    {
        for (int i = 0; i < buttonstates.Count; i++)
        {
            var currentButton = buttonstates[i];
            if (currentButton.nSTATE != (int)eAD_BTN_STATE._BTN_ENABLE)
            {
                bettingChipButtons[i].interactable = false;
                
                SetButtonHighlight(i, false);
                chipButtonHighlights[i].transform.localScale = Vector3.one;
                bettingChipButtons[i].transform.localScale = Vector3.one;

                bettingChipButtons[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);
            }
            else
            {
                bettingChipButtons[i].interactable = true;
                bettingChipButtons[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                
            }
        }
        
        bool tempAreAllHighlightOff = true;
        foreach (var highlight in chipButtonHighlights)
        {
            if (highlight.activeSelf)
            {
                tempAreAllHighlightOff = false;
            }
        }
        if (tempAreAllHighlightOff)
        {
            if( ResourceContainer.Get<ADChipBettingManager>().currentButtonIndex < eAD_BUTTONLIST._BTN_BETTING_1 ||
                ResourceContainer.Get<ADChipBettingManager>().currentButtonIndex > eAD_BUTTONLIST._BTN_BETTING_4)
            {
                ResourceContainer.Get<ADChipBettingManager>().currentButtonIndex = eAD_BUTTONLIST._BTN_BETTING_1;
            }
            var tempIndex = ResourceContainer.Get<ADChipBettingManager>().currentButtonIndex - eAD_BUTTONLIST._BTN_BETTING_1;
            if(bettingChipButtons[tempIndex].interactable)
            {
                SetButtonHighlight(tempIndex, true);
            }
        }

    }
    #endregion


    // public bool bHasBoardEnabled = true;

    #region panel, panel spine FX related
    [TestMethod]
    public void SetEnableBettingBoards(bool bEnable)
    {
        //if (bHasBoardEnabled == bEnable)
        //{
        //    return;
        //}

        Debug.Log("can bet? " + bEnable);

        for (int i = 0; i < betPlaceSizeList.Count; i++)
        {
            var board = GetBetBoard(betPlaceSizeList[i].betPlace);
            board.bettingBoard.GetComponent<Button>().interactable = bEnable;
        }
        forbiddenButton.gameObject.SetActive(!bEnable);
    }


    #region betting forbidden spine related
    // public SkeletonGraphic bettingOpenEndSpine;
    public SkeletonAnimation bettingForbiddenSpine;
    public Button forbiddenButton;
    //public bool bBettingForbiddenSpinePlayed = false;
    #endregion
    

    public void OnForbiddenButtonPressed()
    {
        // only show when betting turn.
        if(ADGameStepHandle.STEP == eAD_STEP._IS_AD_BET)
        {
            PlayForbiddenSpine();
        }
        
        Debug.Log("forbidden button pressed");
    }
    public void PlayForbiddenSpine()
    {
        Debug.Log("forbidden spine playing");
        bettingForbiddenSpine.Play("animation", false);
    }

    #endregion

    #region test methods

    [TestMethod(false)]
    public void BetWithoutServerConnected()
    {
        //ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().SpawnChipsToSpawnPos(bettingChipButtons[tempRandomBtnIdx].transform.position,
        //    new float3(0, 0, 0),
        //    chipTargetPos: bettingPosObject,
        //    paramChipKind: (eAD_BUTTONLIST)(tempRandomBtnIdx + 1),
        //    paramBettingPlace: tempRandomBetPlace,
        //    bettingCount: betCount);

    }

    /// <summary>
    /// For Test purpose on game. betting all boards on current button highlighted
    /// </summary>
    [TestMethod]
    public void BettingAllBoard()
    {
        foreach (var item in betPlaceSizeList)
        {
            Req_Betting(item.betPlace, currentButtonIndex);
        }
        // Req_Betting()
    }
    [TestMethod]
    public void BettingAsMany(int betPlace, int iterationCount)
    {
        for (int i = 0; i < iterationCount; i++)
        {
            // Debug.Log("current iteration count is " + i);
            IncrementMyTotalBettingChip(currentButtonIndex);
            Req_Betting((eADBetPlace._BET_ANIMAL_BAK), currentButtonIndex);
        }
    }

    [TestMethod]
    public void TestDistributeChipInRandom(int betCount)
    {
        var tempRandomBtnIdx = UnityEngine.Random.Range(0, 4);
        var tempRandomBettingPlaceIndex = UnityEngine.Random.Range(0, 21);
        
        var tempRandomChipKind = UnityEngine.Random.Range((int)eAD_BUTTONLIST._BTN_BETTING_1, (int)eAD_BUTTONLIST._BTN_BETTING_4);

        var tempRandomBetPlace = betPlaceSizeList[tempRandomBettingPlaceIndex].betPlace;
        var bettingPosObject = GetTablePositionFromBetPlace(tempRandomBetPlace);
        // ResourceContainer.Get<>

        ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().SpawnChipsToSpawnPos(bettingChipButtons[tempRandomBtnIdx].transform.position,
            new float3(0, 0, 0),
            chipTargetPos: bettingPosObject,
            paramChipKind: (eAD_BUTTONLIST)tempRandomChipKind,
            paramBettingPlace: tempRandomBetPlace,
            bettingCount: betCount);
        if((eAD_BUTTONLIST)(tempRandomBtnIdx + 1) == 0 || tempRandomBetPlace == eADBetPlace._BET_NONE)
        {
            Debug.LogError("something go wrong");
        }
    }
    [TestMethod]
    public void TestDistributeChipInMultiple(int betCount, int iterationCount)
    {
        for (int i = 0; i < iterationCount; i++)
        {
            TestDistributeChipInRandom(betCount);
        }
    }
    [TestMethod]
    public void TestPreviousBettingValue(long previousBetMoney)
    {
        var temp = GetChipsFromMoney(previousBetMoney);
        var temp2 = 0;
    }

    #endregion

    #region common methods
    public Vector3 GetTablePositionFromBetPlace(eADBetPlace betPlace)
    {
        var bettingPosObject = ResourceContainer.Get<ADBettingBoardTag>((betPlace.ToString() + "_" + ((int)betPlace).ToString()));
        return bettingPosObject.transform.position;
    }
    public long GetMoneyFromChip(eAD_BUTTONLIST chip)
    {
        var moneyFromChip = ResourceContainer.Get<ADAnteDependSetting>().chipValueInThisRoom[chip];
        return moneyFromChip;
    }

    public Dictionary<eAD_BUTTONLIST,int> GetChipsFromMoney(long money)
    {
        var tempMoney = money;
        Dictionary<eAD_BUTTONLIST, int> tempDic = new Dictionary<eAD_BUTTONLIST, int>(capacity: 4);
        var chipValueList = ResourceContainer.Get<ADAnteDependSetting>().chipValueInThisRoom;
        
        for (int i = (int)eAD_BUTTONLIST._BTN_BETTING_4; i >= (int)eAD_BUTTONLIST._BTN_BETTING_1 ; --i)
        {
            if(chipValueList.ContainsKey( (eAD_BUTTONLIST)(i) ) == false )
            {
                // int temp = ((int)i - (int)eAD_BUTTONLIST._BTN_NONE);
                Debug.LogError("something is wrong");
            }
            long chipValue = chipValueList[(eAD_BUTTONLIST) (i) ];
            if (tempMoney >= chipValue)
            {
                tempDic.Add((eAD_BUTTONLIST)i, (int)(tempMoney / chipValue));
                tempMoney %= chipValue;
            }
            else
            {
                tempDic.Add((eAD_BUTTONLIST)i, 0);
            }
        }
        return tempDic;
        // throw new NotImplementedException();
    }

    public bool CheckIsEmpty(Dictionary<eADBetPlace, Dictionary<int, long>> needToCheckDic, eADBetPlace betPlace)
    {
        bool isEmpty = true;
        if(needToCheckDic.ContainsKey(betPlace) == false)
        {
            Debug.LogError("there is no winning bet place");
            return isEmpty;
        }
        var tempDic = needToCheckDic[betPlace];
        for (int i = 0; i < tempDic.Count; i++)
        {
            if(tempDic.ContainsKey(i) == false)
            {
                Debug.LogError("there is no user... something goes wrong");
                return isEmpty;
            }
            if (tempDic[i] != 0 )
            {
                isEmpty = false;
                return isEmpty;
            }
            //for (int j = 0; j < 1; j++)
            //{

            //}
        }
        return isEmpty;
    }
    public bool CheckIsEmpty(Dictionary<int, long> needToCheckDic, int userIndex)
    {
        bool isEmpty = false;
        var tempDic = needToCheckDic[userIndex];
        if (tempDic == 0)
        {
            isEmpty = true;
            return isEmpty;
        }
        return isEmpty;
    }

    public ADBettingBoardTag GetBetBoard(eADBetPlace betPlace)
    {
        var tempBettingBoard = ResourceContainer.Get<ADBettingBoardTag>((betPlace.ToString() + "_" + ((int)betPlace).ToString()));
        return tempBettingBoard;
    }


    public void SetButtonHighlight(int highlightButtonIndex, bool bEnable)
    {

        chipButtonHighlights[highlightButtonIndex].SetActive(bEnable);
        
        chipButtonHighlights[highlightButtonIndex].transform.localScale = bEnable ? Vector3.one * 1.2f : Vector3.one;
        bettingChipButtons[highlightButtonIndex].transform.localScale = bEnable ? Vector3.one * 1.2f : Vector3.one;
    }

    [TestMethod]
    public bool CheckIsSingleChip(long money)
    {
        bool isSingleChip = false;
        var currentChipValues = ResourceContainer.Get<ADAnteDependSetting>().chipValueInThisRoom;
        foreach (var item in currentChipValues)
        {
            if(item.Value.Equals(-1))
            {
                continue;
            }
            if((money % item.Value).Equals(0) && (money / item.Value).Equals(1))
            {
                isSingleChip = true;
                return isSingleChip;
            }
        }
        return isSingleChip;
    }
    public void IncrementMyTotalBettingChip(eAD_BUTTONLIST buttonIndex)
    {
        Debug.Log("current my total betting money is " + myTotalBettingMoney);
        myTotalBettingMoney += GetMoneyFromChip(buttonIndex);

        CheckAndSetEnableBoards();
    }
    public void IncrementMyTotalBettingChip(eAD_BUTTONLIST buttonIndex, int chipCount)
    {
        Debug.Log("current my total betting money is " + myTotalBettingMoney
            + " btn index " + buttonIndex.ToString()
            + " chip count " + chipCount);
        myTotalBettingMoney += GetMoneyFromChip(buttonIndex) * chipCount;
        
        CheckAndSetEnableBoards();
    }
    public void IncrementMyTotalBettingChip(long betMoney)
    {
        myTotalBettingMoney += betMoney;
        CheckAndSetEnableBoards();
    }
    public void CheckAndSetEnableBoards()
    {
        Debug.Log("betting maxmoney statement can bet? " + (myTotalBettingMoney >= (long)cGlobalInfos.GetIntoRoomInfo_97().stMAX_BETMONEY)
            + " max money is " + (long)cGlobalInfos.GetIntoRoomInfo_97().stMAX_BETMONEY
            + " my bet money is " + myTotalBettingMoney);
        if(myTotalBettingMoney >= (long)cGlobalInfos.GetIntoRoomInfo_97().stMAX_BETMONEY)
        {
            SetEnableBettingBoards(false);
        }
    }

    public void AddDepthCount(eADBetPlace betPlace)
    {
        if(bettingChipDepths.ContainsKey(betPlace) == false )
        {
            bettingChipDepths.Add(betPlace, currentChipDepth);
        }
        else
        {
            bettingChipDepths[betPlace]++;
        }
    }
    public int GetDepthCount(eADBetPlace betPlace)
    {
        var temp = 0;
        if (bettingChipDepths.ContainsKey(betPlace) == false)
        {
            return currentChipDepth;
            // bettingChipDepths.Add(betPlace, currentChipDepth);
        }
        else
        {
            return bettingChipDepths[betPlace];
            
        }
        return temp;
    }

    public void ClearForNewGame()
    {
        myTotalBettingMoney = 0;
        //ClearMyTotalBetting();
        bDestroyingAllChipEntitiesWithAlpha = true;
        //ClearAllChips();
        
        DisableAllBettingBoardHighlight();
        DisableAllMyBettingMoneyLabel();

        // currentChipDepth = 31;
        bettingChipDepths.Clear();

        bBettingSpinePlayed = false;
        for (int i = 0; i < bBettingAsPreviousArray.Length; i++)
        {
            bBettingAsPreviousArray[i] = false;
        }
        
        
        
        ResourcePool.ClearAll<ADSpineMultiplierEffectItem>(ef =>
        {
            ef.tag.Equals("ADMultiplier");
            
            return ef;
        });

        ResourcePool.ClearAll<ADAllInSpineEffectItem>(ef =>
        {
            ef.tag.Equals("ADAllIn");
            
            return ef;
        });

        AllInActiveDic.Clear();

        // skipping routine handle boolean
        bIsBettingTime = false;
        bIsBettingRoutineOff = true;



    }
    public void ClearMyTotalBetting()
    {
        myTotalBettingMoney = 0;
    }
    /// <summary>
    /// Clear Chips With some time(destroyingTime in ADChipBettingManager.cs)
    /// </summary>
    public void ClearAllChips()
    {
        bDestroyingAllChipEntitiesWithAlpha = true;
    }
    public void DisableDestroyingChipSystem()
    {
        bDestroyingAllChipEntitiesWithAlpha = false;
    }


    #region dish rank related
    public void CheckCurrentRank()
    {
        // 1. 3 dice are same, maximum bet

        // 2. only maximum bet and no loss
    }
    #endregion

    #endregion

    #region betting spine related methods
    [TestMethod]
    public void PlayBettingOpening()
    {
        Sound.Instance.EffPlay("AD_TurnAlarm");
        bettingOpenEndSpine.AnimationState.SetEmptyAnimation(0, 0);
        // "start"
        // from dish shuffle

        bettingOpenEndSpine.AnimationState.SetAnimation(0, "start", false);
    }
    [TestMethod]
    public void PlayBettingEnding()
    {
        Sound.Instance.EffPlay("AD_TurnAlarm");
        bettingOpenEndSpine.AnimationState.SetEmptyAnimation(0, 0);
        // "end"
        // from 3 sec remaining...

        bettingOpenEndSpine.AnimationState.SetAnimation(0, "end", false);

    }

    #endregion

    #region playing sound related methods
    [TestMethod(false)]
    /// <summary>
    /// 
    /// </summary>
    /// <param name="chipsForMovingCount"></param>
    /// <param name="bIsMyBetting">ignore this parameter when first parameter more than or equal 2, currently not used, but it remains for later</param>
    public void PlayBettingSound(int chipsForMovingCount,bool bIsMyBetting)
    {
        // my betting, other betting
        // retreive golds from result lose
        // scatter golds and move to player from result win

        //        이동하는 칩의 개수에 따른 사운드 출력
        //e_chip1 - 1
        //e_chip2 - 2 ~5
        //e_chip3 - 6 ~40
        //e_chip4 - 41 ~80
        //e_chip5 - 81 ~

        #region obsolete
        // 1. check currently played
        //if(chipsForMovingCount == 1)
        //{
        //    bool bTempDidEmptySlotExsist = false;
        //    for (int i = 0; i < TestTimeStack.Count; i++)
        //    {
        //        if (TestTimeStack[i].t <= 0f)
        //        {
        //            Debug.Log("there is empty slot " + i);
        //            bTempDidEmptySlotExsist = true;
        //        }
        //        else if (TestTimeStack[i].t > 1f)
        //        {
        //            TestTimeStack[i].t = 0;
        //        }
        //    }
        //    // 2. if sound playing count reached the limit, do not play further
        //    if (bTempDidEmptySlotExsist == false)
        //    {
        //        List<AudioClipData> tempDatas = Sound.Instance.audioClipDatas[0].list;
        //        var temp = tempDatas.Find(x => x.id.Equals("e_chip1"));
        //        Debug.Log("duration " + temp.data.length);

        //        // 3. play it after sound deleted
        //        CoroutineChain.Start
        //            .Wait(temp.data.length)
        //            .Call(() => PlayBettingSound(chipsForMovingCount, bIsMyBetting));

        //        return;
        //    }
        //}
        #endregion


        // 4. sound related timecontainer handled  must be here, in if statement...
        //var tempAudioDatas = Sound.Instance.audioClipDatas[0].list;

        Debug.Log("sound skip count " + soundSkipCount
            + " sound total count " + totalSoundCount);
        if (bIsMyBetting == false && IsBettingSoundSkipping())
        {
            soundSkipCount++;
            Debug.Log("sound skip count " + soundSkipCount);
            return;
        }
        
        bool bSoundPlayed = false;
        if (chipsForMovingCount <= 1 && bIsMyBetting)
        {
            bSoundPlayed = Sound.Instance.EffPlayWithFlag("e_chip1");
            // SetSoundTimeContainer("e_chip1");
            // StartCoroutine(SetSoundTimeContainer("e_chip1"));
        }
        else if(chipsForMovingCount <= 1 && !bIsMyBetting)
        {
            // Sound.Instance.EffPlay("e_chip2");
            bSoundPlayed = Sound.Instance.EffPlayWithFlag("e_chip1");
            // SetSoundTimeContainer("e_chip1");
            // StartCoroutine(SetSoundTimeContainer("e_chip1"));
        }
        // 2~5
        if (2 <= chipsForMovingCount && chipsForMovingCount <= 5)
        {
            Sound.Instance.EffPlayWithForce("e_chip2", 0);
            bSoundPlayed = true;
            // SetSoundTimeContainer("e_chip2");
        }
        // 6~40
        else if (6 <= chipsForMovingCount && chipsForMovingCount <= 40)
        {
            Sound.Instance.EffPlayWithForce("e_chip3", 1);
            bSoundPlayed = true;
            // SetSoundTimeContainer("e_chip3");
        }
        else if(41 <= chipsForMovingCount && chipsForMovingCount <= 80)
        {
            Sound.Instance.EffPlayWithForce("e_chip4", 2);
            bSoundPlayed = true;
            // SetSoundTimeContainer("e_chip4");
        }
        // 81+
        else if (81 <= chipsForMovingCount)
        {
            Sound.Instance.EffPlayWithForce("e_chip5", 3);
            bSoundPlayed = true;
            // SetSoundTimeContainer("e_chip5");
        }
        if(bSoundPlayed)
        {
            totalSoundCount++;
        }


    }
    


    //[TestMethod]
    //public void TestPlayBettingSound(int iterationCount = 4, int chipsForMovingCount = 85, bool bIsMyBetting = false)
    //{
    //    for (int i = 0; i < iterationCount; i++)
    //    {
    //        var tempFloat = UnityEngine.Random.Range(0.05f, 0.15f);
    //        CoroutineChain.Start
    //            .Wait(tempFloat * i)
    //            .Call(() => PlayBettingSound(chipsForMovingCount, bIsMyBetting));
            
    //    }
    //}
    public IEnumerator SetSoundTimeContainer(string soundID)
    {
        yield return null;
        // return;
        // temp function off....
        var tempAudioDatas = Sound.Instance.audioClipDatas[0].list;
        var temp = tempAudioDatas.Find(x => x.id.Equals(soundID));
        Debug.Log("duration " + temp.data.length);

        // bool bTempDidEmptySlotExsist = false;
        for (int i = 0; i < TestTimeStack.Count; i++)
        {
            if (TestTimeStack[i].t <= 0f)
            {
                TestTimeStack[i].time = temp.data.length;
                this.Wait(TestTimeStack[i]);
                // bTempDidEmptySlotExsist = true;
            }
            
        }

    }
    #endregion

    #region Purformance Handle related methods
    public IEnumerator OthersBettingSkipRoutine()
    {
        
        float limitTime = 0f;
        bIsBettingRoutineOff = false;
        yield return null;
        while (bIsBettingTime)
        {
            // TimeContainer t = new TimeContainer("a", 1f);
            // t.t += Time.deltaTime / time;
            // if (t.t > 1f) t.t = 1f;

            
            limitTime += Time.deltaTime / bettingDecimateTime;
            if(limitTime > 1f)
            {
                Debug.Log("other betting, routine looping");
                limitTime = 0f;
                totalBettingCount = 0;
                bettingSkipCount = 0;

            }
            //if(otherBettingCount >= BETTING_DECIMATE_COUNT)
            //{

            //}
            yield return null;
        }
        bIsBettingRoutineOff = true;
        totalBettingCount = 0;
        bettingSkipCount = 0;
    }
    
    public bool IsBettingSkipping()
    {
        bool tempBool = (totalBettingCount >= bettingDemcimateCount ? true : false) && (bettingSkipCount <= bettingSkipLimitCount ? true : false);
        if(tempBool == true)
        {
            // bettingSkipModifer++;

            //if (bettingSkipLimitCount <= bettingMaximumLimitCount
            //    // && bettingSkipLimitCount > bettingMaximumLimitCount)
            //    )
            //{
            //    bettingSkipLimitCount++;
            //}

            //if(bettingSkipLimitCount >= bettingMaximumLimitCount)
            //{
            //    bettingDecimateTime += 0.02f;
            //    bettingMaximumLimitCount += 4;
            //}
        }
        return tempBool;
        
    }
    

    public IEnumerator OtherBettingSoundSkipRoutine()
    {
        float limitTime = 0f;
        bIsSoundRoutineOff = false;
        yield return null;
        totalSoundCount = 0;
        soundSkipCount = 0;
        while (bIsBettingTime)
        {
            // TimeContainer t = new TimeContainer("a", 1f);
            // t.t += Time.deltaTime / time;
            // if (t.t > 1f) t.t = 1f;


            limitTime += Time.deltaTime / soundDecimateTime;
            if (limitTime > 1f)
            {
                Debug.Log("other betting, routine looping");
                limitTime = 0f;
                totalSoundCount = 0;
                soundSkipCount = 0;

            }
            //if(otherBettingCount >= BETTING_DECIMATE_COUNT)
            //{

            //}
            yield return null;
        }
        //bIsBettingRoutineOff = true;
        //totalBettingCount = 0;
        //bettingSkipCount = 0;
        bIsSoundRoutineOff = true;
        totalSoundCount = 0;
        soundSkipCount = 0;

        yield return null;
    }
    public bool IsBettingSoundSkipping()
    {
        bool tempBool = (totalSoundCount >= soundDemcimateCount ? true : false) && (soundSkipCount <= soundSkipLimitCount ? true : false);
        // soundSkipCount++;
        return tempBool;
    }

    
    #endregion

}
public struct ADChipSpawnInitData
{
    public eAD_BUTTONLIST buttonSelected;
    public eADBetPlace betPlace;

    public eAD_BettingPlaceSize betPlaceSize;

    public float timeArrival;

    #region test purpose
    public int bettingCount;
    #endregion

    public ADChipSpawnInitData(eAD_BUTTONLIST buttonSelected, eADBetPlace betPlace, eAD_BettingPlaceSize betPlaceSize, float timeArrival, int bettingCount = 1)
    {
        this.buttonSelected = buttonSelected;
        this.betPlace = betPlace;
        this.betPlaceSize = betPlaceSize;
        this.timeArrival = timeArrival;
        this.bettingCount = bettingCount;
    }
}

[Serializable]
public class BetPlaceSizeDataStore
{
    public eADBetPlace betPlace;
    public eAD_BettingPlaceSize bettingPlaceSize;

}

public class BetSpawnInfo
{
    public eADBetPlace betPlace;
    public eAD_BUTTONLIST chipKind;

    public int bettingCount;
    // public int betUserIndex;
    public int winUserIndex;


}