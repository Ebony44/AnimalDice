using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Wooriline;




public class ADBetting : PacketHandler
{

    ADBettingListMethod bettingListMethod;
    // ADBettingListAction bettingListAction;

    public override int GetNumber()
    {
        return (int)ANIMALDICE_PK.R_09_BET;
    }
    public override void Func()
    {
        if (GameUtils.st_maxPlayer == -1 || GameUtils.CheckMySerial(-1))
        {
            Debug.Log("loading isn't over yet");
            return;
        }
        else if(ResourceContainer.Get<ADAnteDependSetting>().bHasBeenSet == false)
        {
            Debug.LogError("betting before button set!!!!!!!!");
            PacketManager.Instance.DisConnectGotoLobby();
        }
        // if()
        
        var rec = new R_09_BET(SubGameSocket.m_bytebuffer);
        var bettingFunctions = ResourceContainer.Get<ADChipBettingManager>();

        //if (bettingFunctions.IsBettingSkipping()
        //    && rec.lBET[0].stBETMONEY <= ResourceContainer.Get<ADAnteDependSetting>().chipValueInThisRoom[eAD_BUTTONLIST._BTN_BETTING_2]
        //    && rec.lBET[0].stUSER.nSERIAL != cGlobalInfos.GetIntoRoomInfo_97().nSERIAL)
        //{
        //    Debug.Log("other betting, betting skip count++ " + bettingFunctions.bettingSkipCount
        //        + " bet money is " + (long)rec.lBET[0].stBETMONEY);
        //    bettingFunctions.bettingSkipCount++;
        //    return;
        //}
        //else
        //{
        //    ResourceContainer.Get<ADChipBettingManager>().totalBettingCount++;
        //}

        //if (rec.lBET[0].stUSER.nSERIAL != cGlobalInfos.GetIntoRoomInfo_97().nSERIAL)
        //{
        //    return;
        //}

        Debug.Log("[AD_Betting], Register");

        //Debug.Log("[R_09_BET] serial : " + rec.stBET.stUSER.nSERIAL + ", chipKind: " + rec.stBET.nBTNIDX
        //    + " table position: " + rec.stBET.nTABLEPOS
        //    + " table position2: " + rec.stBET.stPARTINFO.nTABLEPOS
        //    + " bet money: " + (long)rec.stBET.stBETMONEY
        //    + " have money " + (long)rec.stBET.stHAVEMONEY.stHAVEMONEY
        //    + " gap money " + (long)rec.stBET.stHAVEMONEY.stGAPMONEY);

        #region obsolete
        //var betInfo = rec.stBET;
        //var betting = new ADBettingAction(
        //    betInfo.stUSER.nSERIAL,
        //    betInfo.stBETMONEY,
        //    betInfo.stHAVEMONEY.stHAVEMONEY,
        //    betInfo.stHAVEMONEY.stGAPMONEY,
        //    betInfo.nTABLEPOS,
        //    betInfo.nBTNIDX,
        //    betInfo.stPARTINFO
        //    );

        // ActionPlayer.Play(betting);
        #endregion
        var betList = rec.lBET;
        // var bettingList = new ADBettingListAction(betList);
        // PlayWithSound(betList);
        // ActionPlayer.Play(bettingList);

        //if (bettingListAction == null)
        //{
        //    bettingListAction = new ADBettingListAction(betList);

        //}
        //else
        //{
        //    bettingListAction.SetBettingListAction(betList);
        //}



        if (bettingListMethod == null)
        {
            //var temp = new ADBettingListMethod(betList);
            bettingListMethod = new ADBettingListMethod(betList);
        }
        else
        {
            bettingListMethod.SetBetlist(betList);
        }
        
        
        if (GameUtils.st_maxPlayer == -1 || GameUtils.CheckMySerial(-1))
        {

        }
        else
        {

            // temp.ActionMethod();

            bettingListMethod.ActionMethod();
            
            // register
            //ActionPlayer.Play(bettingListAction);

            // rec.lBET[0]
            //var mySerial = cGlobalInfos.GetIntoRoomInfo_97().nSERIAL;
            //if (rec.lBET[0].stUSER.nSERIAL == mySerial)
            //{
            //    var bettingFunctions = ResourceContainer.Get<ADChipBettingManager>();
            //    bettingFunctions.Rec_MyBetting((eADBetPlace)rec.lBET[0].nTABLEPOS, (eAD_BUTTONLIST)rec.lBET[0].nBTNIDX, (long)rec.lBET[0].stBETMONEY);
            //}
        }


    }


