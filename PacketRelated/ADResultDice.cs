using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Wooriline;

public class ADResultDice : PacketHandler
{
    public List<int> tempList;
    public override int GetNumber()
    {
        return (int)ANIMALDICE_PK.R_09_RESULT_DICE;
    }
    public override void Func()
    {
        var rec = new R_09_RESULT_DICE(SubGameSocket.m_bytebuffer);
        Debug.Log("[R_09_RESULT_DICE] aniTime : " + rec.nTIME 
            + " opentime  " + rec.nOPENTIME 
            + " viewtime  " + rec.nVIEWTIME 
            + " tabletime  " + rec.nTABLETIME 
            + ", dice 1: " + rec.nDICE1 
            + ", dice 2: " + rec.nDICE2 
            + ", dice 3: " + rec.nDICE3
            + " game index is " + (int)rec.stGAME_IDX);

        var tempList = new List<int> { rec.nDICE1, rec.nDICE2, rec.nDICE3 };
        // throw new System.NotImplementedException();
        var resultDice = new ADResultDiceAction(rec.nTIME / 1000f,
            rec.nOPENTIME / 1000f,
            rec.nVIEWTIME / 1000f,
            rec.nTABLETIME / 1000f,
            tempList,
            rec.lWINPART,
            (int)rec.stGAME_IDX
            );
        ActionPlayer.Play(resultDice);

    }
    [TestMethod]
    public void TestResultDice()
    {
        var tempList = new List<int> { 1, 2, 4 };
        var tempWinBetPlace = new List<st09_WINTABLE_INFO>(3);
        var tempItem1 = new st09_TABLE_PART();
        var tempItem2 = new st09_TABLE_PART();
        var tempItem3 = new st09_TABLE_PART();
        
        tempItem1.nTABLEPOS = (int)eADBetPlace._BET_ANIMAL_BAK;
        tempItem2.nTABLEPOS = (int)eADBetPlace._BET_ANIMAL_DAK;
        tempItem3.nTABLEPOS = (int)eADBetPlace._BET_ANIMAL_GE;

        // tempWinBetPlace.Add

        var resultDice = new ADResultDiceAction(5.3f,
            2.5f,
            1f,
            1.8f,
            tempList,
            tempWinBetPlace,
            0
            );
        ActionPlayer.Play(resultDice);
    }


    [TestMethod]
    public void TestMultiplierSpine(eADBetPlace betplace, float yPosModifier, bool bStaying)
    {
        var tempBettingmanager = ResourceContainer.Get<ADChipBettingManager>();
        var tempPos = tempBettingmanager.GetBetBoard(betplace).myBetTotalTextObject.transform.position;
        tempPos = new Vector3(tempPos.x, tempPos.y + yPosModifier, tempPos.z);
        var item = ResourcePool.Pop<ADSpineMultiplierEffectItem, EffectData>(new EffectData(
            asset: ResourceContainer.Get<ADResultPartInfoStoring>().multiplierSpine,
            animationName: "x3",
            scaleFactor: 6f,
            tag: "ADMultiplier"
            ));
        item.transform.position = tempPos;
        item.SetSortingLayer("WorldForward", 5050);
        //item.SetSortingLayer("WorldForward", 5056);
        if (bStaying == false)
        {

            item.Play();
        }
        else
        {
            item.PlaySpineStaying();
        }
    }
    [TestMethod]
    public void ClearMultiplierSpine()
    {
        ResourcePool.ClearAll<ADSpineMultiplierEffectItem>(ef =>
        {
            ef.tag.Equals("ADMultiplier");
            return ef;
        });
    }

    //public IEnumerator TestBettingBoardHighlightRoutine()
    //{
    //    List<eADBetPlace> _winTableParts = new List<eADBetPlace>();
    //    _winTableParts.Add(eADBetPlace._BET_ANIMAL_BAK);
    //    _winTableParts.Add(eADBetPlace._BET_ANIMAL_DAK);
    //    _winTableParts.Add(eADBetPlace._BET_ANIMAL_GE);

    //    // ontime, staytime,offtime

    //    TimeContainer.Stack _highLightOn = new TimeContainer.Stack(_winTableParts.Count * 3 + 3, 0.2f, "TABLE_HIGHLIGHT_ON");
    //    TimeContainer.Stack _highLightStay = new TimeContainer.Stack(2, 0.1f, "TABLE_HIGHLIGHT_STAY");
    //    TimeContainer.Stack _highLightOff = new TimeContainer.Stack(_winTableParts.Count * 2 + 2, 0.3f, "TABLE_HIGHLIGHT_OFF");

    //    // 4. loop through 2 times over 1~3 steps
    //    for (int i = 0; i < 3; i++)
    //    {
    //        // 1. turn on highlight


    //        for (int j = 0; j < _winTableParts.Count; j++)
    //        {
    //            var table = ResourceContainer.Get<ADChipBettingManager>().GetBetBoard((eADBetPlace)_winTableParts[j]);
    //            TimeContainer time = new TimeContainer("TestHighLightTime", 1.2f);
    //            // table.highlightBettingBoard.AlphaTween(1f, _highLightOn.Pop());
    //            table.highlightBettingBoard.AlphaTween(0.6f, _highLightOn.Pop()); // modified from 1f to 0.6f...
    //        }
    //        // var t1 = _highLightOn.Pop();
    //        yield return this.Wait(_highLightOn.Pop());


    //        if(i == 2)
    //        {
    //            // 5. on and stay until handled off received
    //            yield break;
    //        }

    //        // 2. hold highlight on
    //        yield return new WaitForSeconds(_highLightStay.Pop().time);

    //        // 3. turn off highlight
    //        for (int j = 0; j < _winTableParts.Count; j++)
    //        {
    //            var table = ResourceContainer.Get<ADChipBettingManager>().GetBetBoard((eADBetPlace)_winTableParts[j]);
    //            TimeContainer time = new TimeContainer("TestHighLightTime", 1.2f);
    //            table.highlightBettingBoard.AlphaTween(0f, _highLightOff.Pop());
    //        }
    //        // var t2 = _highLightOff.Pop();
    //        yield return this.Wait(_highLightOff.Pop());

    //    }


    //}

