using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Wooriline;

public class ADResult : PacketHandler
{
    public override int GetNumber()
    {
        return (int)ANIMALDICE_PK.R_09_RESULT;
    }

    public override void Func()
    {
        var rec = new R_09_RESULT(SubGameSocket.m_bytebuffer);
        Debug.Log("[R_09_RESULT] " + ", time : " + rec.nTIME / 1000f + ", viewTime : " + rec.nVIEWTIME / 1000f);

        foreach (var winloseUser in rec.lWINLOSE)
        {
            Debug.Log("[R_09_RESULT] " + "user name : " + winloseUser.stUSER.szID 
                + ", have money : " +   (long)winloseUser.stHAVEMONEY.stHAVEMONEY
                + ", result money : " + (long)winloseUser.stRESULTMONEY
                + ", gap money : " +    (long)winloseUser.stHAVEMONEY.stGAPMONEY);
        }

        // chip (that's from betting board) move to dealer time?
        // var winTables = rec.lWINPART;
        //var resultUsers = rec.lWINLOSE;
        // var loserResultMoney = new List<long>();
        // var loserHaveMoney = new List<long>();
        // var loserBettingMoney = new List<long>();

        //foreach (var loser in rec.lLOSEPART)
        //{

        //}
        var resultAction = new ADResultAction(rec.nTIME / 1000f, rec.nVIEWTIME / 1000f, rec.lWINLOSE);
        ActionPlayer.Play(resultAction);

        
    }
    [TestMethod]
    public void TestViewText()
    {
        List<st09_WINLOSE> tempList = new List<st09_WINLOSE>(6);
        st09_WINLOSE tempItem1 = new st09_WINLOSE();
        st09_WINLOSE tempItem2 = new st09_WINLOSE();

        tempItem1.stUSER.szID = "asdf";
        tempItem1.stUSER.nSERIAL = 0;

        tempItem1.nTABLEPOS = (int)eADBetPlace._BET_ANIMAL_DAK;
        tempItem1.nWINLOSE = 1;
        tempItem1.stBETMONEY = 1000;
        tempItem1.stHAVEMONEY.stHAVEMONEY = 4000;
        tempItem1.stHAVEMONEY.stGAPMONEY = -1000;
        tempItem1.stRESULTMONEY = -2000;

        tempItem2.stUSER.szID = "qwer";
        tempItem2.stUSER.nSERIAL = 4;

        tempItem2.nTABLEPOS = (int)eADBetPlace._BET_ANIMAL_DAK;
        tempItem2.nWINLOSE = 1;
        tempItem2.stBETMONEY = 1000;
        tempItem2.stHAVEMONEY.stHAVEMONEY = 6000;
        tempItem2.stHAVEMONEY.stGAPMONEY = -2000;
        tempItem2.stRESULTMONEY = -4000;

        tempList.Add(tempItem1);
        tempList.Add(tempItem2);

        //tempList.Add()
        var resultAction = new ADResultAction(4f, 2f, tempList);
        ActionPlayer.Play(resultAction);

    }
    [TestMethod]
    public void TestViewText2()
    {
        TimeContainer.Stack _viewTime = new TimeContainer.Stack(11 * 2, 1.5f, "AD_RESULT_VIEW_TIME");
        var player = ResourcePool.Find<GamePlayer>(p => p.roomSerial == 0);
        var resultText = ResourcePool.Pop<ADResultGoldText, long>(20000);
        // resultText.transform.position = ResourceContainer.Get<GameUserPosition>(0).transform.position;
        resultText.transform.position = ResourceContainer.Get<ResourceTag>("ADMyWinLoseMoneyText").transform.position;

        player.userPhoto.AlphaTween(0.4f, _viewTime.Pop()); // . color. = new Color(1, 1, 1, 0.5f);

        //yield return resultText.Wait(_viewTime.Pop());


        // resultText.Wait(_viewTime.Pop());
        // player.userPhoto.SetAlpha(1f);

        var resultGoldTexts = ResourcePool.GetAll<ADResultGoldText>();
        //foreach (var text in resultGoldTexts)
        //{
        //    text.render.AlphaTween(0f, _viewTime.Pop());
        //}

        // consider refactor this..?
        CoroutineChain.Start
            .Wait(_viewTime.Pop())
            .Wait(0.01f)
            .Call(() => player.userPhoto.AlphaTween(1f,0.01f))
            .Call(() =>
            {
                foreach (var text in resultGoldTexts)
                {
                    text.render.AlphaTween(0f, _viewTime.Pop());
                }
            });

    }

}

public class ADResultAction : IAction
{
    public string Log => "[R_09_ADResultAction]";

    public string ID => "ADResultAction";

    public List<string> CancelID => new List<string> { "ADResultAction" };