    [TestMethod(false)]
    public void TestAllInEffect(int playerIndex, bool bStaying)
    {
        // var tempBettingmanager = ResourceContainer.Get<ADChipBettingManager>();
        //var tempPos = deployPos;
        
        var pp = ResourceContainer.Get<GameUserPosition>(playerIndex);

        var tempPos = pp.transform.position;
        tempPos = new Vector3(tempPos.x - 4f, tempPos.y + 5f, 100f);
        var temp = SpineEffect.Instance.spineList;
        // var temp2 = SpineEffect.Instance.spineList[5].asset;
        var temp2 = ResourceContainer.Get<ADChipBettingManager>().AllInSpine;
        var item = ResourcePool.Pop<ADAllInSpineEffectItem, EffectData>(new EffectData(
            // asset: ResourceContainer.Get<spin>().multiplierSpine,
            asset: temp2,
            animationName: "start",
            scaleFactor: 6f,
            tag: "ADAllIn"
            ));
        item.transform.position = tempPos;
        item.SetSortingLayer("WorldForward", 5057);
        if (bStaying == false)
        {

            item.Play();
        }
        else
        {
            item.PlaySpineStaying();
        }
    }

    [TestMethod(false)]
    public void TestClearAllInEffect()
    {
        ResourcePool.ClearAll<ADAllInSpineEffectItem>(ef =>
        {
            ef.tag.Equals("ADAllIn");
            return ef;
        });
    }

    [TestMethod(false)]
    public void TestDisplayList()
    {
        var temp = SpineEffect.Instance.spineList;
        var temp2 = SpineEffect.Instance.spineList[5];
        Debug.Log("asdf");
    }

}

#region obsolete
//public class ADBettingAction : IAction
//{
//    public string Log => "[R_09_ADBettingAction]";

//    public string ID => "ADBetting";

//    public List<string> CancelID => null;

//    //public stUSER_BASE stUSER;
//    //public int		nSERIAL;
//    //public string szID;

//    //public int nTABLEPOS;
//    //public int nBTNIDX;
//    //public stINT64 stBETMONEY;

//    //public stUSERMONEY stHAVEMONEY;
//    //public stINT64 stHAVEMONEY;
//    //public stINT64 stGAPMONEY;

//    //public st09_TABLE_PART stPARTINFO;
//    //public int nTABLEPOS;
//    //public stINT64 stTOTALBETMONEY;
//    //public stINT64 stRESULTMONEY;
//    //public stINT64 stMYBETMONEY;

//    private long _bet;
//    private long _have;
//    private int _userSerial;
//    private long _gap;
//    // private int _betIdx;

//    private eADBetPlace _betPlace;
//    private eAD_BUTTONLIST _chipKind;

//    public ADBettingAction(int userSerial, long bet, long have, long gap, int betPlace, int chipKind, st09_TABLE_PART tableInfo)
//    {
//        _userSerial = userSerial;
//        _bet = bet;
//        _have = have;
//        _gap = gap;
//        _betPlace = (eADBetPlace)betPlace;
//        _chipKind = (eAD_BUTTONLIST)chipKind;


//    }



//    public IEnumerator ActionRoutine()
//    {
//        // 1. set user's have money, gap money from received bet data
//        var idx = GameUtils.ConvertSerialToPosition(_userSerial);
//        var user = ResourcePool.Find<GamePlayer>(p => p.roomIdx == idx);
//        // var pp = ResourceContainer.Get<BoogyiPosition>(idx);

//        var mySerial = cGlobalInfos.GetIntoRoomInfo_97().nSERIAL;




//        if (_userSerial.Equals( mySerial) == false)
//        {
//            #region before changing nick/gap display principle...
//            //user.lbHave.SetAlpha(0f, false);

//            //// user.lbNickname.StopAllCoroutines();
//            //TimeContainer.ContainClear("UserHaveMoneyRolling");
//            //TimeContainer t1 = new TimeContainer("UserHaveMoneyRolling", GameUtils.st_globalTweenTime);

//            //// user.lbNickname.NumberKMBTween(user.Have, _have, GameUtils.st_globalTweenTime);
//            //user.lbNickname.NumberTween(user.Have, _have, g => g.ToStringWithKMB(false, 3, false), t1);

//            //// player.Gap = _gab;

//            //user.Have = _have;
//            //// user.Gap = _gap;
//            //user.lbGab.text = string.Empty;
//            //user.lbHave.color = new Color(0f, 0f, 0f, 0f);
//            #endregion

//            #region nick/gap 
//            user.lbHave.SetAlpha(0f, false);

//            // user.lbNickname.StopAllCoroutines();
//            TimeContainer.ContainClear("UserHaveMoneyRolling");
//            TimeContainer t1 = new TimeContainer("UserHaveMoneyRolling", GameUtils.st_globalTweenTime);

//            // user.lbNickname.NumberKMBTween(user.Have, _have, GameUtils.st_globalTweenTime);

//            // user.lbNickname.NumberTween(user.Gap, _gap, g => g.ToStringWithKMB(false, 3, false), t1);