    #region delete it after test
    [TestMethod]
    public void SetDicePack(List<int> diceValues)
    {
        var tempSprites = new List<Sprite>(capacity: 3); // ResourceContainer.Get<ADDishDiceSymbol>("DiceSymbols").diceSymbols;
        var diceCount =  tempSprites.Capacity;//diceValues.Count; //ResourceContainer.Get<ADDishDiceSymbol>("DiceSymbols").diceCount;
        // diceValues = tempList;
        for (int i = 0; i < diceCount; i++)
        {
            tempSprites.Add(ResourceContainer.Get<ADDishDiceSymbol>("DiceSymbols").diceSymbols[tempList[i]]);
        }

        // 1. set which dice pack active 
        var tempDicePackIndex = Random.Range(1, 2);
        var dicePack = ResourceContainer.Get<GameObject>("DicePack" + tempDicePackIndex.ToString());
        dicePack.SetActive(true);
        ResourceContainer.Get<GameObject>("DicePack" + (tempDicePackIndex == 1 ? 2 : 1).ToString()).SetActive(false);

        // 2. change sprites of those dice
        var modifier = (tempDicePackIndex - 1) * diceCount;
        for (int i = 0; i < diceCount; ++i)
        {
            ResourceContainer.Get<Image>("DiceSymbol" + (modifier + i + 1).ToString()).sprite = tempSprites[i];
        }

        // 3. change rotation
        var tempRotValue = Random.Range(-20f, 20f);
        dicePack.transform.eulerAngles = new Vector3(0, 0, tempRotValue);
        // dicePack.transform.rotation = new Quaternion(0, 0, tempRotValue, 0);


    }

    [TestMethod(false)]

    public void TestResultBoardSpine(int betPlace, float scale)
    {

        // animal_1 ~ animal_6 -> 2.733s
        // win_board_1 ~ win_board_4 -> 4.867s

        List<int> betPlaces = new List<int>(capacity: 6);
        // Dictionary<int, int> betDic = new Dictionary<int, int>(capacity: 5);
        List<int> betEffectPlayed = new List<int>(capacity: 6);
        List<int> betAnimalEffectPlayed = new List<int>(capacity: 3);
        // betPlaces.Add(betPlace);
        betPlaces.Add(24);
        betPlaces.Add(26);
        betPlaces.Add(46);

        betPlaces.Add(10);
        betPlaces.Add(30);

        var tempResultPart = ResourceContainer.Get<ADResultPartInfoStoring>();
        foreach (var betItem in betPlaces)
        {
            var firstDigit = betItem % 10;
            var secondDigit = betItem / 10;
            var effectIndex = -1;
            if (firstDigit != 0) // double sided board betting
            {
                //if (betEffectPlayed.Contains(firstDigit) == false)
                //{
                //    betEffectPlayed.Add(firstDigit); // a. animal board
                //    effectIndex = firstDigit * 10;
                //    tempResultPart.PlayAnimalBoard(effectIndex);
                //}

                betEffectPlayed.Add(betItem); // b. gap or side board
                effectIndex = tempResultPart.CategorizeGapSideBoard(betItem);
                tempResultPart.PlayGapSideBoard(effectIndex, betItem);


                if (betEffectPlayed.Contains(firstDigit * 10) == false
                    && betAnimalEffectPlayed.Contains(firstDigit * 10) == false)
                {
                    // tempResultPart.PlayAnimalEdgeBoard(firstDigit * 10);
                    tempResultPart.PlayAnimalBoard(firstDigit * 10);
                    betAnimalEffectPlayed.Add(firstDigit * 10);
                }
                if (betEffectPlayed.Contains(secondDigit * 10) == false
                    && betAnimalEffectPlayed.Contains(secondDigit * 10) == false)
                {
                    // tempResultPart.PlayAnimalEdgeBoard(secondDigit * 10);
                    tempResultPart.PlayAnimalBoard(secondDigit * 10);
                    betAnimalEffectPlayed.Add(secondDigit * 10);
                }

            }
            else // one side(one animal image only) board betting
            {
                if (betEffectPlayed.Contains(secondDigit) == false)
                {
                    betEffectPlayed.Add(secondDigit); // a. animal board
                    effectIndex = secondDigit * 10;
                    tempResultPart.PlayAnimalEdgeBoard(effectIndex);

                    if (betAnimalEffectPlayed.Contains(secondDigit * 10) == false)
                    {
                        tempResultPart.PlayAnimalBoard(effectIndex);
                        betAnimalEffectPlayed.Add(secondDigit * 10);
                    }
                }
            }

        }


    }
    
    #endregion

}

public class ADResultDiceAction : IAction
{
    public string Log => "[R_09_ADResultDiceAction]";

    public string ID => "ADResultDiceAction";

    public List<string> CancelID => new List<string> { "ADResultDiceAction"};

    #region variables and constructor

    TimeContainer.Stack _backgroundFade;

    TimeContainer.Stack _dishAppear;
    TimeContainer.Stack _dishCoverOff;
    TimeContainer.Stack _diceHighlight;
    TimeContainer.Stack _dishDisappear;
    TimeContainer.Stack _historyIconDisappear;
    TimeContainer.Stack _historyIconAppear;
    TimeContainer.Stack _historyIconAppearAlpha;

    //TimeContainer.Stack _tableHighLight;
    TimeContainer.Stack _tableWaitForNextPacket;

    TimeContainer.Stack _highLightOn;
    TimeContainer.Stack _highLightStay;
    TimeContainer.Stack _highLightOff;

    TimeContainer.Stack _ResultDiceSpineFX;


    TimeContainer.StackSum _all;
    public float _aniTime;
    public List<int> _diceValues = new List<int>(capacity: 3);

    // List<st09_TABLE_PART> _winTableParts;
    List<st09_WINTABLE_INFO> _winTableParts;

    public int _currentGameIndex;

    #endregion

