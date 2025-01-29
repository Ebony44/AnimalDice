using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ADDishDiceManager : ResourceItemBase
{

    public SkeletonGraphic dishDiceSpine;
    public SkeletonGraphic dishDiceSpineCover;
    public float alphaTime = 0.5f;

    #region variable for new dish spine Fx
    public List<SkeletonDataAsset> dishSpineList;
    //"close_top   ",
    //"close_under ",
    //"open_top    ",
    //"open_under  ",

    //"1_top  ",
    //"1_under",
    //"2_top  ",
    //"2_under",
    //"3_top  ",
    //"3_under",
    public string[] dishTopAnimName = new string[] 
    {
        "close_top",
        "open_top",
        "1_top",
        "2_top",
        "3_top",
    };

    public string[] dishBotAnimName = new string[]
    {
        "close_under",
        "open_under",
        "1_under",
        "2_under",
        "3_under",
    };

    public enum eADDishRank
    {
        NOTHING = 0,
        FIVE_TIMES = 1,
        THREE_TIMES = 2,
        ONE_TIME = 3,
    }


    #endregion

    #region Shuffle Part
    public Coroutine PingPongWithXAxis(float rangeX, float length, TimeContainer t)
    {
        return StartCoroutine(PingPongRoutine(rangeX, length, t));
    }
    public IEnumerator PingPongRoutine(float rangeX, float length, TimeContainer time)
    {
        var st = Time.realtimeSinceStartup;
        // TimeContainer t1 = new TimeContainer("TestShake", time);
        var dishPos = ResourceContainer.Get<ResourceTag>("DishParent").transform;
        while (time.t < 1f)
        {
            yield return null;
            if (time.t > 1f)
            {
                time.t = 1f;
            }
            time.t += Time.deltaTime / time.time;
            var t = Mathf.PingPong(Time.realtimeSinceStartup - st, length) / length;
            //if(testAnimCurve != null)
            //{
            //    t = testAnimCurve.Evaluate(t1.t);
            //}

            // trans.localScale = Vector3.Lerp(Vector3.one * min, Vector3.one * max, t);

            dishPos.position = Vector3.Lerp((dishPos.position - Vector3.right * rangeX), (dishPos.position + Vector3.right * rangeX), t);

        }
        // TimeContainer.ContainClear("DISH_SHAKE");
        // dishPos.localPosition = Vector3.zero;
    }

    #endregion

    #region Result Part



    #endregion

    #region Test methods
    [TestMethod]
    public void TestDishDiceOpen()
    {
        // "open_top"
        // "open_under"
        
        TimeContainer t1 = new TimeContainer("ADspineAlphaTime", alphaTime);
        TimeContainer.ContainClear("ADspineAlphaTime");
        if (dishDiceSpine.Skeleton != null)
        {
            //winLoseSpine.skeletonDataAsset = null;
            // winLoseSpine.Clear();
            dishDiceSpine.AnimationState.SetEmptyAnimation(0, 0);
            dishDiceSpine.AlphaTween(1f, t1);
            // winLoseSpine.Skeleton.a = 1f;
        }
        if (dishDiceSpineCover.Skeleton != null)
        {
            //winLoseSpine.skeletonDataAsset = null;
            // winLoseSpine.Clear();
            dishDiceSpineCover.AnimationState.SetEmptyAnimation(0, 0);
            dishDiceSpineCover.AlphaTween(1f, t1);
            // winLoseSpine.Skeleton.a = 1f;
        }
        // winLoseSpine.AnimationState.SetAnimation(0, "win", false);
        dishDiceSpine.Play("open_under", false);
        dishDiceSpineCover.Play("open_top", false);
    }
    [TestMethod]
    public void TestDishDiceClose()
    {
        
        // "close_top"
        // "close_under"
        TimeContainer t1 = new TimeContainer("ADspineAlphaTime", alphaTime);
        TimeContainer.ContainClear("ADspineAlphaTime");
        if (dishDiceSpine.Skeleton != null)
        {
            //winLoseSpine.skeletonDataAsset = null;
            // winLoseSpine.Clear();
            dishDiceSpine.AnimationState.SetEmptyAnimation(0, 0);
            dishDiceSpine.AlphaTween(1f, t1);
            // winLoseSpine.Skeleton.a = 1f;
        }
        if (dishDiceSpineCover.Skeleton != null)
        {
            //winLoseSpine.skeletonDataAsset = null;
            // winLoseSpine.Clear();
            dishDiceSpineCover.AnimationState.SetEmptyAnimation(0, 0);
            dishDiceSpineCover.AlphaTween(1f, t1);
            // winLoseSpine.Skeleton.a = 1f;
        }
        // winLoseSpine.AnimationState.SetAnimation(0, "win", false);

        // dishDiceSpine.Play("close_under", false);
        // dishDiceSpineCover.Play("close_top", false);


        // dishDiceSpine.AnimationState.Apply(mySkeleton);
        // Time.timeScale = 0;
        // var tempAnim = dishDiceSpine.AnimationState.SetAnimation(0, "close_under", false);
        tempUnderDish = dishDiceSpine.AnimationState.SetAnimation(0, "close_under", false);
        tempTopDish = dishDiceSpineCover.AnimationState.SetAnimation(1, "close_top", false);
        tempUnderDish.timeScale = 0.2f;
        tempTopDish.timeScale = 0.2f;

        // var tempAnim2 = dishDiceSpine.Skeleton;
        // tempAnim2.time = mySliderFloat * 10f;
        // var tempanim3 = dishDiceSpine.AnimationState.GetCurrent(0).TrackTime;



    }
    [TestMethod]
    public void TestDishClear()
    {
        dishDiceSpine.AnimationState.SetEmptyAnimation(0, 0);
        dishDiceSpineCover.AnimationState.SetEmptyAnimation(1, 0);

        
        tempUnderDish = null;
        tempTopDish = null;
        tempAnim3 = null;
        tempAnim4 = null;

        diceImage.SetAlpha(0f, true);

        // dishDiceSpine.Clear();
        // dishDiceSpineCover.Clear();


    }

    [TestMethod(false)]
    public void TestStartAnimSliderRoutine()
    {
        bIsRoutinePlaying = true;
        StartCoroutine(TestAnimSliderRoutine());
    }
    public IEnumerator TestAnimSliderRoutine()
    {
        tempUnderDish = dishDiceSpine.AnimationState.SetAnimation(0, "close_under", false);
        tempTopDish = dishDiceSpineCover.AnimationState.SetAnimation(1, "close_top", false);
        tempUnderDish.TimeScale = 0.01f;
        tempTopDish.TimeScale = 0.01f;
        yield return new WaitForSeconds(0.01f);
        tempUnderDish.TimeScale = 0f;
        tempTopDish.TimeScale = 0f;
        // previousFloat = mySliderFloat;

        while (bIsRoutinePlaying)
        {
            if (previousFloat != mySliderFloat)
            {
                //if(tempAnim1 == null || tempAnim2 == null)
                //{
                //    continue;
                //}
                tempUnderDish.trackTime = mySliderFloat * sliderModifier;
                tempTopDish.trackTime = mySliderFloat * sliderModifier;


                // arrive at 0.28f
                var tempMoveFloat = mySliderFloat / 0.28f;
                dicepack.transform.position = Vector3.LerpUnclamped(
                    startPos.transform.position,
                    arrivePos.transform.position,
                    moveCurve.Evaluate(tempMoveFloat));

                var tempAlphaFloat = mySliderFloat / 0.13f;

                diceImage.SetAlpha(Mathf.Lerp(0f, 1f, tempAlphaFloat), true);

                var tempSizeFloat = mySliderFloat / 0.13f;

                dicepack.transform.localScale = Vector3.LerpUnclamped(
                    Vector3.one * 0.5f,
                    Vector3.one,
                    moveCurve.Evaluate(tempSizeFloat));


                if (mySliderFloat > 0.4f)
                {
                    diceImage.SetAlpha(0f, true);
                }
            }
            previousFloat = mySliderFloat;
            yield return null;
        }

    }

    [TestMethod(false)]
    public void TestStartAnimSliderRoutine2(int animIndex)
    {
        bIsRoutinePlaying = true;
        StartCoroutine(TestAnimSliderRoutine2(animIndex));
        
    }
    public IEnumerator TestAnimSliderRoutine2(int animIndex)
    {
        if(animIndex == 0)
        {
            ChangeCurrentSpine(0);
            tempUnderDish = dishDiceSpine.AnimationState.SetAnimation(0, "open_under", false);
            tempTopDish = dishDiceSpineCover.AnimationState.SetAnimation(1, "open_top", false);
        }
        else if(animIndex == 1)
        {
            ChangeCurrentSpine(1);
            tempUnderDish = dishDiceSpine.AnimationState.SetAnimation(0, "1_under", false);
            tempTopDish = dishDiceSpineCover.AnimationState.SetAnimation(1, "1_top", false);
        }
        else if (animIndex == 2)
        {
            ChangeCurrentSpine(1);
            tempUnderDish = dishDiceSpine.AnimationState.SetAnimation(0, "2_under", false);
            tempTopDish = dishDiceSpineCover.AnimationState.SetAnimation(1, "2_top", false);
            // 0.21
        }
        else if (animIndex == 3)
        {
            ChangeCurrentSpine(1);
            tempUnderDish = dishDiceSpine.AnimationState.SetAnimation(0, "3_under", false);
            tempTopDish = dishDiceSpineCover.AnimationState.SetAnimation(1, "3_top", false);
            // 
        }
        else if (animIndex == 10)
        {
            ChangeCurrentSpine(2);
            tempUnderDish = dishDiceSpine.AnimationState.SetAnimation(0, "open_under", false);
            tempTopDish = dishDiceSpineCover.AnimationState.SetAnimation(1, "open_top", false);
        }
        else if (animIndex == 11)
        {
            ChangeCurrentSpine(3);
            tempUnderDish = dishDiceSpine.AnimationState.SetAnimation(0, "1_under", false);
            tempTopDish = dishDiceSpineCover.AnimationState.SetAnimation(1, "1_top", false);
        }
        else if (animIndex == 12)
        {
            ChangeCurrentSpine(3);
            tempUnderDish = dishDiceSpine.AnimationState.SetAnimation(0, "2_under", false);
            tempTopDish = dishDiceSpineCover.AnimationState.SetAnimation(1, "2_top", false);
        }
        else if (animIndex == 13)
        {
            ChangeCurrentSpine(3);
            tempUnderDish = dishDiceSpine.AnimationState.SetAnimation(0, "3_under", false);
            tempTopDish = dishDiceSpineCover.AnimationState.SetAnimation(1, "3_top", false);
        }

        tempUnderDish.TimeScale = 0.01f;
        tempTopDish.TimeScale = 0.01f;
        yield return new WaitForSeconds(0.01f);
        tempUnderDish.TimeScale = 0f;
        tempTopDish.TimeScale = 0f;
        // previousFloat = mySliderFloat;

        dishDiceSpine.SetAlpha(0f);
        dishDiceSpineCover.SetAlpha(0f);
        float sliderModifier2 = 4f;
        bIsAlphaOnForOpening = false;
        bIsAlphaOffForOpening = false;
        diceImage.SetAlpha(0f, true);
        dicepack.transform.position = arrivePos.transform.position;
        
        while (bIsRoutinePlaying)
        {
            if (previousFloat != mySliderFloat)
            {
                //if(tempAnim1 == null || tempAnim2 == null)
                //{
                //    continue;
                //}
                tempUnderDish.trackTime = mySliderFloat * sliderModifier2;
                tempTopDish.trackTime = mySliderFloat * sliderModifier2;

                if(mySliderFloat <= 0.05)
                {

                    // dishDiceSpine.color  = new Color(1f,1f,1f,0f);
                    // dishDiceSpineCover.color = new Color(1f, 1f, 1f, 0f);

                    dishDiceSpine.SetAlpha(0f);
                    dishDiceSpineCover.SetAlpha(0f);

                }
                else
                {
                    dishDiceSpine.color = new Color(1f, 1f, 1f, 1f);
                    dishDiceSpineCover.color  = new Color(1f,1f,1f,1f);
                }


                if (mySliderFloat > DICE_SHOW_UP_TIME)
                {
                    if (bIsAlphaOnForOpening == false)
                    {
                        diceImage.SetAlpha(1f, true);
                        bIsAlphaOnForOpening = true;
                    }

                }
                if (mySliderFloat > DICE_DISAPPEAR_TIME) //time.t > 0.67f)
                {

                    if (bIsAlphaOffForOpening == false)
                    {
                        
                        diceImage.AlphaTween(0f, 0.64f, true);
                        bIsAlphaOffForOpening = true;
                    }
                }

                //if(mySliderFloat > 0.21f 
                //    && animIndex == 1
                //    && bSound2Played == false)
                //{
                //    Sound.Instance.EffPlay("AD_DishCoverOpen2");
                //    bSound2Played = true;
                //}
                //if (mySliderFloat > 0.16f
                //    && animIndex == 3
                //    && bSound3Played == false)
                //{
                //    Sound.Instance.EffPlay("AD_DishCoverOpen3");
                //    bSound3Played = true;
                //}

            }
            previousFloat = mySliderFloat;
            yield return null;
        }
    }



    [TestMethod(false)]
    public void ChangeCurrentSpine(int spineIndex)
    {
        dishDiceSpine.skeletonDataAsset = dishSpineList[spineIndex];
        dishDiceSpineCover.skeletonDataAsset = dishSpineList[spineIndex];

        dishDiceSpine.Initialize(true);
        dishDiceSpineCover.Initialize(true);
        // dishDiceSpine.AnimationState.ClearTrack(0);
        // dishDiceSpineCover.AnimationState.ClearTrack(0);
    }

    
    [TestMethod(false)]
    public void PlaySpineWithIndex(int spineIndex)
    {
        dishDiceSpine.AnimationState.SetEmptyAnimation(0, 0);
        dishDiceSpineCover.AnimationState.SetEmptyAnimation(0, 0);

        dishDiceSpine.Play(dishBotAnimName[spineIndex], false);
        dishDiceSpineCover.Play(dishTopAnimName[spineIndex], false);

        Debug.Log("top name is " + dishTopAnimName[spineIndex]
            + " bot name is " + dishBotAnimName[spineIndex]);

    }


    #endregion


    public void StartShuffleDish(TimeContainer time)
    {
        // TimeContainer.ContainClear("DISH_SUFFLE");
        StartCoroutine(DishShuffleRoutine(time));
    }

    public IEnumerator DishShuffleRoutine(TimeContainer time)
    {
        var manager = ResourceContainer.Get<ADDishDiceManager>();

        manager.ChangeCurrentSpine(0);

        // tempAnim1 = dishDiceSpine.AnimationState.SetAnimation(0, "close_under", false);
        // tempAnim2 = dishDiceSpineCover.AnimationState.SetAnimation(1, "close_top", false);

        TimeContainer.ContainClear("DISH_DICE_CLEAR");
        // dishDiceSpine.AnimationState.SetEmptyAnimation(0, 0);
        // dishDiceSpineCover.AnimationState.SetEmptyAnimation(0, 0);

        dishDiceSpine.AnimationState.SetAnimation(0,        manager.dishBotAnimName[0], false);
        dishDiceSpineCover.AnimationState.SetAnimation(1,   manager.dishTopAnimName[0], false);
        yield return null;
        // dishDiceSpine.Skeleton.a = 1f;
        // dishDiceSpineCover.Skeleton.a = 1f;
        dishDiceSpine.AlphaTween(1f, 0.01f);
        dishDiceSpineCover.AlphaTween(1f, 0.01f);

        // tempAnim1.TimeScale = 0f;
        // tempAnim2.TimeScale = 0f;

        // previousFloat = mySliderFloat;

        bool bTempShuffleSoundPlayed = false;
        Sound.Instance.EffPlay("AD_DishAppear");
        while (time.t < 1f)
        {
            time.t += Time.deltaTime / time.time;

            if (time.t > 1f) time.t = 1f;

            // tempAnim1.trackTime = time.t * sliderModifier;
            // tempAnim2.trackTime = time.t * sliderModifier;

            var tempMoveFloat = time.t / 0.28f;
            dicepack.transform.position = Vector3.LerpUnclamped(
                startPos.transform.position,
                arrivePos.transform.position,
                moveCurve.Evaluate(tempMoveFloat));

            var tempAlphaFloat = time.t / 0.13f;

            diceImage.SetAlpha(Mathf.Lerp(0f, 1f, tempAlphaFloat), true);

            var tempSizeFloat = time.t / 0.13f;

            dicepack.transform.localScale = Vector3.LerpUnclamped(
                Vector3.one * 0.5f,
                Vector3.one,
                moveCurve.Evaluate(tempSizeFloat));


            if (time.t > 0.4f)
            {
                diceImage.SetAlpha(0f, true);
            }
            // dish sound should start at the  50% of spine FX progress
            if(time.t >= 0.5f && bTempShuffleSoundPlayed == false)
            {
                bTempShuffleSoundPlayed = true;
                Sound.Instance.EffPlay("AD_DishShuffle");
            }

            
            yield return null;
        }
    }

    [TestMethod(false)]
    public void TestStartResultDice2(int animIndex)
    {
        // TimeContainer t1 = new TimeContainer("a", 2.5f);
        TimeContainer t1 = new TimeContainer("a", 4f);
        StartCoroutine(ResultDiceRoutine(t1, (eADDishRank)animIndex));
    }

    public void StartResultDice(TimeContainer time, eADDishRank dishRank)
    {
        StartCoroutine(ResultDiceRoutine(time, dishRank));
    }

    public IEnumerator ResultDiceRoutine(TimeContainer time, eADDishRank dishRank)
    {

        var manager = ResourceContainer.Get<ADDishDiceManager>();

        float underDishDuration = 0f;
        float topDishDuration = 0f;


        TimeContainer.ContainClear("DISH_DICE_CLEAR");
        // dishDiceSpine.AnimationState.SetEmptyAnimation(0, 0);
        // dishDiceSpineCover.AnimationState.SetEmptyAnimation(0, 0);

        // 1. check which dice effect will show
        if (dishRank == eADDishRank.NOTHING)
        {
            manager.ChangeCurrentSpine(0);
            
            dishDiceSpine.AnimationState.SetAnimation(0, manager.dishBotAnimName[1], false);
            dishDiceSpineCover.AnimationState.SetAnimation(1, manager.dishTopAnimName[1], false);
            
        }
        else
        {
            manager.ChangeCurrentSpine(1);
            dishDiceSpine.SetAlpha(0f);
            dishDiceSpineCover.SetAlpha(0f);
            if (dishRank == eADDishRank.FIVE_TIMES)
            {
                dishDiceSpine.AnimationState.SetAnimation(0, manager.dishBotAnimName[4], false);
                dishDiceSpineCover.AnimationState.SetAnimation(1, manager.dishTopAnimName[4], false);
                
            }
            else if(dishRank == eADDishRank.THREE_TIMES)
            {
                dishDiceSpine.AnimationState.SetAnimation(0, manager.dishBotAnimName[3], false);
                dishDiceSpineCover.AnimationState.SetAnimation(1, manager.dishTopAnimName[3], false);
                
            }
            else if (dishRank == eADDishRank.ONE_TIME)
            {
                dishDiceSpine.AnimationState.SetAnimation(0, manager.dishBotAnimName[2], false);
                dishDiceSpineCover.AnimationState.SetAnimation(1, manager.dishTopAnimName[2], false);
            }
            
        }
        yield return new WaitForSeconds(0.02f);
        
        if (dishRank == eADDishRank.NOTHING)
        {
            dishDiceSpine.SetAlpha(1f);
            dishDiceSpineCover.SetAlpha(1f);
        }
        else
        {
            // TimeContainer.Stack t1 = new TimeContainer.Stack(2, 0.1f, "DISH_DICE_CLEAR");

            dishDiceSpine.SetAlpha(1f);
            dishDiceSpineCover.SetAlpha(1f);

            // dishDiceSpine.AlphaTween(1f, 0.55f);
            // dishDiceSpineCover.AlphaTween(1f, 0.55f);
        }


        diceImage.SetAlpha(0f, true);
        bIsAlphaOnForOpening = false;
        bIsAlphaOffForOpening = false;
        dicepack.transform.position = arrivePos.transform.position;

        #region temporal codes delete it after server time adjusted
        // 2.5f -> 1f 
        // 0.5 sec -> 1.25f
        // tempAnim3.TimeScale = 1f * 2.5f;
        // tempAnim4.TimeScale = 1f * 2.5f;
        #endregion


        //yield return new WaitForSeconds(0.82f);
        //diceImage.SetAlpha(1f, true);

        Debug.Log("time is " + time.time);
        bool bTempOpenSoundPlayed = false;
        bool bSound2Played = false;
        bool bSound3Played = false;

        tempUnderDish = dishDiceSpine.AnimationState.GetCurrent(0);
        tempTopDish = dishDiceSpineCover.AnimationState.GetCurrent(1);
        // tempAnim1.trackTime = mySliderFloat * sliderModifier2;

        Sound.Instance.EffPlay("AD_DishAppear");
        while ( tempUnderDish.IsComplete == false)
        {
            //Debug.Log("is tempanim1 complete " + tempAnim1.IsComplete
            //    + " track time " + tempAnim1.trackTime
            //    + " mod time " + tempAnim1.animationStart / tempAnim1.trackTime);
            time.t += Time.deltaTime / time.time;

            if (time.t > 1f) time.t = 1f;

            //if (time.t > 0.4f && tempBool1 == false)
            //{
            //    Debug.Log("asdf");
            //    tempBool1 = true;
            //}

            if (tempUnderDish.AnimationTime / tempUnderDish.animationEnd > DICE_SHOW_UP_TIME)//  time.t > (0.32f))
            {
                if (bIsAlphaOnForOpening == false)
                {
                    diceImage.SetAlpha(1f, true);
                    bIsAlphaOnForOpening = true;
                }
            }

            if (tempUnderDish.AnimationTime / tempUnderDish.animationEnd > DICE_DISAPPEAR_TIME) //time.t > 0.67f)
            {

                if (bIsAlphaOffForOpening == false)
                {
                    // diceImage.SetAlpha(1f, true);
                    diceImage.AlphaTween(0f, 0.64f, true);
                    bIsAlphaOffForOpening = true;
                }
            }

            if (tempUnderDish.AnimationTime / tempUnderDish.animationEnd > SOUND_PLAY_TIMING_1) //time.t > 0.67f)
            {
                
                if (bTempOpenSoundPlayed == false
                    && dishRank == eADDishRank.NOTHING)
                {
                    bTempOpenSoundPlayed = true;
                    // Sound.Instance.EffPlay("AD_DishCoverOpen");
                    Sound.Instance.EffPlayWithForce("AD_DishCoverOpen");
                }
            }
            


            if (tempUnderDish.AnimationTime / tempUnderDish.animationEnd > soundPlayTiming2
                    && (dishRank == eADDishRank.THREE_TIMES || dishRank == eADDishRank.ONE_TIME)
                    && bSound2Played == false)
            {
                // Sound.Instance.EffPlay("AD_DishCoverOpen2"); // effect sound 1
                Sound.Instance.EffPlayWithForce("AD_DishCoverOpen2"); // effect sound 1
                bSound2Played = true;
            }
            if (tempUnderDish.AnimationTime / tempUnderDish.animationEnd > soundPlayTiming3
                && dishRank == eADDishRank.FIVE_TIMES
                && bSound3Played == false)
            {
                Sound.Instance.EffPlayWithForce("AD_DishCoverOpen3");
                bSound3Played = true;
            }
            yield return null;
        }


        #region obsolete
        // 0.82f
        //while (time.t < 1f)
        //{

        //    time.t += Time.deltaTime / time.time;

        //    if (time.t > 1f) time.t = 1f;

        //    if (time.t > (0.32f))
        //    {
        //        if (bIsAlphaChangedForOpening == false)
        //        {
        //            diceImage.SetAlpha(1f, true);
        //            bIsAlphaChangedForOpening = true;
        //        }


        //    }
        //    else
        //    {
        //        // diceImage.SetAlpha(0f, true);
        //    }
        //    // if(time.t > 0.59f) // previous sound play time...
        //    if (time.t > 0.67f)
        //    {
        //        if (bTempOpenSoundPlayed == false
        //            && dishRank == eADDishRank.NOTHING)
        //        {
        //            bTempOpenSoundPlayed = true;
        //            Sound.Instance.EffPlay("AD_DishCoverOpen");
        //        }
        //    }


        //    if (time.t > soundPlayTiming2
        //            && dishRank == eADDishRank.THREE_TIMES
        //            && bSound2Played == false)
        //    {
        //        Sound.Instance.EffPlay("AD_DishCoverOpen2"); // effect sound 1
        //        bSound2Played = true;
        //    }
        //    if (time.t > soundPlayTiming3
        //        && dishRank == eADDishRank.FIVE_TIMES
        //        && bSound3Played == false)
        //    {
        //        Sound.Instance.EffPlay("AD_DishCoverOpen3");
        //        bSound3Played = true;
        //    }
        //    yield return null;
        //}
        #endregion
        // tempAnim1 = dishDiceSpine.AnimationState.SetAnimation(0, "3_under", false);
        // tempAnim2 = dishDiceSpineCover.AnimationState.SetAnimation(1, "3_top", false);
        // tempAnim1.trackTime = mySliderFloat * sliderModifier2;

        yield return null;
    }
    public void ClearDiceAnimation(float time)
    {
        StartCoroutine(ClearDiceAnimationRoutine(time));
        //dishDiceSpine.AnimationState.SetEmptyAnimation(0, 0);
        //dishDiceSpineCover.AnimationState.SetEmptyAnimation(1, 0);


        //tempAnim1 = null;
        //tempAnim2 = null;
        //tempAnim3 = null;
        //tempAnim4 = null;
        //diceImage.SetAlpha(0f, true);
    }
    public IEnumerator ClearDiceAnimationRoutine(float time)
    {
        
        TimeContainer.Stack _clearTime = new TimeContainer.Stack(3, time, "DISH_DICE_CLEAR");
        dishDiceSpine.AlphaTween(0f, _clearTime.Pop());
        dishDiceSpineCover.AlphaTween(0f, _clearTime.Pop());
        diceImage.AlphaTween(0f, time, true);

        yield return new WaitForSeconds(0.5f);

        Debug.Log("alpha of spine 1 is " + dishDiceSpine.Skeleton.a +
            " alpha of spine 2 is " + dishDiceSpineCover.Skeleton.a +
            " alpha of image is " + diceImage.color.a);

        yield return new WaitForSeconds(time);
        dishDiceSpine.AnimationState.SetEmptyAnimation(0, 0);
        dishDiceSpineCover.AnimationState.SetEmptyAnimation(1, 0);



        // tempAnim1 = null;
        // tempAnim2 = null;
        // tempAnim3 = null;
        // tempAnim4 = null;
        // tempAnim1.TimeScale = 1f;
        // tempAnim2.TimeScale = 1f;

        diceImage.SetAlpha(0f, true);
    }

    [Range(0.0f, 1f)]
    public float mySliderFloat;

    public bool bIsRoutinePlaying = false;
    public float previousFloat = 0;

    public const float sliderModifier = 2.26f;

    public const float closeDishSpineModifier = 2.26f;
    public const float openDishSpineModifier = 2.5f;

    // public float soundPlayTiming2 = 0.21f;
    // public float soundPlayTiming3 = 0.16f;

    public const float SOUND_PLAY_TIMING_1 = 0.5f;
    public float soundPlayTiming2 = 0.2f;
    public float soundPlayTiming3 = 0.2f;

    public float tempModFloat1 = 0.25f;
    public float tempModFloat2 = 0.5f;

    public const float DICE_SHOW_UP_TIME = 0.16f;
    public const float DICE_DISAPPEAR_TIME = 0.84f;
    


    public bool bIsAlphaOnForOpening = false;
    public bool bIsAlphaOffForOpening = false;


    // public bool bSound2Played = false;
    // public bool bSound3Played = false;


    Skeleton mySkeleton;
    TrackEntry tempUnderDish;
    TrackEntry tempTopDish;
    TrackEntry tempAnim3;
    TrackEntry tempAnim4;

    public GameObject dicepack;
    public Image diceImage;

    public Transform startPos;
    public Transform arrivePos;
    public AnimationCurve moveCurve;

}