//            user.lbGab.NumberTween(user.Gap, _gap, g => g.ToStringWithKMB(false, 3, false), t1);

//            user.lbHave.color = new Color(0f, 0f, 0f, 0f);

//            // player.Gap = _gab;

//            //user.Have = _have;
//            // user.Gap = _gap;
//            //user.lbGab.text = string.Empty;
//            #endregion

//        }
//        else
//        {
//            // my pad's havemoeny and gap money
//            // ResourceContainer.Get<TextMeshProUGUI>("MyMoney").text = _have.ToStringWithKMB();
//            ResourceContainer.Get<ADMyInfoTag>("MyInfo").haveMoney = _have;
//            ResourceContainer.Get<ADMyInfoTag>("MyInfo").gapMoney = _gap;
//        }


//        // 2. bet chip to betting place
//        var bettingFunctions = ResourceContainer.Get<ADChipBettingManager>();

//        if(_userSerial == mySerial)
//        {
//            Debug.Log("[R_09_BET], my betting");
//            bettingFunctions.Rec_MyBetting(_betPlace, _chipKind, _bet);

//            // ResourceContainer.Get<ADChipBettingManager>().OnBetting()
//        }
//        else
//        {
//            Debug.Log("[R_09_BET], others betting");
//            bettingFunctions.Rec_BettingFromOthers(user.transform.position, _betPlace, _chipKind, _bet, idx);

//        }
//        // Sound.Instance.EffPlay("GOLD_MOVE"); -> removed


//        // 3. fill the gap between server and client for tablepart infomation...
//        // -> if my information is diferrent from server -> fill that gap.
//        // more like error handling?
//        // if server has more info(money that table pos) -> spawn more...


//        yield return null;
//    }

//    public IEnumerable<TimeContainer> GetAllTimeContainerList()
//    {
//        // return null;
//        yield break;
//    }

//}


#endregion

#region obsolete
//public class ADBettingListAction : IAction
//{
//    public string Log => "[R_09_ADBettingAction]";

//    // public string ID => "ADBetting";
//    public string ID => null;

//    public List<string> CancelID => null;



//    private List<st09_BET_DATA> _betList;
//    private long _bet;
//    private long _have;
//    private int _userSerial;
//    private long _gap;
//    // private int _betIdx;

//    private eADBetPlace _betPlace;
//    private eAD_BUTTONLIST _chipKind;

//    // public ADBettingListAction(int userSerial, long bet, long have, long gap, int betPlace, int chipKind, st09_TABLE_PART tableInfo)
//    public ADBettingListAction(List<st09_BET_DATA> betList)
//    {
//        _betList = betList;
//    }
//    public void SetBettingListAction(List<st09_BET_DATA> betList)
//    {
//        _betList = betList;
//    }

//    public IEnumerator ActionRoutine()
//    {

//        long tempHaveMoney = 0;
//        long tempGapMoney = 0;
//        long tempBetMoney = 0;
//        int chipTotalCount = 0;
//        int idx = 0;
//        GamePlayer user = null;
//        var mySerial = cGlobalInfos.GetIntoRoomInfo_97().nSERIAL;

//        #region 0. error handling
//        if (_betList[0].stHAVEMONEY.stHAVEMONEY < 0)
//        {
//            yield break;
//        }
//        #endregion

//        foreach (var betItem in _betList)
//        {
//            // 1. set user's have money, gap money from received bet data
//            if(idx ==0)
//            {
//                idx = GameUtils.ConvertSerialToPosition(betItem.stUSER.nSERIAL);
//            }
//            if(user == null)
//            {
//                user = ResourcePool.Find<GamePlayer>(p => p.roomIdx == idx);
//            }
            

            
//            if (betItem.stUSER.nSERIAL.Equals(mySerial) == false)
//            {

//                #region nick/gap 
//                user.lbHave.SetAlpha(0f, false);

//                // user.lbNickname.StopAllCoroutines();
//                TimeContainer.ContainClear("UserHaveMoneyRolling");
//                TimeContainer t1 = new TimeContainer("UserHaveMoneyRolling", GameUtils.st_globalTweenTime);

//                // user.lbNickname.NumberKMBTween(user.Have, _have, GameUtils.st_globalTweenTime);
                

//                // player.Gap = _gab;
//                if(_betList.Count == 1)
//                {

//                    ResourceContainer.Get<ADGameMain>().SetPlayerGapMoney(user, betItem.stHAVEMONEY.stGAPMONEY);

//                    //user.Gap = betItem.stHAVEMONEY.stGAPMONEY;

//                    user.lbHave.color = new Color(0f, 0f, 0f, 0f);
//                }
                
//                #endregion