    public ADResultDiceAction(float aniTime, float openTime, float viewTime, float tableTime, List<int> diceValues, List<st09_WINTABLE_INFO> winTableParts, int currentGameIndex)
    {
        _aniTime = aniTime;
        _diceValues = diceValues;
        _winTableParts = winTableParts;
        // open time view time 
        _backgroundFade = new TimeContainer.Stack(4, viewTime / 8, "BACKGROUND_FADE"); // appear and disappear with dish,dice and background

        _dishAppear = new TimeContainer.Stack(2, openTime/      2, "DISH_APPEAR"); // scale and transform
        _dishCoverOff = new TimeContainer.Stack(2, openTime/    2, "DISH_COVER_OFF"); // alpha and transform
        
        _diceHighlight = new TimeContainer.Stack(1, viewTime /  3, "DICE_HIGHLIGHT"); 
        _dishDisappear = new TimeContainer.Stack(2, viewTime /  3, "DISH_DISAPPEAR"); // alpha and transform

        _historyIconDisappear = new TimeContainer.Stack(4, viewTime / 4, "HISTORY_ICON_DISAPPEAR"); // alpha and transform
        _historyIconAppear = new TimeContainer.Stack(6, viewTime / 4, "HISTORY_ICON_APPEAR"); // alpha and transform
        _historyIconAppearAlpha = new TimeContainer.Stack(3, viewTime / 8, "HISTORY_ICON_APPEAR_ALPHA"); // alpha and transform

        // _tableHighLight = new TimeContainer.Stack(_winTableParts.Count + 1 , tableTime * 3 / 4, "HISTORY_ICON_APPEAR_ALPHA"); // alpha and transform
        // _tableWaitForNextPacket = new TimeContainer.Stack(_winTableParts.Count + 1, tableTime / 4, "HISTORY_ICON_APPEAR_ALPHA"); // alpha and transform

        // opentime + viewtime = 2.5f(dice FX ) + 1f
        // table time = 1.4f

        _highLightOn    = new TimeContainer.Stack(_winTableParts.Count * 3 + 3, 0.2f, "TABLE_HIGHLIGHT_ON");
        _highLightStay  = new TimeContainer.Stack(2, 0.1f, "TABLE_HIGHLIGHT_STAY");
        _highLightOff   = new TimeContainer.Stack(_winTableParts.Count * 2 + 2, 0.3f, "TABLE_HIGHLIGHT_OFF");

        _ResultDiceSpineFX = new TimeContainer.Stack(2, openTime, "RESULT_DICE");

        // _ResultDiceSpineFX




        _all = new TimeContainer.StackSum(_backgroundFade, _dishAppear, _dishCoverOff, _diceHighlight, _dishDisappear,
        _historyIconDisappear, _historyIconAppear, _historyIconAppearAlpha,
        _highLightOn, _highLightStay, _highLightOff
        );

        _currentGameIndex = currentGameIndex;
    }

