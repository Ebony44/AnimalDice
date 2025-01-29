using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Wooriline;


public class ADDishShuffle : PacketHandler
{
    // public float rangeX;
    // public float lengthIntensity;
    public TimeContainer shakeTime;

    public override int GetNumber()
    {
        return (int)ANIMALDICE_PK.R_09_SHUFFLE;
    }

    public override void Func()
    {
        var rec = new R_09_SHUFFLE(SubGameSocket.m_bytebuffer);
        Debug.Log("[R_09_SHUFFLE] AniTime : " + rec.nANITIME/1000f + " , current game index : " + (int)rec.stGAME_IDX);
        ResourceContainer.Get<ADChipBettingManager>().currentGameIndex = (int)rec.stGAME_IDX; // store from here, use it from Req_betting
        var dishShuffle = new DishShuffle(rec.nANITIME/1000f, rangeX: 10f, length: 0.2f);
        ActionPlayer.Play(dishShuffle);

        // throw new System.NotImplementedException();
    }

    [TestMethod]
    public void TestShuffleDish(float receivedAniTime = 1f, float rangeX = 20f)
    {
        var shuffle = new DishShuffle(receivedAniTime, rangeX: rangeX, length: 0.2f); // TODO: fix these magic number
        ActionPlayer.Play(shuffle);



    }

    #region obsolete
    //[TestMethod]
    //public void MoveDishToStart()
    //{
    //    var endPos = ResourceContainer.Get<GameObject>("DishAppearPosition").transform.position;
    //    ResourceContainer.Get<RectTransform>("DishParent").DOMove(endPos, 1f);
    //}
    //[TestMethod]
    //public void MoveDishCover(bool bPutCoverOn, float spendingTime)
    //{
    //    // var endPos = ResourceContainer.Get<GameObject>("DishAppearPosition").transform.position;
    //    TimeContainer t1 = new TimeContainer("DishCoverRelatedFX", spendingTime);
    //    //if(bPutCoverOn)
    //    //{
    //    //    ResourceContainer.Get<Image>("DishCoverImage").DOComplete();
    //    //    ResourceContainer.Get<Image>("DishCoverImage").SetAlpha(1f);
    //    //    // ResourceContainer.Get<Image>("DishCoverImage").DOFade(1, 0f);
    //    //    ResourceContainer.Get<RectTransform>("DishCoverTransform").DOLocalMove(new Vector3(0, 0, 0), spendingTime);
    //    //}
    //    //else
    //    //{
    //    //    // ResourceContainer.Get<Image>("DishCoverImage").SetAlpha(1f);
    //    //    ResourceContainer.Get<RectTransform>("DishCoverTransform").DOLocalMove(new Vector3(240, 130, 0), spendingTime);
    //    //    ResourceContainer.Get<Image>("DishCoverImage").DOFade(0, spendingTime);
    //    //}

    //    if(bPutCoverOn)
    //    {
    //        ResourceContainer.Get<ResourceTag>("DishCoverTag").MoveLocal(new Vector3(0, 0, 0), 1f);
    //    }
    //    else
    //    {

    //    ResourceContainer.Get<ResourceTag>("DishCoverTag").MoveLocal(new Vector3(240, 130, 0), 1f);
    //    }

    //}

    #endregion

    public class DishShuffle : IAction
    {
        public string Log
        {
            get
            {
                var log = "[Divide] aniTime : " + _aniTime;
                
                return log;
            }
        }

        public string ID => "DishShuffle";

        public List<string> CancelID => new List<string> { "DishShuffle" };

        // public int nSTEP;
        // public stINT64 stGAME_IDX;
        // public int nANITIME;
        #region variables and constructor
        TimeContainer.Stack _dishAppear;
        TimeContainer.Stack _dishCoverOn;
        TimeContainer.Stack _shakeTime;
        // TimeContainer.Stack _dishCoverOff;
        TimeContainer.Stack _dishDisappear;
        TimeContainer.StackSum _all;
        public float _aniTime;

        #region shaking related
        public float _rangeX;
        public float _length;
        
        #endregion

        public DishShuffle(float aniTime, float rangeX, float length)
        {
            _aniTime = aniTime;
            _dishAppear = new TimeContainer.Stack(2, aniTime /  5, "DISH_APPEAR"); // scale and transform
            _dishCoverOn = new TimeContainer.Stack(1, aniTime / 5, "DISH_COVER_ON");
            _shakeTime = new TimeContainer.Stack(2, aniTime /   5, "DISH_SHAKE");
            
            _dishDisappear = new TimeContainer.Stack(2, aniTime /  5, "DISH_DISAPPEAR"); // scale and transform

            _rangeX = rangeX;
            _length = length;

            _all = new TimeContainer.StackSum(_dishAppear, _dishCoverOn, _shakeTime, _dishDisappear);
        }
        #endregion