//            }
//            else
//            {
//                // my pad's havemoeny and gap money
//                // ResourceContainer.Get<TextMeshProUGUI>("MyMoney").text = _have.ToStringWithKMB();
//                if (_betList.Count == 1)
//                {
//                    Debug.Log("[R_09_BET] user name " + user.Nick
//                    + " user gap money " + (long)betItem.stHAVEMONEY.stGAPMONEY
//                    + " betting pos " + (eADBetPlace)betItem.nTABLEPOS);
//                    ResourceContainer.Get<ADMyInfoTag>("MyInfo").haveMoney = betItem.stHAVEMONEY.stHAVEMONEY;
//                    ResourceContainer.Get<ADMyInfoTag>("MyInfo").gapMoney = betItem.stHAVEMONEY.stGAPMONEY;
//                }
//            }
            
//            if (_betList.Count == 1)
//            {
//                user.Have = betItem.stHAVEMONEY.stHAVEMONEY;
//            }
//            else if (_betList.Count > 1)
//            {
//                // tempHaveMoney += betItem.stHAVEMONEY.stHAVEMONEY;
//                tempHaveMoney = betItem.stHAVEMONEY.stHAVEMONEY; // all havemoney are same in list
//                tempGapMoney = betItem.stHAVEMONEY.stGAPMONEY;
//                tempBetMoney += betItem.stBETMONEY;

//                var tempChips = ResourceContainer.Get<ADChipBettingManager>().GetChipsFromMoney(betItem.stBETMONEY);
//                foreach (var chip in tempChips)
//                {
//                    chipTotalCount += chip.Value;
//                }
//            }


//            // 2. bet chip to betting place
//            var bettingFunctions = ResourceContainer.Get<ADChipBettingManager>();
//            var gameMain = ResourceContainer.Get<ADGameMain>();
//            // gameMain.IncrementCounts(idx);
//            if (betItem.stUSER.nSERIAL == mySerial)
//            {
//                // Debug.Log("[R_09_BET], my betting, count is " + gameMain.playersBettingCount[0]);

//                // bettingFunctions.Rec_MyBetting(_betPlace, _chipKind, _bet);

//                //bettingFunctions.Rec_MyBetting( (eADBetPlace) betItem.nTABLEPOS, (eAD_BUTTONLIST)betItem.nBTNIDX, (long)betItem.stBETMONEY);

//                ResourceContainer.Get<ADTest_2>().OnBetting((eADBetPlace)betItem.nTABLEPOS);


//                // ResourceContainer.Get<ADChipBettingManager>().OnBetting()
//            }
//            else
//            {
//                // Debug.Log("[R_09_BET], others betting, count is " + gameMain.playersBettingCount[idx]);

//                //bettingFunctions.Rec_BettingFromOthers(user.transform.position, 
//                //    (eADBetPlace)betItem.nTABLEPOS, 
//                //    (eAD_BUTTONLIST)betItem.nBTNIDX, 
//                //    (long)betItem.stBETMONEY,
//                //    idx);
                
//                ResourceContainer.Get<ADTest_2>().OnBettingForAuto((eADBetPlace)betItem.nTABLEPOS);

//            }
//        }

//        // sound related for more than 1
//        if (_betList.Count > 1)
//        {

//            // 1. play sound
//            //Debug.Log("chip total count is " + chipTotalCount
//            //    + " least chip value in this room " + ResourceContainer.Get<ADAnteDependSetting>().chipValueInThisRoom[eAD_BUTTONLIST._BTN_BETTING_1]);
//            ResourceContainer.Get<ADChipBettingManager>().PlayBettingSound(chipTotalCount, false);

//            // 2. change user gap,have moneys

//            // idx = GameUtils.ConvertSerialToPosition(_betList[0].stUSER.nSERIAL);
//            // user = ResourcePool.Find<GamePlayer>(p => p.roomIdx == idx);


//            // mySerial = cGlobalInfos.GetIntoRoomInfo_97().nSERIAL;
//            if (_betList[0].stUSER.nSERIAL.Equals(mySerial) == false)
//            {
//                TimeContainer.ContainClear("UserHaveMoneyRolling");
//                TimeContainer t1 = new TimeContainer("UserHaveMoneyRolling", GameUtils.st_globalTweenTime);



//                user.Have = tempHaveMoney - tempBetMoney;
//                // user.Gap = (tempGapMoney - tempBetMoney);
//                ResourceContainer.Get<ADGameMain>().SetPlayerGapMoney(user, (tempGapMoney - tempBetMoney));

//                user.lbHave.color = new Color(0f, 0f, 0f, 0f);
//            }
//            else
//            {
//                ResourceContainer.Get<ADMyInfoTag>("MyInfo").haveMoney = tempHaveMoney - tempBetMoney;
//                ResourceContainer.Get<ADMyInfoTag>("MyInfo").gapMoney = tempGapMoney - tempBetMoney;
//            }