    public IEnumerator ActionRoutine()
    {
        yield return null;
        // 0. background music fading
        // TimeContainer musicHandleTime = new TimeContainer("MUSIC_HANDLE_TIME", 1f);
        // ResourceContainer.Get<ADGameMain>().HandleBackgroundMusic(0.1f, musicHandleTime);
        ResourceContainer.Get<ADGameMain>().HandleBackgroundMusicWithDelay(0.1f, 1f);


        // 0.background fading...
        var tempFade = ResourceContainer.Get<Image>("BackgroundFade");
        // float tempFadeTime = 0.3f;
        tempFade.gameObject.SetActive(true);
        yield return tempFade.AlphaTween(0.25f, _backgroundFade.Pop()); // viewtime/4 (0.25sec currently)


        //0.set winning bet place
        var tempWinBetPlaces = new List<int>(capacity: 5);
        for (int i = 0; i < _winTableParts.Count; i++)
        {
            tempWinBetPlaces.Add(_winTableParts[i].nTABLEPOS);
            Debug.Log("caught win bet place is " + _winTableParts[i].nTABLEPOS);
            Debug.Log("caught win bet place is " + ((eADBetPlace)_winTableParts[i].nTABLEPOS).ToString()
                + " caught multiplier is  " + (long)_winTableParts[i].nMULTI
                );
            // " caught win bet place my bet money is  " + (long)_winTableParts[i].stMYBETMONEY
            // " caught win bet place result money is  " + (long)_winTableParts[i].stRESULTMONEY);
            foreach (var user in _winTableParts[i].lUSER)
            {
                Debug.Log("user name is " + user.szID);
            }
        }

        ResourceContainer.Get<ADResultPartInfoStoring>().SetWinBetPlace(tempWinBetPlaces);


        SetDicePack(_diceValues);

        #region 

        // 1. dice spine FX
        // TimeContainer.ContainClear("RESULT_DICE");
        // TimeContainer diceRoutineTime = new TimeContainer("RESULT_DICE", ADDishDiceManager.openDishSpineModifier);
        var tempRank = CheckCurrentRank(_winTableParts);
        Debug.Log("dish rank is " + tempRank.ToString());

        ResourceContainer.Get<ADDishDiceManager>().StartResultDice(_ResultDiceSpineFX.Pop(), tempRank);
        // ResourceContainer.Get<ADDishDiceManager>().StartResultDice(_ResultDiceSpineFX.Pop(), false);

        yield return new WaitForSeconds(_ResultDiceSpineFX.Pop().time); // opentime + viewtime/4(2.75sec currently)

        // 2. remove fading, dish and dice

        // ResourceContainer.Get<ADDishDiceManager>().ClearDiceAnimation(0.4f);

        // 2.1 block NPC tip

        // ResourceContainer.Get<Image>("ADTipBlock").gameObject.SetActive(true);

        yield return tempFade.AlphaTween(0f, _backgroundFade.Pop()); // opentime + viewtime/2(3sec currently)
        
        tempFade.gameObject.SetActive(false);
        // ResourceContainer.Get<Image>("ADTipBlock").gameObject.SetActive(false);

        // 3. (history panel from right bottom) update 3 dice icon
        // 3.1. disappear icons with alpha
        Vector3 tempDisappearPos1 = ResourceContainer.Get<RectTransform>("PreviousDiceStayPos" + (1).ToString()).localPosition;
        tempDisappearPos1 = new Vector3(tempDisappearPos1.x, tempDisappearPos1.y + 40f, tempDisappearPos1.z);
        Vector3 tempDisappearPos2 = ResourceContainer.Get<RectTransform>("PreviousDiceStayPos" + (2).ToString()).localPosition;
        tempDisappearPos2 = new Vector3(tempDisappearPos2.x, tempDisappearPos2.y + 40f, tempDisappearPos2.z);
        Vector3 tempDisappearPos3 = ResourceContainer.Get<RectTransform>("PreviousDiceStayPos" + (3).ToString()).localPosition;
        tempDisappearPos3 = new Vector3(tempDisappearPos3.x, tempDisappearPos3.y + 40f, tempDisappearPos3.z);
        for (int i = 0; i < ResourceContainer.Get<ADHistoryManager>().previousHistoryDiceCount; i++)
        {
            ResourceContainer.Get<Image>("PreviousDice" + (i + 1).ToString()).AlphaTween(0f, _historyIconDisappear.Pop());
        }
        // yield return ResourceContainer.Get<ResourceTag>("DishParent").Wait(_historyIconDisappear.Pop());
        yield return ResourceContainer.Get<ADDishDiceManager>().Wait(_historyIconDisappear.Pop());

        // 3.2. appear icons
        // ResourceContainer.Get<>
        Vector3 tempStayPos1 = ResourceContainer.Get<RectTransform>("PreviousDiceStayPos" + (1).ToString()).localPosition;
        Vector3 tempStayPos2 = ResourceContainer.Get<RectTransform>("PreviousDiceStayPos" + (2).ToString()).localPosition;
        Vector3 tempStayPos3 = ResourceContainer.Get<RectTransform>("PreviousDiceStayPos" + (3).ToString()).localPosition;
        //for (int i = 0; i < ResourceContainer.Get<ADHistoryManager>().previousHistoryDiceCount; i++)
        //{
        //    // tempStayPos1 = ResourceContainer.Get<RectTransform>("PreviousDiceStayPos" + (i + 1).ToString()).position;
        //    ResourceContainer.Get<Image>("PreviousDice" + (i + 1).ToString()).SetAlpha(1f);
        //}

        ResourceContainer.Get<ADHistoryPopup>().ChangeCurrentDie(0, (eAD_DICE)_diceValues[0]);
        ResourceContainer.Get<Image>("PreviousDice" + (1).ToString()).AlphaTween(1f, _historyIconAppearAlpha.Pop());
        ResourceContainer.Get<Image>("PreviousDice" + (1).ToString()).MoveLocal(tempDisappearPos1, tempStayPos1, _historyIconAppear.Pop());
        TimeContainer t1 = _historyIconAppear.Pop();
        t1.time /= 3;
        yield return ResourceContainer.Get<Image>("PreviousDice" + (1).ToString()).Wait(t1); 

        ResourceContainer.Get<ADHistoryPopup>().ChangeCurrentDie(1, (eAD_DICE)_diceValues[1]);
        ResourceContainer.Get<Image>("PreviousDice" + (2).ToString()).AlphaTween(1f, _historyIconAppearAlpha.Pop());
        ResourceContainer.Get<Image>("PreviousDice" + (2).ToString()).MoveLocal(tempDisappearPos2, tempStayPos2, _historyIconAppear.Pop());
        TimeContainer t2 = _historyIconAppear.Pop();
        t2.time /= 3;
        yield return ResourceContainer.Get<Image>("PreviousDice" + (2).ToString()).Wait(t2); 

        ResourceContainer.Get<ADHistoryPopup>().ChangeCurrentDie(2, (eAD_DICE)_diceValues[2]);
        ResourceContainer.Get<Image>("PreviousDice" + (3).ToString()).AlphaTween(1f, _historyIconAppearAlpha.Pop());
        ResourceContainer.Get<Image>("PreviousDice" + (3).ToString()).MoveLocal(tempDisappearPos3, tempStayPos3, _historyIconAppear.Pop());
        TimeContainer t3 = _historyIconAppear.Pop();
        t3.time /= 3;
        yield return ResourceContainer.Get<Image>("PreviousDice" + (3).ToString()).Wait(t3); // opentime + viewtime/4 + viewtime  (3.75 sec currently)

        // 4. update history panel

        // ResourceContainer.Get<ADHistoryPopup>().AddHistory((eAD_DICE)_diceValues[0], (eAD_DICE)_diceValues[1], (eAD_DICE)_diceValues[2]);

        ResourceContainer.Get<ADHistoryPopup>().SetCurrentDiceValues((eAD_DICE)_diceValues[0], (eAD_DICE)_diceValues[1], (eAD_DICE)_diceValues[2]);

        var history = ResourceContainer.Get<ADHistoryPopup>();
        history.SetCurrentGameIndex(_currentGameIndex);
        Debug.Log("setting up history index : " + _currentGameIndex);
        history.AddHistory(_currentGameIndex, history.currentDiceValues[0], history.currentDiceValues[1], history.currentDiceValues[2], 0);
        #endregion

        // 5. play multiplier spine on betting board
        var tempBettingmanager = ResourceContainer.Get<ADChipBettingManager>();
        var mySerial = cGlobalInfos.GetIntoRoomInfo_97().nSERIAL;
        var tempBetList = new List<int>(capacity: 6);

        bool bMyBet = ResourceContainer.Get<ADChipBettingManager>().myBettingMoney.Count > 0;

        bool bMissMulti = false;

        foreach (var table in _winTableParts)
        {
            var temp = table.lUSER.Find(x => x.nSERIAL == mySerial);
            if (temp != null)
            {
                tempBetList.Add(table.nTABLEPOS);
            }
            if (table.nMULTI <= 1) // no multiplier
            {
                continue;
            }
            
            if (table.lUSER.Count != 0 || (temp == null && bMyBet))
            {
                // condition changed
                // if any user win with multiplier...
                var animString = "x" + table.nMULTI.ToString();
                if (temp == null && bMyBet)
                {
                    bMissMulti = true;
                    animString += "_lose";
                }

                var tempBetPlace = tempBettingmanager.GetBetBoard((eADBetPlace)table.nTABLEPOS);
                var tempPos = tempBetPlace.myBetTotalTextObject.transform.position;

                tempPos.y += 5f;

                PlayMultiplierSpine(animString, tempPos);
            }
            // 7. result board spine(only my player win that board)
        }

        if (bMissMulti)
            // Sound.Instance.EffPlay("AD_MissMulti");
            Sound.Instance.EffPlayWithForce("AD_MissMulti");

        PlayResultBoardSpine(tempBetList);

        // 6. at the same time highlight win betting boards
        #region 5. highlight win betting boards
        // 4. loop through 2 times over 1~3 steps // 0.6f * 2 + 0.2f = 1.4f 
        for (int i = 0; i < 3; i++)
        {
            // error handle... when connect middle of on going game
            //
            if(eAD_STEP._IS_AD_OPEN_DICE > ADGameStepHandle.STEP 
                || ADGameStepHandle.STEP > eAD_STEP._IS_AD_RESULT_END)
            {
                ResourceContainer.Get<ADChipBettingManager>().DisableAllBettingBoardHighlight();
                yield break;
                // skip entire highlight sequence...
            }
                // open dice ~ result_end...

            // 1. turn on highlight


            for (int j = 0; j < tempWinBetPlaces.Count; j++)
            {
                var table = ResourceContainer.Get<ADChipBettingManager>().GetBetBoard((eADBetPlace)tempWinBetPlaces[j]);

                table.highlightBettingBoard.AlphaTween(0.48f, _highLightOn.Pop());
            }
            // var t1 = _highLightOn.Pop();
            yield return ResourceContainer.Get<ADChipBettingManager>().Wait(_highLightOn.Pop());


            if (i == 2)
            {
                // 5. on and stay until handled off received
                continue;
            }

            // 2. hold highlight on
            yield return new WaitForSeconds(_highLightStay.Pop().time);

            // 3. turn off highlight
            for (int j = 0; j < tempWinBetPlaces.Count; j++)
            {
                var table = ResourceContainer.Get<ADChipBettingManager>().GetBetBoard((eADBetPlace)tempWinBetPlaces[j]);

                table.highlightBettingBoard.AlphaTween(0f, _highLightOff.Pop());
            }
            // var t2 = _highLightOff.Pop();
            yield return ResourceContainer.Get<ADChipBettingManager>().Wait(_highLightOff.Pop());

        }
        #endregion

        // 7. play result spine on board

        




    }


    