    TimeContainer.Stack _totalTime;
    TimeContainer.Stack _viewTime;

    TimeContainer.StackSum _all;

    List<st09_WINLOSE> _resultUsers = new List<st09_WINLOSE>();

    public ADResultAction(float totalTime, float viewTime, List<st09_WINLOSE> resultUsers)
    {
        _resultUsers = resultUsers;

        _totalTime = new TimeContainer.Stack(1, totalTime * 3 / 4, "AD_RESULT_TOTAL_TIME"); // 
        // _viewTime = new TimeContainer.Stack( (resultUsers.Count * 3) + 11 + 1 + 1,  viewTime, "AD_RESULT_VIEW_TIME");
        _viewTime = new TimeContainer.Stack((11 * 4) + 1 + 1, viewTime, "AD_RESULT_VIEW_TIME");

        _all = new TimeContainer.StackSum(_totalTime, _viewTime);
    }

    public IEnumerator ActionRoutine()
    {
        // 0. update history
        long myResultMoney = 0;
        var mySerial = cGlobalInfos.GetIntoRoomInfo_97().nSERIAL;
        foreach (var winUser in _resultUsers)
        {
            // var idx = GameUtils.ConvertSerialToPosition(winUser.stUSER.nSERIAL);
            // var user = ResourcePool.Find<GamePlayer>(p => p.roomIdx == idx);
            if (mySerial == winUser.stUSER.nSERIAL)
            {
                myResultMoney += winUser.stRESULTMONEY;
            }
        }

        var history = ResourceContainer.Get<ADHistoryPopup>();
        // history.AddHistory(history.currentDiceValues[0], history.currentDiceValues[1], history.currentDiceValues[2], myResultMoney);
        history.ModifyMoneySprite(history.currentGameIndex,myResultMoney);

        Debug.Log("my result money is " + myResultMoney);

        for (int i = 0; i < _resultUsers.Count; i++)
        {
            var roomIndex = _resultUsers[i].stUSER.nSERIAL.ConvertToRoomIdx();
            var player = ResourcePool.Find<GamePlayer>(p => p.roomIdx == roomIndex);
            var currentResultUser = _resultUsers[i];
            
            // var pp = ResourceContainer.Get<GameUserPosition>

            // 1. display spine FX
            var resultText = ResourcePool.Pop<ADResultGoldText, long>(currentResultUser.stRESULTMONEY);
            resultText.transform.position = ResourceContainer.Get<GameUserPosition>(currentResultUser.stUSER.nSERIAL.ConvertToRoomIdx()).transform.position;
            
            if(currentResultUser.stUSER.nSERIAL == mySerial)
            {
                //"ADMyWinLoseMoneyText"
                resultText.transform.position = ResourceContainer.Get<ResourceTag>("ADMyWinLoseMoneyText").transform.position;
                resultText.render.rectTransform.sizeDelta = new Vector2(120f * 1.2f,
                30f * 1.2f);
                resultText.render.fontSize = 36;
                var t1 = _viewTime.Pop(); // user count
                
                if (currentResultUser.stRESULTMONEY > 0)
                {
                    if(currentResultUser.stRESULTMONEY >= cGlobalInfos.GetIntoRoomInfo_97().stMAX_BETMONEY)
                    {
                        ResourceContainer.Get<ADResultPartInfoStoring>().TestSpineWin(t1.time, 3);
                    }
                    else
                    {
                        ResourceContainer.Get<ADResultPartInfoStoring>().TestSpineWin(t1.time, 2);
                    }

                    ResourceContainer.Get<ADResultPartInfoStoring>().Wait(t1);
                }
                else
                {
                    ResourceContainer.Get<ADResultPartInfoStoring>().TestSpineLose(t1.time);
                    ResourceContainer.Get<ADResultPartInfoStoring>().Wait(t1);
                }
                
                // 1.1 play sound if my player is won -> removed, and moved into spine FX

                if ((long)currentResultUser.stRESULTMONEY > 0)
                {
                    // Sound.Instance.EffPlay("AD_Win");
                }
                else
                {

                }

            }
            else // not my serial
            {
                resultText.render.rectTransform.sizeDelta = new Vector2(120f,
                30f);
                resultText.render.fontSize = 29;
            }

            if (currentResultUser.stUSER.nSERIAL.Equals(mySerial) == false)
            {
                // player.userPhoto.AlphaTween(0.4f, _viewTime.Pop());
                if(player != null)
                {
                    player.userPhoto.ColorTween(new Color(0.4f, 0.4f, 0.4f, 1f), _viewTime.Pop()); // user count * 2 // removed... from only my player
                }
            }

        }

        var tempTime = _totalTime.Pop(); // (user count * 2) + 1
        ResourceContainer.Get<ADResultPartInfoStoring>().Wait(tempTime);
        // yield return new WaitForSeconds(tempTime.time);

        yield return new WaitForSeconds(tempTime.time);
        //

        // 2. display result money

        var resultGoldTexts = ResourcePool.GetAll<ADResultGoldText>();
        ResourceContainer.Get<ADResultPartInfoStoring>().TestSpineClear();
        var tempUsers = ResourcePool.GetAll<GamePlayer>();
        foreach (var user in tempUsers)
        {
            if(user.roomSerial.Equals(-1) == false)
            {
                // user.userPhoto.AlphaTween(1f, 0.01f);
                user.userPhoto.ColorTween(new Color(1f, 1f, 1f, 1f), 0.01f);
            }
        }
        foreach (var text in resultGoldTexts)
        {
            text.render.AlphaTween(0f, _viewTime.Pop()); // (user count * 3) + 1
        }
        

        var t2 = _viewTime.Pop(); // (user count * 3) + 2
        yield return new WaitForSeconds(t2.time / 2); 

        // 3. apply money changes
        foreach (var resultUser in _resultUsers)
        {
            // ResourceContainer.Get<TextMeshProUGUI>("MyMoney").text =     ((long)resultUser.stHAVEMONEY.stHAVEMONEY).ToStringWithKMB();
            // ResourceContainer.Get<TextMeshProUGUI>("MyGap").text =       ((long)resultUser.stHAVEMONEY.stGAPMONEY).ToStringWithKMB();
            

            if (resultUser.stUSER.nSERIAL == mySerial)
            {
                ResourceContainer.Get<ADMyInfoTag>("MyInfo").haveMoney = ((long)resultUser.stHAVEMONEY.stHAVEMONEY);
                ResourceContainer.Get<ADMyInfoTag>("MyInfo").gapMoney = ((long)resultUser.stHAVEMONEY.stGAPMONEY);
                
                
            }
            else
            {
                Debug.Log("current user serial is " + resultUser.stUSER.nSERIAL);
                var currentUser = ResourcePool.Find<GamePlayer>(p => p.roomSerial == resultUser.stUSER.nSERIAL);

                if(currentUser != null) // error handle for NullReferenceException
                {
                    currentUser.lbHave.SetAlpha(0f, false);

                    Debug.Log("result user " + resultUser.stUSER.szID + " gap money is " + (long)resultUser.stHAVEMONEY.stGAPMONEY
                        + " resultmoney is " + (long)resultUser.stRESULTMONEY);

                    // currentUser.Gap = resultUser.stHAVEMONEY.stGAPMONEY;
                    ResourceContainer.Get<ADGameMain>().SetPlayerGapMoney(currentUser, resultUser.stHAVEMONEY.stGAPMONEY);

                    currentUser.Have = ((long)resultUser.stHAVEMONEY.stHAVEMONEY);

                    currentUser.lbHave.color = new Color(0f, 0f, 0f, 0f);
                }



            }


        }
        // 

        // 4. destroy chips on board -> move to result win phase.....
        // ResourceContainer.Get<ADChipBettingManager>().ClearAllChips();

        #region
        // consider refactor this..?
        // var resultGoldTexts = ResourcePool.GetAll<ADResultGoldText>();
        //CoroutineChain.Start
        //        .Wait(_viewTime.Pop())
        //        .Wait(0.01f)
        //        // .Call(() => player.userPhoto.SetAlpha(1f))
        //        // .Call(() => player.userPhoto.AlphaTween(1f,0.01f))
        //        .Call(() =>
        //        {
        //            ResourceContainer.Get<ADResultPartInfoStoring>().TestSpineClear();
        //            var tempUsers = ResourcePool.GetAll<GamePlayer>();
        //            foreach (var user in tempUsers)
        //            {
        //                user.userPhoto.AlphaTween(1f, 0.01f);
        //            }
        //            foreach (var text in resultGoldTexts)
        //            {
        //                text.render.AlphaTween(0f, _viewTime.Pop());
        //            }
        //        });


        //yield return null;
        // throw new System.NotImplementedException();
        #endregion

        #region error handle for mid-game joined user
        if (ResourceContainer.Get<ADGameMain>().bJoinMiddleOfGame == true)
        {
            ResourceContainer.Get<ADChipBettingManager>().bDestroyChipImmediately = true;
            CoroutineChain.Start
                .Wait(1f)
                .Call(() => ResourceContainer.Get<ADChipBettingManager>().bDestroyChipImmediately = false);
        }
        #endregion

    }

    //public void SetMoney(UILabel label,long startMoney, long endMoney, bool bRolling, float time)
    //{
    //    label.RollingKMB(startMoney, endMoney, time);
    //}

    public IEnumerable<TimeContainer> GetAllTimeContainerList()
    {
        return _all;
        // throw new System.NotImplementedException();
    }
}