//        }
//        else if(_betList.Count == 1)
//        {
//            //Debug.Log("[R_09_BET] user name " + user.Nick
//            //        + " user gap money " + tempGapMoney);
//            ResourceContainer.Get<ADChipBettingManager>().PlayBettingSound(1, false);
//        }


//        CheckAndPlayAllInEffect(_betList.Count, user, tempHaveMoney, tempBetMoney);

//        // yield return null;
//    }

//    public IEnumerable<TimeContainer> GetAllTimeContainerList()
//    {
//        // return null;
//        yield break;
//    }
//    //public void PlayAllInEffect(int playerIndex, bool bStaying, bool bIsMine)
//    //{
//    //    // var tempBettingmanager = ResourceContainer.Get<ADChipBettingManager>();
//    //    //var tempPos = deployPos;

//    //    var pp = ResourceContainer.Get<GameUserPosition>(playerIndex);

//    //    var tempPos = pp.transform.position;
//    //    tempPos = new Vector3(tempPos.x - 4f, tempPos.y + 5f, 100f);
//    //    var temp = SpineEffect.Instance.spineList;
//    //    // var temp2 = SpineEffect.Instance.spineList[5].asset;
//    //    var temp2 = ResourceContainer.Get<ADChipBettingManager>().AllInSpine;
//    //    var item = ResourcePool.Pop<ADAllInSpineEffectItem, EffectData>(new EffectData(
//    //        // asset: ResourceContainer.Get<spin>().multiplierSpine,
//    //        asset: temp2,
//    //        animationName: "start",
//    //        scaleFactor: 6f,
//    //        tag: "ADAllIn"
//    //        ));
//    //    item.transform.position = tempPos;
//    //    item.SetSortingLayer("WorldForward", 5057);
//    //    if (bStaying == false)
//    //    {

//    //        item.Play();
//    //    }
//    //    else
//    //    {
//    //        item.PlaySpineStaying();
//    //    }
//    //}
//    public void CheckAndPlayAllInEffect(int betListCount, GamePlayer user, long tempHaveMoney, long tempBetMoney)
//    {
//        var mySerial = cGlobalInfos.GetIntoRoomInfo_97().nSERIAL;
//        if (betListCount > 1)
//        {
//            if ((tempHaveMoney - tempBetMoney) < ResourceContainer.Get<ADAnteDependSetting>().chipValueInThisRoom[eAD_BUTTONLIST._BTN_BETTING_1]
//            && ResourceContainer.Get<ADChipBettingManager>().AllInActiveDic.ContainsKey(user.roomSerial) == false)
//            {
//                //Debug.Log("user " + user.Nick + " have money " + user.Have
//                //    + "current room least chip value is " + ResourceContainer.Get<ADAnteDependSetting>().chipValueInThisRoom[eAD_BUTTONLIST._BTN_BETTING_1]);

//                ResourceContainer.Get<ADChipBettingManager>().AllInActiveDic.Add(user.roomSerial, true);
//                // PlayAllInEffect(user.roomIdx, true, user.roomSerial == mySerial ? true : false);
//                ResourceContainer.Get<ADGameMain>().PlayAllInEffect(user.roomIdx, true, user.roomSerial == mySerial ? true : false);
//                // when all in effect activated, bettingboard function comes in
//                if(user.roomSerial.Equals(mySerial))
//                {
//                    ResourceContainer.Get<ADChipBettingManager>().SetEnableBettingBoards(false);
//                }
//            }
//        }
//        else
//        {
//            if (user.Have < ResourceContainer.Get<ADAnteDependSetting>().chipValueInThisRoom[eAD_BUTTONLIST._BTN_BETTING_1]
//            && ResourceContainer.Get<ADChipBettingManager>().AllInActiveDic.ContainsKey(user.roomSerial) == false)
//            {
//                //Debug.Log("user " + user.Nick + " have money " + user.Have
//                //    + "current room least chip value is " + ResourceContainer.Get<ADAnteDependSetting>().chipValueInThisRoom[eAD_BUTTONLIST._BTN_BETTING_1]);

//                ResourceContainer.Get<ADChipBettingManager>().AllInActiveDic.Add(user.roomSerial, true);
//                // PlayAllInEffect(user.roomIdx, true, user.roomSerial == mySerial ? true : false);
//                ResourceContainer.Get<ADGameMain>().PlayAllInEffect(user.roomIdx, true, user.roomSerial == mySerial ? true : false);
//                // when all in effect activated, bettingboard function comes in
//                if (user.roomSerial.Equals(mySerial))
//                {
//                    ResourceContainer.Get<ADChipBettingManager>().SetEnableBettingBoards(false);
//                }
//            }
//        }
        
//    }


//}

#endregion

public class ADBettingListMethod
{
    // public string Log => "[R_09_ADBettingAction]";

    // public string ID => "ADBetting";

    // public List<string> CancelID => null;



    private List<st09_BET_DATA> _betList;
    