    public void PlayResultBoardSpine(List<int> betPlaces)
    {

        // animal_1 ~ animal_6 -> 2.733s
        // win_board_1 ~ win_board_4 -> 4.867s

        // List<int> betPlaces = new List<int>(capacity: 6);
        // Dictionary<int, int> betDic = new Dictionary<int, int>(capacity: 5);
        List<int> betEffectPlayed = new List<int>(capacity: 6);
        List<int> betAnimalEffectPlayed = new List<int>(capacity: 3);
        // betPlaces.Add(betPlace);
        var tempResultPart = ResourceContainer.Get<ADResultPartInfoStoring>();
        foreach (var betItem in betPlaces)
        {
            var firstDigit = betItem % 10;
            var secondDigit = betItem / 10;
            var effectIndex = -1;
            if (firstDigit != 0) // double sided board betting
            {
                //if (betEffectPlayed.Contains(firstDigit) == false)
                //{
                //    betEffectPlayed.Add(firstDigit); // a. animal board
                //    effectIndex = firstDigit * 10;
                //    tempResultPart.PlayAnimalBoard(effectIndex);
                //}

                betEffectPlayed.Add(betItem); // b. gap or side board
                effectIndex = tempResultPart.CategorizeGapSideBoard(betItem);
                tempResultPart.PlayGapSideBoard(effectIndex, betItem);


                if (betEffectPlayed.Contains(firstDigit * 10) == false
                    && betAnimalEffectPlayed.Contains(firstDigit * 10) == false)
                {
                    // tempResultPart.PlayAnimalEdgeBoard(firstDigit * 10);
                    tempResultPart.PlayAnimalBoard(firstDigit * 10);
                    betAnimalEffectPlayed.Add(firstDigit * 10);
                }
                if (betEffectPlayed.Contains(secondDigit * 10) == false
                    && betAnimalEffectPlayed.Contains(secondDigit * 10) == false)
                {
                    // tempResultPart.PlayAnimalEdgeBoard(secondDigit * 10);
                    tempResultPart.PlayAnimalBoard(secondDigit * 10);
                    betAnimalEffectPlayed.Add(secondDigit * 10);
                }

            }
            else // one side(one animal image only) board betting
            {
                if (betEffectPlayed.Contains(secondDigit) == false)
                {
                    betEffectPlayed.Add(secondDigit); // a. animal board
                    effectIndex = secondDigit * 10;
                    tempResultPart.PlayAnimalEdgeBoard(effectIndex);

                    if (betAnimalEffectPlayed.Contains(secondDigit * 10) == false)
                    {
                        tempResultPart.PlayAnimalBoard(effectIndex);
                        betAnimalEffectPlayed.Add(secondDigit * 10);
                    }
                }
            }

        }


    }

    private void PlayMultiplierSpine(string animationName, Vector3 tempPos)
    {
        var tempData = new EffectData(
                asset: ResourceContainer.Get<ADResultPartInfoStoring>().multiplierSpine,
                animationName: animationName,
                scaleFactor: 6f,
                tag: "ADMultiplier"
                );
        tempData.hideType = ESpineHide.Alpha;
            var item = ResourcePool.Pop<ADSpineMultiplierEffectItem, EffectData>(tempData);
            //item.SetSortingLayer("WorldForward", 5056);
            item.SetSortingLayer("WorldForward", 5050);
            item.transform.position = tempPos;
            item.PlaySpineStaying();
        if (ResourceContainer.Get<ADResultPartInfoStoring>().bHasMultiplierPlayed == false && !animationName.Contains("lose"))
        {
            // Sound.Instance.EffPlay("AD_BettingBoardMultiplier");
            Sound.Instance.EffPlayWithForce("AD_BettingBoardMultiplier");
            ResourceContainer.Get<ADResultPartInfoStoring>().bHasMultiplierPlayed = true;
        }
    }

