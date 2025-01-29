using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
// using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Wooriline;

public class ADTest_2 : ResourceItemBase
{
    ADChipBettingManager tempManager;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();        
        CoroutineChain.Start
            .Wait(0.2f)
            .Call(() =>
            {
                tempManager = ResourceContainer.Get<ADChipBettingManager>();
                tempManager.SetEnableBettingBoards(true);
                // ChipSetting();
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChipSetting()
    {
        var anteSetting = ResourceContainer.Get<ADAnteDependSetting>();
        var moneyList = new List<long>() { 1000, 2000, 10000, 20000 };
        anteSetting.SetSmallChipAndButtons(moneyList);

        #region set chip entity setting and store it

        var spawner = ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>();
        
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, new BlobAssetStore());
        var beforeConversionObject = spawner.chipPrefab;
        var beforeConversionSpriteRenderer = beforeConversionObject.GetComponent<SpriteRenderer>();
        for (int i = 0; i < anteSetting.smallChipSprites.Count; i++)
        {
            beforeConversionSpriteRenderer.sprite = anteSetting.smallChipSprites[i];
            var chipEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(beforeConversionObject, settings);
            spawner.chipStoredPrefabs.Add((eAD_BUTTONLIST._BTN_BETTING_1 + i), chipEntity);
        }


        #endregion
    }
    public void OnBetting(string id)
    {
        // var tempManager = ResourceContainer.Get<ADChipBettingManager>();
        
        var bettingChipButtons = tempManager.bettingChipButtons;
        var tempIndex = (int)(tempManager.currentButtonIndex - 1 - eAD_BUTTONLIST._BTN_NONE);
        var bettingPosObject = ResourceContainer.Get<ADBettingBoardTag>(id);//.GetComponent<ADBettingBoardTag>();
        eADBetPlace bettingPos = eADBetPlace._BET_ANIMAL_GE;
        if (bettingPosObject != null)
        {

            Debug.Log("betting place is " + bettingPosObject.bettingPlace.ToString());
            bettingPos = bettingPosObject.bettingPlace;
        }
        ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().SpawnChipsToSpawnPos(bettingChipButtons[tempIndex].transform.position,
            new Unity.Mathematics.float3(0, 0, 0),
            chipTargetPos: bettingPosObject.transform.position,
            paramChipKind: tempManager.currentButtonIndex,
            paramBettingPlace: bettingPos);

        tempManager.SetMyBettingMoney(bettingPos, 1000);

    }
    public void OnBetting(eADBetPlace betPlace)
    {
        

        // var bettingChipButtons = tempManager.bettingChipButtons;
        // var tempIndex = (int)(tempManager.currentButtonIndex - 1 - eAD_BUTTONLIST._BTN_NONE);
        
        eADBetPlace bettingPos = betPlace;
        
        // var bettingPosObject = ResourceContainer.Get<ADChipBettingManager>().GetBetBoard(betPlace);
        

        //ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().SpawnChipsToSpawnPos(bettingChipButtons[tempIndex].transform.position,
        //    new Unity.Mathematics.float3(0, 0, 0),
        //    chipTargetPos: bettingPosObject.transform.position,
        //    paramChipKind: tempManager.currentButtonIndex,
        //    paramBettingPlace: bettingPos);

        tempManager.SetMyBettingMoney(bettingPos, 1000);

    }
    public void OnBettingForAuto(string id)
    {
        Debug.Log("[AD_TEST_2], Start: ");
        var bettingChipButtons = tempManager.bettingChipButtons;
        var tempIndex = (int)(tempManager.currentButtonIndex - 1 - eAD_BUTTONLIST._BTN_NONE);
        var bettingPosObject = ResourceContainer.Get<ADBettingBoardTag>(id);//.GetComponent<ADBettingBoardTag>();
        eADBetPlace bettingPos = eADBetPlace._BET_ANIMAL_GE;
        if (bettingPosObject != null)
        {

            Debug.Log("betting place is " + bettingPosObject.bettingPlace.ToString());
            bettingPos = bettingPosObject.bettingPlace;
        }
        ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().SpawnChipsToSpawnPos(bettingChipButtons[tempIndex].transform.position,
            new Unity.Mathematics.float3(0, 0, 0),
            chipTargetPos: bettingPosObject.transform.position,
            paramChipKind: tempManager.currentButtonIndex,
            paramBettingPlace: bettingPos);

        Debug.Log("[AD_TEST_2], End: ");
    }
    public void OnBettingForAuto(eADBetPlace betPlace)
    {
        

        var bettingChipButtons = tempManager.bettingChipButtons;
        var tempIndex = (int)(tempManager.currentButtonIndex - 1 - eAD_BUTTONLIST._BTN_NONE);
        
        eADBetPlace bettingPos = betPlace;
        
        var bettingPosObject = ResourceContainer.Get<ADChipBettingManager>().GetBetBoard(betPlace);


        //ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().SpawnChipsToSpawnPos(bettingChipButtons[tempIndex].transform.position,
        //    new Unity.Mathematics.float3(0, 0, 0),
        //    chipTargetPos: bettingPosObject.transform.position,
        //    paramChipKind: tempManager.currentButtonIndex,
        //    paramBettingPlace: bettingPos);

        
    }
    [TestMethod(false)]
    public void DisableBettingLabel()
    {
        tempManager.bDestroyingChipEntities = true;
        tempManager.DisableAllMyBettingMoneyLabel();

        CoroutineChain.Start
            .Call(() => tempManager.bDestroyingAllChipEntitiesWithAlpha = true)
            .Wait(0.6f)
            .Call(() => tempManager.bDestroyingAllChipEntitiesWithAlpha = false);
    }

    #region auto related
    public bool bIsAuto = false;
    public Coroutine autoRoutine;
    public Rotation rotationScript;
    public Image autoAImage;
    public Image autoRotateImage;

    public List<Sprite> autoPrepSprite;

    public float minBettingInterval = 0.0002f;
    public float maxBettingInterval = 0.0005f;
    public void OnAutoButtonClick()
    {

        bIsAuto = !bIsAuto;

        if (bIsAuto == true)
        {
            if (autoRoutine != null)
            {
                StopCoroutine(autoRoutine);
            }
            autoRoutine = StartCoroutine(StartAutoRoutine(11, minBettingInterval));
            rotationScript.SetLoop(true);
            autoAImage.sprite = autoPrepSprite[2];
            autoRotateImage.sprite = autoPrepSprite[3];
        }
        else
        {
            rotationScript.SetLoop(false);
            autoAImage.sprite = autoPrepSprite[0];
            autoRotateImage.sprite = autoPrepSprite[1];
        }
    }

    #region test Variables
    public static int REGISTER_COUNT = 0;
    public int registerCount = 0;
    #endregion
    public IEnumerator StartAutoRoutine(int iterationCount = 1, float bettingMinInterval = 0.3f)
    {
        yield return null;
        var betPlaceList = ResourceContainer.Get<ADChipBettingManager>().betPlaceSizeList;

        while (bIsAuto)
        {

            var tempRandomFloat = Random.Range(bettingMinInterval, maxBettingInterval);
            
            // var tempRandomBetChipKind = Random.Range((int)eAD_BUTTONLIST._BTN_BETTING_1, (int)eAD_BUTTONLIST._BTN_BETTING_5);
            

            yield return new WaitForSeconds(tempRandomFloat);
            ResourceContainer.Get<ADGameMain>().bettingWatch.Start();
            Debug.Log("[AD_TEST_2], Register Count: " + REGISTER_COUNT);
            
            for (int i = 0; i < iterationCount; i++)
            {
                Debug.Log("[AD_TEST_2], Register");
                REGISTER_COUNT++;
                #region part of betting packet
                // var rec = new R_09_BET(SubGameSocket.m_bytebuffer);
                //var idx = GameUtils.ConvertSerialToPosition(0);
                // var user = ResourcePool.Find<GamePlayer>(p => p.roomIdx == idx);

                //user.lbHave.SetAlpha(0f, false);
                //TimeContainer.ContainClear("UserHaveMoneyRolling");
                //TimeContainer t1 = new TimeContainer("UserHaveMoneyRolling", GameUtils.st_globalTweenTime);



                //ResourceContainer.Get<ADGameMain>().SetPlayerGapMoney(user, 1000);
                //user.lbHave.color = new Color(0f, 0f, 0f, 0f);


                // ResourceContainer.Get<ADMyInfoTag>("MyInfo").haveMoney = 500;
                // ResourceContainer.Get<ADMyInfoTag>("MyInfo").gapMoney = 500;
                // ResourceContainer.Get<ADChipBettingManager>().PlayBettingSound(5, false);
                #endregion


                var tempRandomBetPlace = betPlaceList[Random.Range(0, betPlaceList.Count)].betPlace;
                var tempBetString = (tempRandomBetPlace.ToString() + "_" + ((int)tempRandomBetPlace).ToString());
                var tempRandomBetChipKind = Random.Range((int)eAD_BUTTONLIST._BTN_BETTING_1, (int)eAD_BUTTONLIST._BTN_BETTING_2);

                ResourceContainer.Get<ADChipBettingManager>().currentButtonIndex = (eAD_BUTTONLIST)tempRandomBetChipKind;
                // ResourceContainer.Get<ADChipBettingManager>().OnBetting(tempBetString);

                #region actionplayer related
                //List<st09_BET_DATA> tempBetList = new List<st09_BET_DATA>();
                //var tempItem = new st09_BET_DATA();
                //tempItem.stBETMONEY = 1000;
                //tempItem.stUSER.nSERIAL = 0;
                //tempItem.stUSER.szID = "";
                //tempItem.stHAVEMONEY.stHAVEMONEY = 6000;
                //tempItem.stHAVEMONEY.stGAPMONEY = 1000;

                //tempItem.nTABLEPOS = (int)tempRandomBetPlace;
                //tempItem.nBTNIDX = (int)tempRandomBetChipKind;
                //// tempItem.stPARTINFO.nTABLEPOS
                //tempBetList.Add(tempItem);
                //ADBettingListAction tempAction = new ADBettingListAction(tempBetList);
                //ActionPlayer.Play(tempAction);
                //ADTestBettingAction testAction = new ADTestBettingAction();
                //ActionPlayer.Play(testAction);
                #endregion


                ResourceContainer.Get<ADChipBettingManager>().Req_Betting(tempRandomBetPlace, (eAD_BUTTONLIST)tempRandomBetChipKind);
                // OnBettingForAuto(tempBetString);


                // yield return new WaitForSeconds(0.02f);
            }
            // ResourceContainer.Get<ADChipBettingManager>().Req_Betting(tempRandomBetPlace, (eAD_BUTTONLIST)tempRandomBetChipKind);
            ResourceContainer.Get<ADGameMain>().bettingWatch.Stop();
            // Debug.Log("betting test, time was taken : " + ResourceContainer.Get<ADGameMain>().bettingWatch.ElapsedMilliseconds + " ms");

            ResourceContainer.Get<ADGameMain>().bettingWatch.Reset();
            yield return null;
        }
    }




    #endregion

    #region sound related test method

    private bool bIsSoundRoutineOn = false;

    [TestMethod(false)]
    public void StartSoundPlayRoutine(bool bEnable)
    {
        bIsSoundRoutineOn = bEnable;
        var tempManager = ResourceContainer.Get<ADChipBettingManager>();
        tempManager.bIsBettingTime = bEnable;
        if (bEnable)
        {
            StartCoroutine(TestSoundPlayRoutine());
            
            
            

        }
    }
    public IEnumerator TestSoundPlayRoutine()
    {
        var tempManager = ResourceContainer.Get<ADChipBettingManager>();
        while (bIsSoundRoutineOn)
        {

            // Sound.Instance.EffPlay("e_chip1");
            tempManager.PlayBettingSound(1, false);
            yield return new WaitForSeconds(0.004f);
            if(tempManager.bIsSoundRoutineOff == true)
            {
                StartCoroutine(tempManager.OtherBettingSoundSkipRoutine());
            }
        }
    }
    [TestMethod(false)]
    public void StartSoundForcefully()
    {
        Sound.Instance.EffPlayWithForce("e_chip3");

    }

    #endregion

}

public class ADTestBettingAction : IAction
{

    public static int REGISTER_COUNT = 0;
    public string Log => "[R_09_ADBettingAction]" + REGISTER_COUNT;

    // public string ID => "ADBetting";
    public string ID => null;

    public List<string> CancelID => null;

    public int testInt = 1;
    public int tempVariable = 0;

    public ADTestBettingAction()
    {
 
        tempVariable = REGISTER_COUNT++;
        
    }
    public IEnumerator ActionRoutine()
    {
        testInt++;
        if(testInt == -1)
        {
            yield return null;
        }
        
        //throw new System.NotImplementedException();
    }

    public IEnumerable<TimeContainer> GetAllTimeContainerList()
    {
        throw new System.NotImplementedException();
    }


    

}