    // public ADBettingListAction(int userSerial, long bet, long have, long gap, int betPlace, int chipKind, st09_TABLE_PART tableInfo)
    public ADBettingListMethod(List<st09_BET_DATA> betList)
    {
        _betList = betList;
    }
    public void SetBetlist(List<st09_BET_DATA> betList)
    {
        _betList = betList;
    }
    public void ActionMethod()
    {
        Debug.Log("[AD_Betting], Start ");
        
        

        var bettingFunctions = ResourceContainer.Get<ADChipBettingManager>();
        var gameMain = ResourceContainer.Get<ADGameMain>();
        // 0. start purformance related coroutine.
        if(bettingFunctions.bIsBettingTime == false)
        {
            bettingFunctions.bIsBettingTime = true;
        }
        //if(bettingFunctions.bIsBettingRoutineOff == true)
        //{
        //    Debug.Log("other betting, turn on routine");
        //    // bettingFunctions.StartCoroutine(bettingFunctions.OthersBettingSkipRoutine());
        //}
        if(bettingFunctions.bIsSoundRoutineOff == true)
        {
            Debug.Log("other skip betting, turn on routine");
            bettingFunctions.StartCoroutine(bettingFunctions.OtherBettingSoundSkipRoutine());
        }
        
        

        long tempHaveMoney = 0;
        long tempGapMoney = 0;
        long tempBetMoney = 0;
        int chipTotalCount = 0;
        int idx = -1;
        GamePlayer user = null;
        var mySerial = cGlobalInfos.GetIntoRoomInfo_97().nSERIAL;

        #region 0. error handling
        if (_betList[0].stHAVEMONEY.stHAVEMONEY < 0)
        {
            // yield break;
            return;
        }
        #endregion
        
        foreach (var betItem in _betList)
        {
            // 1. set user's have money, gap money from received bet data
            if (idx == -1)
            {
                idx = GameUtils.ConvertSerialToPosition(betItem.stUSER.nSERIAL);
            }
            if (user == null)
            {
                user = ResourcePool.Find<GamePlayer>(p => p.roomIdx == idx);
            }

            Debug.Log("[AD_Betting], betitem have money is " + (long)betItem.stHAVEMONEY.stHAVEMONEY
                    + " gap money is " + (long)betItem.stHAVEMONEY.stGAPMONEY
                    + " betlist count is " + _betList.Count
                    + " bet index is " + betItem.nBTNIDX
                    + " bet money " + (long)betItem.stBETMONEY);

            //if (_betList.Count == 1)
            //{
            //    user.Have = betItem.stHAVEMONEY.stHAVEMONEY;

            //    // Debug.Log("betlist, count is 1, money is " + (long)betItem.stBETMONEY);
            //}
            // else if (_betList.Count > 1)
            {
                
                // tempHaveMoney += betItem.stHAVEMONEY.stHAVEMONEY;
                tempHaveMoney = betItem.stHAVEMONEY.stHAVEMONEY; // all havemoney are same in list
                tempGapMoney = betItem.stHAVEMONEY.stGAPMONEY;
                tempBetMoney += betItem.stBETMONEY;

                var tempChips = ResourceContainer.Get<ADChipBettingManager>().GetChipsFromMoney(betItem.stBETMONEY);
                foreach (var chip in tempChips)
                {
                    chipTotalCount += chip.Value;
                }
            }


            // 2. bet chip to betting place
            
            // gameMain.IncrementCounts(idx);
            if (betItem.stUSER.nSERIAL == mySerial)
            {
                //Debug.Log("[R_09_BET], my betting, count is " + gameMain.playersBettingCount[0]);

                // bettingFunctions.Rec_MyBetting(_betPlace, _chipKind, _bet);

                bettingFunctions.Rec_MyBetting((eADBetPlace)betItem.nTABLEPOS, (eAD_BUTTONLIST)betItem.nBTNIDX, (long)betItem.stBETMONEY);

                bettingFunctions.totalBettingCount++;

            }
            else
            {

                bettingFunctions.Rec_BettingFromOthers(user.transform.position,
                    (eADBetPlace)betItem.nTABLEPOS,
                    (eAD_BUTTONLIST)betItem.nBTNIDX,
                    (long)betItem.stBETMONEY,
                    idx);

                // skip for purformance
                //Debug.Log("other betting, betting skip count is " + bettingFunctions.bettingSkipCount
                //        + " betting count is " + bettingFunctions.otherBettingCount);
                //if (bettingFunctions.IsBettingSkipping() && betItem.stBETMONEY <= ResourceContainer.Get<ADAnteDependSetting>().chipValueInThisRoom[eAD_BUTTONLIST._BTN_BETTING_2])
                //{
                //    //Debug.Log("other betting, betting skip count++ " + bettingFunctions.bettingSkipCount
                //    //    + " bet money is " + (long)betItem.stBETMONEY);

                //    bettingFunctions.bettingSkipCount++;
                //}
                //else
                //{
                //    // Debug.Log("other betting, betting count++ " + bettingFunctions.otherBettingCount);
                    
                //    bettingFunctions.totalBettingCount++;

                //    //bettingFunctions.Rec_BettingFromOthers(user.transform.position,
                //    //(eADBetPlace)betItem.nTABLEPOS,
                //    //(eAD_BUTTONLIST)betItem.nBTNIDX,
                //    //(long)betItem.stBETMONEY,
                //    //idx);
                //}


            }
        }

        // sound related for more than 1
        if (_betList.Count > 1)
        {
            // ResourceContainer.Get<ADChipBettingManager>().PlayBettingSound(chipTotalCount, false);
            if (_betList[0].stUSER.nSERIAL == mySerial)
            {
                ResourceContainer.Get<ADChipBettingManager>().PlayBettingSound(chipTotalCount, true);
            }
            else
            {
                ResourceContainer.Get<ADChipBettingManager>().PlayBettingSound(chipTotalCount, false);
            }

        }
        else if (_betList.Count == 1)
        {
            if(_betList[0].stUSER.nSERIAL == mySerial)
            {
                ResourceContainer.Get<ADChipBettingManager>().PlayBettingSound(1, true);
            }
            else
            {
                ResourceContainer.Get<ADChipBettingManager>().PlayBettingSound(1, false);
            }
            
        }
        #region
        // 2. change user gap,have moneys
        if (_betList.Count > 1)
        {
            if (_betList[0].stUSER.nSERIAL.Equals(mySerial) == false)
            {
                TimeContainer.ContainClear("UserHaveMoneyRolling");
                TimeContainer t1 = new TimeContainer("UserHaveMoneyRolling", GameUtils.st_globalTweenTime);


                // user.Have = tempHaveMoney - tempBetMoney;
                // user.Gap = (tempGapMoney - tempBetMoney);
                // ResourceContainer.Get<ADGameMain>().SetPlayerGapMoney(user, (tempGapMoney - tempBetMoney));
                // ResourceContainer.Get<ADGameMain>().SetPlayerGapMoney(user, (tempGapMoney - tempBetMoney));
                // user.lbHave.color = new Color(0f, 0f, 0f, 0f);
            }
        }

        if (_betList[0].stUSER.nSERIAL.Equals(mySerial) == false)
        {
            var chipKind = _betList[0].nBTNIDX;
            if (eAD_BUTTONLIST._BTN_BETTING_1 > (eAD_BUTTONLIST)chipKind || (eAD_BUTTONLIST)chipKind > eAD_BUTTONLIST._BTN_BETTING_4)
            {

            }
            else
            {

            }
            var tempGapDic = ResourceContainer.Get<ADGameMain>().gapMoneysDic;
            user.Have = tempHaveMoney - tempBetMoney;
            if (tempGapDic.ContainsKey(user))
            {
                var startGap = tempGapDic[user];
                Debug.Log("startgap is " + startGap
                    + " temp bet money " + tempBetMoney);
                ResourceContainer.Get<ADGameMain>().SetPlayerGapMoney(user, (startGap - tempBetMoney));
            }
            else
            {
                tempGapDic.Add(user, 0);
                var startGap = tempGapDic[user];
                Debug.Log("startgap is " + startGap
                    + " temp bet money " + tempBetMoney);
                ResourceContainer.Get<ADGameMain>().SetPlayerGapMoney(user, (startGap - tempBetMoney));
            }
            user.lbHave.color = new Color(0f, 0f, 0f, 0f);
        }
        else
        {
            var betItem = _betList[0];
            var chipKind = _betList[0].nBTNIDX;
            var tempStartGap = ResourceContainer.Get<ADMyInfoTag>("MyInfo").gapMoney;
            if (eAD_BUTTONLIST._BTN_BETTING_1 > (eAD_BUTTONLIST)chipKind || (eAD_BUTTONLIST)chipKind > eAD_BUTTONLIST._BTN_BETTING_4)
            {
                ResourceContainer.Get<ADMyInfoTag>("MyInfo").haveMoney = tempHaveMoney - tempBetMoney;

                Debug.Log("startgap is " + tempStartGap
                    + " temp bet money " + tempBetMoney
                    + " endgap is " + (tempStartGap - tempBetMoney)
                    + " previous bet");
                ResourceContainer.Get<ADMyInfoTag>("MyInfo").gapMoney -= tempBetMoney;
            }
            else
            {
                ResourceContainer.Get<ADMyInfoTag>("MyInfo").haveMoney = betItem.stHAVEMONEY.stHAVEMONEY;
                
                Debug.Log("startgap is " + tempStartGap
                    + " temp bet money " + tempBetMoney
                    + " endgap is " + (long)betItem.stHAVEMONEY.stGAPMONEY
                    + " not previous bet");
                ResourceContainer.Get<ADMyInfoTag>("MyInfo").gapMoney = betItem.stHAVEMONEY.stGAPMONEY;
                
            }
            
        }

        #endregion

        Debug.Log("bet list count is " + _betList.Count
            + " user have money is " + user.Have
            + " least chip value " + ResourceContainer.Get<ADAnteDependSetting>().chipValueInThisRoom[eAD_BUTTONLIST._BTN_BETTING_1]);
        CheckAndPlayAllInEffect(_betList.Count, user, tempHaveMoney, tempBetMoney);
        Debug.Log("[AD_Betting], End ");
    }