    private ADDishDiceManager.eADDishRank CheckCurrentRank(List<st09_WINTABLE_INFO> tableParts)
    {
        // List<st09_WINTABLE_INFO> _winTableParts;
        var manager = ResourceContainer.Get<ADChipBettingManager>();
        #region obsolete
        //if (manager.myTotalBettingMoney == (long)cGlobalInfos.GetIntoRoomInfo_97().stMAX_BETMONEY)
        //{
        //    var bHasSameDice = false;
        //    // 1. 3 dice are same, maximum bet -> X
        //    #region obsolete
        //    //if (tableParts.Count == 1)
        //    //{
        //    //    bHasSameDice = (tableParts[0].lUSER.Find(x => x.nSERIAL == cGlobalInfos.GetIntoRoomInfo_97().nSERIAL) != null) ? true : false;
        //    //    Debug.Log("dish rank 3 dice are same " + bHasSameDice);
        //    //    foreach (var bettingPlace in manager.myBettingMoney)
        //    //    {
        //    //        if (bettingPlace.Value > 0)
        //    //        {
        //    //            if (bettingPlace.Key != (eADBetPlace)tableParts[0].nTABLEPOS)
        //    //            {
        //    //                return ADDishDiceManager.eADDishRank.NOTHING;
        //    //            }
        //    //        }
        //    //    }



        //    //    return ADDishDiceManager.eADDishRank.DICE_SAME;
        //    //}
        //    //// 2. only maximum bet and no losing gold -> X

        //    //else
        //    //{
        //    //    int winTableCount = tableParts.Count;
        //    //    Debug.Log("table part count " + winTableCount
        //    //        + " my betting place count " + manager.myBettingMoney.Count);

        //    //    foreach (var bettingPlace in manager.myBettingMoney)
        //    //    {
        //    //        if (bettingPlace.Value > 0)
        //    //        {
        //    //            int tempCount = 0;
        //    //            int wrongCount = 0;
        //    //            foreach (var table in tableParts)
        //    //            {
        //    //                if (bettingPlace.Key != (eADBetPlace)table.nTABLEPOS)
        //    //                {
        //    //                    tempCount++;
        //    //                }
        //    //            }
        //    //            if (tempCount == tableParts.Count)//(tableParts.Count * manager.myBettingMoney.Count) )
        //    //            {
        //    //                return ADDishDiceManager.eADDishRank.NOTHING;
        //    //            }


        //    //            //if(wrongCount > 0)
        //    //            //{
        //    //            //    return ADDishDiceManager.eADDishRank.NOTHING;
        //    //            //}

        //    //        }
        //    //        if(manager.myBettingMoney.Count != tableParts.Count)
        //    //        {
        //    //            // if any losing gold exists.
        //    //            // return ADDishDiceManager.eADDishRank.NOTHING;
        //    //        }
        //    //        if (winTableCount == 0)
        //    //        {
        //    //            Debug.Log("dish rank no losing golds");
        //    //        }
        //    //    }
        //    //    return ADDishDiceManager.eADDishRank.NO_LOST_GOLD;

        //    //}
        //    #endregion
            
        //    long myTotalResultMoney = 0;
        //    foreach (var table in tableParts)
        //    {
        //        foreach (var bettingPlace in manager.myBettingMoney)
        //        {
        //            if (bettingPlace.Key == (eADBetPlace)table.nTABLEPOS)
        //            {
        //                if(table.nMULTI > 1)
        //                {
        //                    myTotalResultMoney += bettingPlace.Value * table.nMULTI;
        //                }
        //                else if(table.nMULTI == 1)
        //                {
        //                    myTotalResultMoney += bettingPlace.Value;
        //                }
        //            }
        //        }
                
        //    }
        //    Debug.Log("result_dice  my total result money is " + myTotalResultMoney);
        //    // 1. maximum bet's 5x times
        //    if ( ((long)cGlobalInfos.GetIntoRoomInfo_97().stMAX_BETMONEY * 5 ) <= myTotalResultMoney )
        //    {
        //        return ADDishDiceManager.eADDishRank.FIVE_TIMES;
        //    }
        //    // 2. maximum bet's 3x times
        //    else if (((long)cGlobalInfos.GetIntoRoomInfo_97().stMAX_BETMONEY * 3) <= myTotalResultMoney)
        //    {
        //        return ADDishDiceManager.eADDishRank.THREE_TIMES;
        //    }
            

        //}
        #endregion
        long myTotalResultMoney = 0;
        foreach (var table in tableParts)
        {
            foreach (var bettingPlace in manager.myBettingMoney)
            {
                if (bettingPlace.Key == (eADBetPlace)table.nTABLEPOS)
                {
                    if (table.nMULTI > 1)
                    {
                        myTotalResultMoney += bettingPlace.Value * table.nMULTI;
                    }
                    else if (table.nMULTI == 1)
                    {
                        myTotalResultMoney += bettingPlace.Value;
                    }
                }
            }

        }
        Debug.Log("result_dice  my total result money is " + myTotalResultMoney);
        // 1. maximum bet's 5x times
        if (((long)cGlobalInfos.GetIntoRoomInfo_97().stMAX_BETMONEY * 5) <= myTotalResultMoney)
        {
            return ADDishDiceManager.eADDishRank.FIVE_TIMES;
        }
        // 2. maximum bet's 3x times
        else if (((long)cGlobalInfos.GetIntoRoomInfo_97().stMAX_BETMONEY * 3) <= myTotalResultMoney
            && myTotalResultMoney < ((long)cGlobalInfos.GetIntoRoomInfo_97().stMAX_BETMONEY * 5))
        {
            return ADDishDiceManager.eADDishRank.THREE_TIMES;
        }
        else if (((long)cGlobalInfos.GetIntoRoomInfo_97().stMAX_BETMONEY) <= myTotalResultMoney 
            && myTotalResultMoney < ((long)cGlobalInfos.GetIntoRoomInfo_97().stMAX_BETMONEY * 3))
        {
            return ADDishDiceManager.eADDishRank.ONE_TIME;
        }
        else
        {

            return ADDishDiceManager.eADDishRank.NOTHING;
        }



    }