        public IEnumerator ActionRoutine()
        {
            yield return null;
            TimeContainer.ContainClear("DISH_SUFFLE");
            TimeContainer t1 = new TimeContainer("DISH_SUFFLE", ADDishDiceManager.closeDishSpineModifier);
            ResourceContainer.Get<ADDishDiceManager>().StartShuffleDish(t1);

            

            yield return new WaitForSeconds(ADDishDiceManager.closeDishSpineModifier);
            // after dish shuffle complete...
            ResourceContainer.Get<ADChipBettingManager>().PlayBettingOpening();
            

            var remainingTime = _aniTime - ADDishDiceManager.closeDishSpineModifier - 1.5f;
            if(remainingTime > 0)
            {
                Debug.Log("[R_09_SHUFFLE] remaining time after betting spine FX : " + remainingTime );
                yield return new WaitForSeconds(remainingTime);
            }
            else
            {
                Debug.Log("[R_09_SHUFFLE] remaining time after betting spine FX : " + (_aniTime - ADDishDiceManager.closeDishSpineModifier));
                yield return new WaitForSeconds(_aniTime - ADDishDiceManager.closeDishSpineModifier);
            }

            

        }

        #region obsolete
        //public IEnumerator ActionRoutine()
        //{
        //    // Step by Step
        //    // shuffle dice
        //    // 0. define dice's transform, rotation
        //    // "ADDishDiceSymbol"
        //    // SetDicePack(); // -> remove when packet connected, these will be decided by server


        //    // 1. appear from top outside(from reduced scale) with cover opened
        //    var appearPos = ResourceContainer.Get<GameObject>("DishAppearPosition").transform.position;
        //    var stayPos = ResourceContainer.Get<GameObject>("DishStayPosition").transform.position;
        //    var coverOffPos = ResourceContainer.Get<ResourceTag>("DishCoverOffPosition").transform.localPosition;

        //    ResourceContainer.Get<ResourceTag>("DishParent").GetComponent<Image>().SetAlpha(1f, true);

        //    ResourceContainer.Get<ResourceTag>("DishParent").transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        //    ResourceContainer.Get<ResourceTag>("DishCoverTag").transform.localPosition = coverOffPos;

        //    ResourceContainer.Get<ResourceTag>("DishParent").Scale(new Vector3(1f, 1f, 1f), _dishAppear.Pop());
        //    yield return ResourceContainer.Get<ResourceTag>("DishParent").Move(appearPos, stayPos, _dishAppear.Pop());

        //    // 2. put cover on the dish with the cover

        //    ResourceContainer.Get<Image>("DishCoverImage").SetAlpha(1f);
        //    yield return ResourceContainer.Get<ResourceTag>("DishCoverTag").MoveLocal(new Vector3(0, 0, 0), _dishCoverOn.Pop());

        //    // 3. shake it along x axis
        //    var tempShakeTime = _shakeTime.Pop();
        //    _length = tempShakeTime.time / 4f;
        //    yield return ResourceContainer.Get<ADDishDiceManager>("DishDiceManager").PingPongWithXAxis(_rangeX,_length,_shakeTime.Pop());

        //    yield return new WaitForSeconds(0.2f);


        //    // 4. Pull off dish cover -> it's result part.
        //    // ResourceContainer.Get<Image>("DishCoverImage").AlphaTween(0, _dishCoverOff.Pop());
        //    // yield return ResourceContainer.Get<ResourceTag>("DishCoverTag").MoveLocal(coverOffPos, _dishCoverOff.Pop());

        //    // 5. disappear to top outside
        //    // var tempDisappearTime = _dishDisappear.Pop();

        //    // ResourceContainer.Get<Image>("DishCoverImage").AlphaTween(0, _dishDisappear.Pop());
        //    ResourceContainer.Get<ResourceTag>("DishParent").Scale(new Vector3(0.95f, 0.95f, 0.95f), _dishDisappear.Pop());
        //    yield return ResourceContainer.Get<ResourceTag>("DishParent").Move(stayPos, appearPos, _dishDisappear.Pop());


        //    // throw new System.NotImplementedException();
        //}

        #endregion


        public IEnumerable<TimeContainer> GetAllTimeContainerList()
        {
            return _all;
        }