    public IEnumerable<TimeContainer> GetAllTimeContainerList()
    {
        // return null;
        yield break;
    }
    public void CheckAndPlayAllInEffect(int betListCount, GamePlayer user, long tempHaveMoney, long tempBetMoney)
    {
        var mySerial = cGlobalInfos.GetIntoRoomInfo_97().nSERIAL;
        if (betListCount > 1)
        {
            if ((tempHaveMoney - tempBetMoney) < ResourceContainer.Get<ADAnteDependSetting>().chipValueInThisRoom[eAD_BUTTONLIST._BTN_BETTING_1]
            && ResourceContainer.Get<ADChipBettingManager>().AllInActiveDic.ContainsKey(user.roomSerial) == false)
            {
                ResourceContainer.Get<ADChipBettingManager>().AllInActiveDic.Add(user.roomSerial, true);
                // PlayAllInEffect(user.roomIdx, true, user.roomSerial == mySerial ? true : false);
                
                ResourceContainer.Get<ADGameMain>().PlayAllInEffect(user.roomIdx, true, user.roomSerial == mySerial ? true : false);
                // when all in effect activated, bettingboard function comes in
                if (user.roomSerial.Equals(mySerial))
                {
                    ResourceContainer.Get<ADChipBettingManager>().SetEnableBettingBoards(false);
                }
            }
        }
        else
        {
            if (user.roomSerial.Equals(mySerial))
            {
                if (ResourceContainer.Get<ADMyInfoTag>("MyInfo").haveMoney < ResourceContainer.Get<ADAnteDependSetting>().chipValueInThisRoom[eAD_BUTTONLIST._BTN_BETTING_1]
                    && ResourceContainer.Get<ADChipBettingManager>().AllInActiveDic.ContainsKey(user.roomSerial) == false)
                {

                    ResourceContainer.Get<ADChipBettingManager>().AllInActiveDic.Add(user.roomSerial, true);

                    ResourceContainer.Get<ADGameMain>().PlayAllInEffect(user.roomIdx, true, user.roomSerial == mySerial ? true : false);
                    ResourceContainer.Get<ADChipBettingManager>().SetEnableBettingBoards(false);
                }
            }
            else
            {
                if (user.Have < ResourceContainer.Get<ADAnteDependSetting>().chipValueInThisRoom[eAD_BUTTONLIST._BTN_BETTING_1]
                    && ResourceContainer.Get<ADChipBettingManager>().AllInActiveDic.ContainsKey(user.roomSerial) == false)
                {
                    ResourceContainer.Get<ADChipBettingManager>().AllInActiveDic.Add(user.roomSerial, true);

                    ResourceContainer.Get<ADGameMain>().PlayAllInEffect(user.roomIdx, true, user.roomSerial == mySerial ? true : false);
                    // ResourceContainer.Get<ADChipBettingManager>().SetEnableBettingBoards(false);
                }
            }
            //if (user.Have < ResourceContainer.Get<ADAnteDependSetting>().chipValueInThisRoom[eAD_BUTTONLIST._BTN_BETTING_1]
            //&& ResourceContainer.Get<ADChipBettingManager>().AllInActiveDic.ContainsKey(user.roomSerial) == false)
            //{
            //    //Debug.Log("user " + user.Nick + " have money " + user.Have
            //    //    + "current room least chip value is " + ResourceContainer.Get<ADAnteDependSetting>().chipValueInThisRoom[eAD_BUTTONLIST._BTN_BETTING_1]);

            //    ResourceContainer.Get<ADChipBettingManager>().AllInActiveDic.Add(user.roomSerial, true);
            //    // PlayAllInEffect(user.roomIdx, true, user.roomSerial == mySerial ? true : false);
            //    ResourceContainer.Get<ADGameMain>().PlayAllInEffect(user.roomIdx, true, user.roomSerial == mySerial ? true : false);
            //    // when all in effect activated, bettingboard function comes in
            //    if (user.roomSerial.Equals(mySerial))
            //    {

            //        ResourceContainer.Get<ADChipBettingManager>().SetEnableBettingBoards(false);
            //    }
            //}
        }

    }


}