    #region obsolete
    //public IEnumerator ActionRoutine()
    //{
    //    // yield break;
    //    // 0. set winning bet place
    //    var tempWinBetPlaces = new List<int>(capacity: 5);
    //    for (int i = 0; i < _winTableParts.Count; i++)
    //    {
    //        tempWinBetPlaces.Add(_winTableParts[i].nTABLEPOS);
    //        Debug.Log("win bet place is " + _winTableParts[i].nTABLEPOS);
    //        Debug.Log("win bet place is " + ((eADBetPlace)_winTableParts[i].nTABLEPOS).ToString());
    //    }

    //    ResourceContainer.Get<ADResultPartInfoStoring>().SetWinBetPlace(tempWinBetPlaces);

    //    // 1. appear from top...with background fading...
    //    var tempFade = ResourceContainer.Get<Image>("BackgroundFade");
    //    // float tempFadeTime = 0.3f;
    //    tempFade.gameObject.SetActive(true);
    //    yield return tempFade.AlphaTween(0.25f, _backgroundFade.Pop());

    //    //
    //    #region dice FX
    //    ResourceContainer.Get<ResourceTag>("DishParent").GetComponent<Image>().SetAlpha(1f, true);

    //    SetDicePack(_diceValues);


    //    ResourceContainer.Get<ResourceTag>("DishCoverTag").transform.localPosition = new Vector3(0, 0, 0);
    //    ResourceContainer.Get<ResourceTag>("DishParent").transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    //    var appearPos = ResourceContainer.Get<GameObject>("DishAppearPosition").transform.position;
    //    var stayPos = ResourceContainer.Get<GameObject>("DishStayPosition").transform.position;



    //    ResourceContainer.Get<ResourceTag>("DishParent").Scale(new Vector3(1f, 1f, 1f), _dishAppear.Pop());
    //    yield return ResourceContainer.Get<ResourceTag>("DishParent").Move(appearPos, stayPos, _dishAppear.Pop());

    //    // 2. cover off

    //    ResourceContainer.Get<Image>("DishCoverImage").SetAlpha(1f);
    //    ResourceContainer.Get<ResourceTag>("DishCoverTag").transform.localPosition = new Vector3(0, 0, 0);
    //    var coverOffPos = ResourceContainer.Get<ResourceTag>("DishCoverOffPosition").transform.localPosition;

    //    ResourceContainer.Get<Image>("DishCoverImage").AlphaTween(0, _dishCoverOff.Pop());
    //    yield return ResourceContainer.Get<ResourceTag>("DishCoverTag").MoveLocal(coverOffPos, _dishCoverOff.Pop());


    //    // 3. dice highlighting
    //    var tempDicePack = ResourceContainer.Get<GameObject>("DicePack1").activeSelf ? ResourceContainer.Get<GameObject>("DicePack1") : ResourceContainer.Get<GameObject>("DicePack2");
    //    yield return tempDicePack.GetComponent<Image>().AlphaTween(0.9f, _diceHighlight.Pop());// modify when dice highlight image comes

    //    // 4. dish and dice disappear

    //    yield return ResourceContainer.Get<ResourceTag>("DishParent").GetComponent<Image>().AlphaTween(0f, _dishDisappear.Pop(), true);

    //    // 5. (history panel from right bottom) update 3 dice icon
    //    // 5.1. disappear icons with alpha
    //    Vector3 tempDisappearPos1 = ResourceContainer.Get<RectTransform>("PreviousDiceStayPos" + (1).ToString()).localPosition;
    //    tempDisappearPos1 = new Vector3(tempDisappearPos1.x, tempDisappearPos1.y + 40f, tempDisappearPos1.z);
    //    Vector3 tempDisappearPos2 = ResourceContainer.Get<RectTransform>("PreviousDiceStayPos" + (2).ToString()).localPosition;
    //    tempDisappearPos2 = new Vector3(tempDisappearPos2.x, tempDisappearPos2.y + 40f, tempDisappearPos2.z);
    //    Vector3 tempDisappearPos3 = ResourceContainer.Get<RectTransform>("PreviousDiceStayPos" + (3).ToString()).localPosition;
    //    tempDisappearPos3 = new Vector3(tempDisappearPos3.x, tempDisappearPos3.y + 40f, tempDisappearPos3.z);
    //    for (int i = 0; i < ResourceContainer.Get<ADHistoryManager>().previousHistoryDiceCount; i++)
    //    {
    //        ResourceContainer.Get<Image>("PreviousDice" + (i + 1).ToString()).AlphaTween(0f, _historyIconDisappear.Pop());
    //    }
    //    yield return ResourceContainer.Get<ResourceTag>("DishParent").Wait(_historyIconDisappear.Pop());

    //    // 5.2. appear icons
    //    // ResourceContainer.Get<>
    //    Vector3 tempStayPos1 = ResourceContainer.Get<RectTransform>("PreviousDiceStayPos" + (1).ToString()).localPosition;
    //    Vector3 tempStayPos2 = ResourceContainer.Get<RectTransform>("PreviousDiceStayPos" + (2).ToString()).localPosition;
    //    Vector3 tempStayPos3 = ResourceContainer.Get<RectTransform>("PreviousDiceStayPos" + (3).ToString()).localPosition;
    //    //for (int i = 0; i < ResourceContainer.Get<ADHistoryManager>().previousHistoryDiceCount; i++)
    //    //{
    //    //    // tempStayPos1 = ResourceContainer.Get<RectTransform>("PreviousDiceStayPos" + (i + 1).ToString()).position;
    //    //    ResourceContainer.Get<Image>("PreviousDice" + (i + 1).ToString()).SetAlpha(1f);
    //    //}

    //    ResourceContainer.Get<ADHistoryPopup>().ChangeCurrentDie(0, (eAD_DICE)_diceValues[0]);
    //    ResourceContainer.Get<Image>("PreviousDice" + (1).ToString()).AlphaTween(1f, _historyIconAppearAlpha.Pop());
    //    ResourceContainer.Get<Image>("PreviousDice" + (1).ToString()).MoveLocal(tempDisappearPos1, tempStayPos1, _historyIconAppear.Pop());
    //    TimeContainer t1 = _historyIconAppear.Pop();
    //    t1.time /= 3;
    //    yield return ResourceContainer.Get<Image>("PreviousDice" + (1).ToString()).Wait(t1);

