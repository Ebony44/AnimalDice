using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ADBettingTimeCounter : ResourceItemBase
{

    public GameObject timerObject;
    public TextMeshPro timerText;
    public SpriteRenderer timerSprite;

    private int _number = 0;
    private float _count = 1000f;
    private Coroutine countRoutine;
    private float _Remainingtime = 0f;

    private int _prevNumber;
    // Update is called once per frame
    void Update()
    {
        if(_number < 1)
        {
            return;
        }
        _count -= Time.deltaTime;
        if(_count< 0f)
        {
            SetNumber(_number - 1);
        }
    }

    public int GetNumber()
    {
        return _number; 
    }

    public void SetNumber(int number)
    {
        // Debug.Log("Set Number " + number);
        // StopCoroutine()
        //if (_number == 3)
        //{
        //    _Remainingtime = 1f;
            
        //}
        //if(_number > 1)
        //{
        //    if()
        //    ResourceContainer.Get<ADChipBettingManager>().CheckAndSetEnableBoards();
        //}

        if(countRoutine == null)
        {
            StartCoroutine(SetNumRoutine(number));
        }
        else
        {
            StopCoroutine(countRoutine);
            StartCoroutine(SetNumRoutine(number));
        }
        // throw new NotImplementedException();
    }

    private IEnumerator SetNumRoutine(int number)
    {
        _prevNumber = _number;
        _number = number;
        if(_prevNumber == number)
        {
            yield break;
        }
        _count = 1f;

        timerText.text = _number.ToString();


        ResetTimerState();
        if(_number <= 3)
        {
            TimeContainer.ContainClear("ADTimerHandleTime");
            var betManager = ResourceContainer.Get<ADChipBettingManager>();

            // condition changed...
            // betManager.SetEnableBettingBoards(false);
            // betManager.bHasBoardEnabled = false;
            
            TimeContainer.Stack timerHandleTime = new TimeContainer.Stack(4, 1f, "ADTimerHandleTime"); // text's, sprite's scale, alpha
            timerText.color = Color.red;
            timerSprite.Scale(Vector3.one * 1.2f, timerHandleTime.Pop());
            timerSprite.AlphaTween(0f, timerHandleTime.Pop());
            timerText.AlphaTween(0f, 1f, true);
            timerText.Scale(Vector3.one * 1.2f, timerHandleTime.Pop());


            
            //if(_Remainingtime == 1f)
            //{
            //    // Sound.Instance.EffPlay("AD_3SecAlarm");
            //}
            //else
            //{
            //    if (countRoutine == null)
            //    {
            //        // countRoutine = StartCoroutine(PlayRemainingSoundRoutine());
            //    }
            //}
            



        }
        else
        {
            timerText.color = Color.white;
            timerSprite.transform.localScale = Vector3.one;
            timerText.transform.localScale = Vector3.one;
        }
        // if()

        yield return null;


        // throw new NotImplementedException();
    }
    private void ResetTimerState()
    {
        TimeContainer.ContainClear("ADTimerHandleTime");
        timerSprite.transform.localScale = Vector3.one;
        
        
        // timerSprite.AlphaTween(1f, 0.01f);
        // timerText.AlphaTween(1f, 0.01f, true);
        
        timerSprite.color = new Color(1f, 1f, 1f, 1f);
        timerText.SetAlpha(1f, true);

        timerText.transform.localScale = Vector3.one;

    }
    [TestMethod]
    public void TestSetTimer(int number)
    {
        _number = number;
        _count = 1f;
    }

    public void TurnOffWithAlpha(float alphaTime)
    {
        TimeContainer.Stack times = new TimeContainer.Stack(2, alphaTime, "NPCTimerBalloonAlpha");
        timerSprite.AlphaTween(0f, times.Pop());
        timerText.AlphaTween(0f, times.Pop());
        CoroutineChain.Start
            .Wait(2.5f)
            .Call(() =>
           {
               timerSprite.transform.localScale = Vector3.one;
               timerSprite.color = new Color(1f, 1f, 1f, 0f);
               timerText.transform.localScale = Vector3.one;
               timerText.color = new Color(1f, 1f, 1f, 0f);

           });

    }
    public void TurnOnWithAlpha()
    {
        TimeContainer.ContainClear("NPCTimerBalloonAlpha");
        timerSprite.color = Color.white;
        timerText.SetAlpha(1f);
    }


    //public IEnumerator PlayRemainingSoundRoutine()
    //{
    //    // Sound.Instance.EffPlay("AD_3SecAlarm");

    //    while (_Remainingtime < 1f)
    //    {
    //        _Remainingtime += Time.deltaTime;
    //        if(_Remainingtime > 1f)
    //        {
    //            _Remainingtime = 1f;
    //        }
    //    }
    //    yield return null;
    //    countRoutine = null;
    //}

    //public void ReceiveTimer(int number)
    //{

    //}
}