        public IEnumerator PingPongRoutine(float rangeX, float length, TimeContainer t1)
        {
            var st = Time.realtimeSinceStartup;
            // TimeContainer t1 = new TimeContainer("TestShake", time);
            var dishPos = ResourceContainer.Get<ResourceTag>("DishParent").transform;
            while (t1.t < 1f)
            {
                yield return null;
                if (t1.t > 1f)
                {
                    t1.t = 1f;
                }
                t1.t += Time.deltaTime / t1.time;
                var t = Mathf.PingPong(Time.realtimeSinceStartup - st, length) / length;
                //if(testAnimCurve != null)
                //{
                //    t = testAnimCurve.Evaluate(t1.t);
                //}

                // trans.localScale = Vector3.Lerp(Vector3.one * min, Vector3.one * max, t);

                dishPos.position = Vector3.Lerp((dishPos.position - Vector3.right * rangeX), (dishPos.position + Vector3.right * rangeX), t);

            }
            TimeContainer.ContainClear("DISH_SHAKE");
            dishPos.localPosition = Vector3.zero;
        }


    }

    // test only
    #region for test purpose
    //[TestMethod]
    //public void SetDicePack()
    //{
    //    var tempSprites = ResourceContainer.Get<ADDishDiceSymbol>("DiceSymbols").GetRandomSymbols();

    //    // 1. set which dice pack active 
    //    var tempDicePackIndex = Random.Range(1, 2);
    //    var dicePack = ResourceContainer.Get<GameObject>("DicePack" + tempDicePackIndex.ToString());
    //    dicePack.SetActive(true);
    //    ResourceContainer.Get<GameObject>("DicePack" + (tempDicePackIndex == 1 ? 2 : 1).ToString()).SetActive(false);
    //    var diceCount = ResourceContainer.Get<ADDishDiceSymbol>("DiceSymbols").diceCount;

    //    // 2. change sprites of those dice // -> move or copy paste into result packet...
    //    var modifier = (tempDicePackIndex - 1) * diceCount;
    //    for (int i = 0; i < diceCount; ++i)
    //    {
    //        ResourceContainer.Get<Image>("DiceSymbol" + ((modifier * i) + 1).ToString()).sprite = tempSprites[i];
    //    }

    //    // 3. change rotation
    //    var tempRotValue = Random.Range(-20f, 20f);
    //    dicePack.transform.eulerAngles = new Vector3(0, 0, tempRotValue);
    //    // dicePack.transform.rotation = new Quaternion(0, 0, tempRotValue, 0);


    //}

    //private Coroutine shakeRoutine;
    //public AnimationCurve testAnimCurve;
    //[TestMethod]
    //public void PingPoingWithXScale(float rangeX, float length, float time)
    //{
    //    if(shakeRoutine == null)
    //    {

    //        shakeRoutine = StartCoroutine(PingPongRoutine(rangeX, length, time));
    //    }
    //    else
    //    {
    //        TimeContainer.ContainClear("TestShake");
    //        StopCoroutine(shakeRoutine);
    //        shakeRoutine = StartCoroutine(PingPongRoutine(rangeX, length, time));
    //    }
    //    // ResourceContainer.Get<ResourceTag>("DishParent").Move
    //}
    //[TestMethod]
    //public void StopShake()
    //{
    //    TimeContainer.ContainClear("TestShake");
    //    if(shakeRoutine != null)
    //    {

    //        StopCoroutine(shakeRoutine);
    //    }
    //}
    //public IEnumerator PingPongRoutine(float rangeX, float length, float time)
    //{
    //    var st = Time.realtimeSinceStartup;
    //    TimeContainer t1 = new TimeContainer("TestShake", time);
    //    var dishPos = ResourceContainer.Get<ResourceTag>("DishParent").transform;
    //    while (t1.t < 1f)
    //    {
    //        yield return null;
    //        if(t1.t > 1f)
    //        {
    //            t1.t = 1f;
    //        }
    //        t1.t += Time.deltaTime / t1.time;
    //        var t = Mathf.PingPong(Time.realtimeSinceStartup - st, length) / length;
    //        //if(testAnimCurve != null)
    //        //{
    //        //    t = testAnimCurve.Evaluate(t1.t);
    //        //}

    //        // trans.localScale = Vector3.Lerp(Vector3.one * min, Vector3.one * max, t);

    //        dishPos.position = Vector3.Lerp((dishPos.position - Vector3.right * rangeX), (dishPos.position + Vector3.right * rangeX), t);

    //    }
    //    TimeContainer.ContainClear("TestShake");
    //    dishPos.localPosition = Vector3.zero;
    //}
    #endregion
}