    //    ResourceContainer.Get<ADHistoryPopup>().ChangeCurrentDie(1, (eAD_DICE)_diceValues[1]);
    //    ResourceContainer.Get<Image>("PreviousDice" + (2).ToString()).AlphaTween(1f, _historyIconAppearAlpha.Pop());
    //    ResourceContainer.Get<Image>("PreviousDice" + (2).ToString()).MoveLocal(tempDisappearPos2, tempStayPos2, _historyIconAppear.Pop());
    //    TimeContainer t2 = _historyIconAppear.Pop();
    //    t2.time /= 3;
    //    yield return ResourceContainer.Get<Image>("PreviousDice" + (2).ToString()).Wait(t2);

    //    ResourceContainer.Get<ADHistoryPopup>().ChangeCurrentDie(2, (eAD_DICE)_diceValues[2]);
    //    ResourceContainer.Get<Image>("PreviousDice" + (3).ToString()).AlphaTween(1f, _historyIconAppearAlpha.Pop());
    //    ResourceContainer.Get<Image>("PreviousDice" + (3).ToString()).MoveLocal(tempDisappearPos3, tempStayPos3, _historyIconAppear.Pop());
    //    TimeContainer t3 = _historyIconAppear.Pop();
    //    t3.time /= 3;
    //    yield return ResourceContainer.Get<Image>("PreviousDice" + (3).ToString()).Wait(t3);

    //    #endregion

    //    // 5.3 remove fading

    //    yield return tempFade.AlphaTween(0f, _backgroundFade.Pop());
    //    tempFade.gameObject.SetActive(false);


    //    // 6. update history panel

    //    // ResourceContainer.Get<ADHistoryPopup>().AddHistory((eAD_DICE)_diceValues[0], (eAD_DICE)_diceValues[1], (eAD_DICE)_diceValues[2]);

    //    ResourceContainer.Get<ADHistoryPopup>().SetCurrentDiceValues((eAD_DICE)_diceValues[0], (eAD_DICE)_diceValues[1], (eAD_DICE)_diceValues[2]);

    //    var history = ResourceContainer.Get<ADHistoryPopup>();
    //    history.SetCurrentGameIndex(_currentGameIndex);
    //    history.AddHistory(_currentGameIndex, history.currentDiceValues[0], history.currentDiceValues[1], history.currentDiceValues[2], 0);

    //    // 7. highlight win betting boards
    //    #region 7. highlight win betting boards
    //    // 4. loop through 2 times over 1~3 steps
    //    for (int i = 0; i < 3; i++)
    //    {
    //        // 1. turn on highlight


    //        for (int j = 0; j < tempWinBetPlaces.Count; j++)
    //        {
    //            var table = ResourceContainer.Get<ADChipBettingManager>().GetBetBoard((eADBetPlace)tempWinBetPlaces[j]);

    //            table.highlightBettingBoard.AlphaTween(1f, _highLightOn.Pop());
    //        }
    //        // var t1 = _highLightOn.Pop();
    //        yield return ResourceContainer.Get<ADChipBettingManager>().Wait(_highLightOn.Pop());


    //        if (i == 2)
    //        {
    //            // 5. on and stay until handled off received
    //            yield break;
    //        }

    //        // 2. hold highlight on
    //        yield return new WaitForSeconds(_highLightStay.Pop().time);

    //        // 3. turn off highlight
    //        for (int j = 0; j < tempWinBetPlaces.Count; j++)
    //        {
    //            var table = ResourceContainer.Get<ADChipBettingManager>().GetBetBoard((eADBetPlace)tempWinBetPlaces[j]);

    //            table.highlightBettingBoard.AlphaTween(0f, _highLightOff.Pop());
    //        }
    //        // var t2 = _highLightOff.Pop();
    //        yield return ResourceContainer.Get<ADChipBettingManager>().Wait(_highLightOff.Pop());

    //    }
    //    #endregion

    //}

    #endregion




    [TestMethod]
    public void SetDicePack(List<int> diceValues)
    {
        var tempSprites = new List<Sprite>(3);// ResourceContainer.Get<ADDishDiceSymbol>("DiceSymbols").diceSymbols;
        var diceCount = diceValues.Count; //ResourceContainer.Get<ADDishDiceSymbol>("DiceSymbols").diceCount;
        for (int i = 0; i < diceCount; i++)
        {
            tempSprites.Add(ResourceContainer.Get<ADDishDiceSymbol>("DiceSymbols").diceSymbols[diceValues[i] - 1]);
            // tempSprites[i] = tempSprites[ (diceValues[i]-1) ];
        }

        // 1. set which dice pack active 
        var tempDicePackIndex = Random.Range(1, 2);
        var dicePack = ResourceContainer.Get<GameObject>("DicePack" + tempDicePackIndex.ToString());
        dicePack.SetActive(true);
        
        ResourceContainer.Get<ADDishDiceManager>().dicepack = dicePack;
        ResourceContainer.Get<ADDishDiceManager>().diceImage = dicePack.GetComponent<Image>();

        ResourceContainer.Get<GameObject>("DicePack" + (tempDicePackIndex == 1 ? 2 : 1).ToString()).SetActive(false);

        // 2. change sprites of those dice
        var modifier = (tempDicePackIndex - 1) * diceCount;
        for (int i = 0; i < diceCount; ++i)
        {
            ResourceContainer.Get<Image>("DiceSymbol" + (modifier + i + 1).ToString()).sprite = tempSprites[i];
        }

        // 3. change rotation
        var tempRotValue = Random.Range(-20f, 20f);
        dicePack.transform.eulerAngles = new Vector3(0, 0, tempRotValue);
        // dicePack.transform.rotation = new Quaternion(0, 0, tempRotValue, 0);


    }


    public void Init()
    {
        ResourceContainer.Get<ResourceTag>("DishParent").GetComponent<Image>().SetAlpha(1f, true);
        
        for (int i = 0; i < ResourceContainer.Get<ADHistoryManager>().previousHistoryDiceCount; i++)
        {
            ResourceContainer.Get<Image>("PreviousDice" + (i + 1).ToString()).AlphaTween(0f, _historyIconDisappear.Pop());
        }
    }

    public IEnumerable<TimeContainer> GetAllTimeContainerList()
    {
        return _all;
    }


}