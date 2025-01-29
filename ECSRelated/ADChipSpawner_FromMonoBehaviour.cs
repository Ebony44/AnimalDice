using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.SocialPlatforms;

using System.Collections.Generic;

using Random = Unity.Mathematics.Random;
using System;
using Unity.Collections;
using System.Collections;

public class ADChipSpawner_FromMonoBehaviour : ResourceItemBase
{
    
    // public Sprite[] chipSpritesArray;

    public GameObject chipPrefab;
    public GameObject NoneECSchipPrefab;
    public int count;

    public GameObject systemHandlePrefab;
    

    #region Position related
    public float rangeX;
    public float rangeY;

    private const float bigBoardX = 10f + 1f;
    private const float bigBoardY = 3.2f - 0.2f;

    private const float smallUpLowBoardX = 8f + 0.5f;
    private const float smallUpLowBoardY = 0.4f + 0.6f;

    private const float verticalBoardX = 2f + 1f;
    private const float verticalBoardY = 3.2f + 0.4f;

    private const float horizontalBoardX = 10f;
    private const float horizontalBoardY = 0.4f + 0.6f;

    // private const float bigBoardX = 10f;
    // private const float bigBoardY = 1.2f;

    // public float bigBoardMultiplierForXAxis = 2.2f * 2f;
    // public float smallUpLowBoardMultiplierForXAxis = 2.2f * 2f;

    // public float gapVertBoardMultiplierForXAxis = 2.2f * 2f;
    // public float gapHorBoardMultiplierForXAxis = 2.2f * 2f;
    #endregion



    public EntityManager entityManager;

    private BlobAssetStore blobAssetStore;

    public GameObjectConversionSettings settings;

    #region seed related
    private uint spawnedCountForSeed = 0;
    
    private Random transformRandom;
    private Random rotationRandom;
    #endregion
    #region chip moving time related
    public float timeStandard = 0.4f;
    #endregion
    
    public List<Sprite> chipSpriteList;

    #region Blobasset
    private BlobAssetReference<SimpleAnimationBlob> uniqueBlob;
    public Dictionary<eAD_BUTTONLIST, Entity> chipStoredPrefabs = new Dictionary<eAD_BUTTONLIST, Entity>(capacity: 4);
    #endregion


    protected override void Start()
    {
        // var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);

        // var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(chipPrefab, settings);
        // chipEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(chipPrefab, settings);

        base.Start();

        settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, new BlobAssetStore());

        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        // entityManager = ResourceContainer.Get<ADGameMain>().customWorld.EntityManager;
        var tempMod = (int)eAD_AnteSetting.ANTE_500 * 4;
        var tempList = new List<Sprite>(capacity: 5);
        tempList.Add(chipSpriteList[(chipSpriteList.Count - 1)]);
        for(int i = 0;i<4;++i)
        {
            tempList.Add(chipSpriteList[tempMod + i]);
        }

        chipSpriteList = tempList;
        count = 1;
        // entityManager.sy

        // system.Update();

        // system.Enabled = true;
        // system.ShouldRunSystem();

        // entityManager.Instantiate()
        // var spawner = ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>();
        // var anteSetting = ResourceContainer.Get<ADAnteDependSetting>();
        // var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, new BlobAssetStore());
        
        //var systemEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(systemHandlePrefab, settings);
        //entityManager.AddComponentData(systemEntity, new ADSystemHandlerTag
        //{
        //    bCanMove = true,
        //    bCanMoveToDealer = false,
        //    bCanMarkChips = false,
        //    bCanMoveToBoard = false,
        //    bCanMoveToWinPlayer = false,
        //    bDestroyingChipEntities = false,
        //    bDestroyingAllChipEntitiesWithAlpha = false,
        //    bDestroyingOnlyLoseChipsWithAlpha = false,
        //});
        
    
    
    
    
    
    
    // var beforeConversionSpriteRenderer = beforeConversionObject.GetComponent<SpriteRenderer>();
    //for (int i = 0; i < anteSetting.smallChipSprites.Count; i++)
    //{
    //    beforeConversionSpriteRenderer.sprite = anteSetting.smallChipSprites[i];
    //    var chipEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(beforeConversionObject, settings);
    //    spawner.chipStoredPrefabs.Add((eAD_BUTTONLIST._BTN_BETTING_1 + i), chipEntity);
    //}

}



    private void DespawnAllChips()
    {
        // TODO: move this method into ADChipManager class
        if (ResourceContainer.Get<ADChipBettingManager>().gameObject.activeInHierarchy)
        {
            ResourceContainer.Get<ADChipBettingManager>().bDestroyingChipEntities = true;
        }
    }

    // System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
    public void SpawnChipsToSpawnPos(Vector3 chipStartPos, Vector3 chipMidPos, Vector3 chipTargetPos, eAD_BUTTONLIST paramChipKind, eADBetPlace paramBettingPlace, int bettingCount = -1, int winUserIndex = -1)
    {
        
        if (bettingCount == 0)
        {
            return;
        }

        #region
        // System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        // stopwatch.Start();
        #endregion

        //if(blobAssetStore == null)
        //{
        //    blobAssetStore = new BlobAssetStore();
        //}
        //var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
        // var settings = GameObjectConversionSettings.FromWorld(ResourceContainer.Get<ADGameMain>().customWorld, blobAssetStore);


        #region
        if ((int)paramChipKind - 0 < 0)
        {
            Debug.LogError("index out of range... from middle of chip betting");
            paramChipKind = eAD_BUTTONLIST._BTN_BETTING_1;
        }
        #endregion

        #region 
        /*
        var beforeConversionObject = chipPrefab;
        var beforeConversionSpriteRenderer = beforeConversionObject.GetComponent<SpriteRenderer>();
        // beforeConversionObject.GetComponent<SpriteRenderer>().sprite = chipSpriteList[(int)paramChipKind];
        // var tempMod = ResourceContainer.Get<ADAnteDependSetting>().currentAnteModifier * 4;

        beforeConversionSpriteRenderer.transform.rotation = Quaternion.Euler(0, 0, GetRandomAxis());

        beforeConversionSpriteRenderer.sprite = 
            ResourceContainer.Get<ADAnteDependSetting>().smallChipSprites[((int)(paramChipKind - 1 - eAD_BUTTONLIST._BTN_NONE))];

        beforeConversionSpriteRenderer.sortingOrder = ResourceContainer.Get<ADChipBettingManager>().GetDepthCount(paramBettingPlace);
        ResourceContainer.Get<ADChipBettingManager>().AddDepthCount(paramBettingPlace);



        // var chipEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(chipPrefab, settings);
        var chipEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(beforeConversionObject, settings);
        */
        #endregion

        Debug.Log("ad bet, betting spawn ongoing, betting count " + bettingCount);
        if(chipStoredPrefabs.Count == 0)
        {
            Debug.Log("buttons aren't set, error handle needed");
            return;
        }
        var chipEntity = chipStoredPrefabs[paramChipKind];
        var tempSprite = entityManager.GetComponentObject<SpriteRenderer>(chipEntity);
        tempSprite.sortingOrder = ResourceContainer.Get<ADChipBettingManager>().GetDepthCount(paramBettingPlace);
        ResourceContainer.Get<ADChipBettingManager>().AddDepthCount(paramBettingPlace);
        // tempSprite.transform.rotation = Quaternion.Euler(0, 0, GetRandomAxis());
        // entityManager.GetComponentObject<SpriteRenderer>(chipEntity)


        // ComponentDataFromEntity<SpriteRenderer>

        var Curve = ResourceContainer.Get<ADGameMain>().Curve;

        // var blob = SimpleAnimationBlob.CreateBlob(Curve, Allocator.Persistent); // this isn't optimized.... should make it only 'once'... and every entity shared that single component..

        if (uniqueBlob.IsCreated == false)
        {
            Debug.Log("unique blob is null");
            uniqueBlob = SimpleAnimationBlob.CreateBlob(Curve, Allocator.Persistent);
        }
        else
        {
            Debug.Log("unique blob is not null");
        }

        #region delete it after test
        //if (bettingCount > 0)
        //{
        //    count = bettingCount;
        //}
        //else
        //{
        //    count = 0;
        //}

        //else if(bettingCount == 0)
        //{

        //}
        #endregion
        if (bettingCount == -1)
        {
            bettingCount = 1;
        }
        for (int i = 0; i< bettingCount; ++i)
        {
            var instance = entityManager.Instantiate(chipEntity);

            // var spawnPosition = transform.TransformPoint(new float3(0, 0, 0));
            // var spawnPosition = spawnPos;
            // spawnPosition = GetRandomPosition(spawnPos, minRange, maxRange);


            var betPlaceSize = ResourceContainer.Get<ADChipBettingManager>().CategorizeBetPlace(paramBettingPlace);
            switch (betPlaceSize)
            {
                case eAD_BettingPlaceSize.BIG_BOARD:
                    rangeX = bigBoardX;
                    rangeY = bigBoardY;
                    break;
                case eAD_BettingPlaceSize.SMALL_UPLOW_BOARD:
                    rangeX = smallUpLowBoardX;
                    rangeY = smallUpLowBoardY;
                    break;
                case eAD_BettingPlaceSize.GAP_VERTICAL_BOARD:
                    rangeX = verticalBoardX;
                    rangeY = verticalBoardY;
                    break;
                case eAD_BettingPlaceSize.GAP_HORIZONTAL_BOARD:
                    rangeX = horizontalBoardX;
                    rangeY = horizontalBoardY;
                    break;
                case eAD_BettingPlaceSize._PLACESIZE_MAX:
                    rangeX = 0;
                    rangeY = 0;
                    break;
            }
            var dstPosition = GetRandomPosition(chipTargetPos, rangeX, rangeY);
            //Debug.Log("random dst position's X : " + dstPosition.x
            //    + " Y : " + dstPosition.y);
            
            

            entityManager.SetComponentData(instance, new Translation { Value = chipStartPos });
            entityManager.AddComponentData(instance, new ADChipTag
            {
                srcPos = chipStartPos,
                // midPos = chipMidPos,
                dstPos = dstPosition,
                chipKind = paramChipKind,

                rotationZAxis = GetRandomAxis(),
                bettingPlace = paramBettingPlace,

                bCanBeDestroyed = false,
                // bCanBeDestroyedWithAlpha = false,

                traveledTime = 0f,
                travelTimeStandard  = this.timeStandard,

                bIsLose = false,
                winUserIndex = winUserIndex,


            });


            entityManager.AddComponentData(instance, new SimpleBlobAnimationTag { anim = uniqueBlob, t = 0f, });

            // entityManager.SetComponentData<SpriteRenderer>(instance, new SpriteRenderer { sprite = chipSprites[(int)paramChipKind] });

            // ComponentType.Exclude

        }

        // blobAssetStore.Dispose();

        #region
        // stopwatch.Stop();
        // Debug.Log("betting chip spawn time taken: " + (stopwatch.Elapsed));
        // stopwatch.Reset();
        #endregion

    }

    public void SpawnChipsToSpawnPos(Vector3 chipStartPos, Vector3 chipMidPos, Vector3 chipTargetPos, BetSpawnInfo betSpawnInfo)
    {
        SpawnChipsToSpawnPos(chipStartPos, chipMidPos, chipTargetPos,
            betSpawnInfo.chipKind,
            betSpawnInfo.betPlace,
            betSpawnInfo.bettingCount,
            betSpawnInfo.winUserIndex
            );
    }

    
    /// <summary>
    /// only used for gamestate, middle of connect or reconnect 
    /// dispense chips into bettingboard
    /// </summary>
    /// <param name="chipKind"></param>
    /// <param name="betPlace"></param>

    public void SpawnChipsToRandomSpawnPos(eAD_BUTTONLIST chipKind,eADBetPlace betPlace, int bettingCount = 1)
    {
        // blobAssetStore = new BlobAssetStore();
        // var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);

        // var settings = GameObjectConversionSettings.FromWorld(ResourceContainer.Get<ADGameMain>().customWorld, blobAssetStore);

        #region
        if ((int)chipKind - 0 < 0)
        {
            Debug.LogError("index out of range... from middle of chip betting");
            chipKind = eAD_BUTTONLIST._BTN_BETTING_1;
        }
        #endregion

        //var beforeConversionObject = chipPrefab;
        //var beforeConversionSpriteRenderer = beforeConversionObject.GetComponent<SpriteRenderer>();

        //beforeConversionSpriteRenderer.transform.rotation = Quaternion.Euler(0, 0, GetRandomAxis());

        //beforeConversionSpriteRenderer.sprite = ResourceContainer.Get<ADAnteDependSetting>().smallChipSprites[((int)(chipKind - 1 - eAD_BUTTONLIST._BTN_NONE))];


        //beforeConversionSpriteRenderer.sortingOrder = ResourceContainer.Get<ADChipBettingManager>().GetDepthCount(betPlace);
        //ResourceContainer.Get<ADChipBettingManager>().AddDepthCount(betPlace);
        
        // var chipEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(beforeConversionObject, settings);

        var chipEntity = chipStoredPrefabs[chipKind];
        var tempSprite = entityManager.GetComponentObject<SpriteRenderer>(chipEntity);
        tempSprite.sortingOrder = ResourceContainer.Get<ADChipBettingManager>().GetDepthCount(betPlace);
        ResourceContainer.Get<ADChipBettingManager>().AddDepthCount(betPlace);


        

        var Curve = ResourceContainer.Get<ADGameMain>().Curve;

        // var blob = SimpleAnimationBlob.CreateBlob(Curve, Allocator.Persistent); // this isn't optimized.... should make it only 'once'... and every entity shared that single component..
        if (uniqueBlob.IsCreated == false)
        {
            Debug.Log("unique blob is null");
            uniqueBlob = SimpleAnimationBlob.CreateBlob(Curve, Allocator.Persistent);
        }
        else
        {
            Debug.Log("unique blob is not null");
        }

        for (int i = 0; i < bettingCount; ++i)
        {
            var instance = entityManager.Instantiate(chipEntity);

            var chipTargetPos = ResourceContainer.Get<ADChipBettingManager>().GetTablePositionFromBetPlace(betPlace);

            var betPlaceSize = ResourceContainer.Get<ADChipBettingManager>().CategorizeBetPlace(betPlace);
            switch (betPlaceSize)
            {
                case eAD_BettingPlaceSize.BIG_BOARD:
                    rangeX = bigBoardX;
                    rangeY = bigBoardY;
                    break;
                case eAD_BettingPlaceSize.SMALL_UPLOW_BOARD:
                    rangeX = smallUpLowBoardX;
                    rangeY = smallUpLowBoardY;
                    break;
                case eAD_BettingPlaceSize.GAP_VERTICAL_BOARD:
                    rangeX = verticalBoardX;
                    rangeY = verticalBoardY;
                    break;
                case eAD_BettingPlaceSize.GAP_HORIZONTAL_BOARD:
                    rangeX = horizontalBoardX;
                    rangeY = horizontalBoardY;
                    break;
                case eAD_BettingPlaceSize._PLACESIZE_MAX:
                    rangeX = 0;
                    rangeY = 0;
                    break;
            }
            // var dstPosition = GetRandomPosition(chipTargetPos, rangeX, rangeY);

            var srcPosition = GetRandomPosition(chipTargetPos, rangeX, rangeY);
            var dstPosition = srcPosition;

            Debug.Log("random src position's X : " + srcPosition.x
                + " Y : " + srcPosition.y);

            entityManager.SetComponentData(instance, new Translation { Value = srcPosition });
            entityManager.AddComponentData(instance, new ADChipTag
            {
                srcPos = srcPosition,
                // midPos = new float3(1f, 1f, 1f),
                dstPos = dstPosition,
                chipKind = chipKind,

                // rotationZAxis = GetRandomAxis(),
                bettingPlace = betPlace,

                bCanBeDestroyed = false,
                // bCanBeDestroyedWithAlpha = false,

                traveledTime = 1f,
                travelTimeStandard = this.timeStandard,

                bIsLose = false,
                winUserIndex = -1,


            });

            entityManager.AddComponentData(instance, new SimpleBlobAnimationTag { anim = uniqueBlob, t = 0f, });

        }


        // blobAssetStore.Dispose();
    }

    public IEnumerator SpawnChipRoutine(eAD_BUTTONLIST chipKind, eADBetPlace betPlace, int bettingCount = 1)
    {
        // this.Wait(timeContainer);
        // yield return new WaitForSeconds(timeContainer.time);
        while(ResourceContainer.Get<ADAnteDependSetting>().chipValueInThisRoom[eAD_BUTTONLIST._BTN_BETTING_1].Equals(-1))
        {
            yield return null;
        }
        SpawnChipsToRandomSpawnPos(chipKind, betPlace, bettingCount);
    }

    public void OnDestroy()
    {
        // dispose if it's not released already
        //if(blobAssetStore != null)
        //{
        //    blobAssetStore.Dispose();
        //}

        settings.BlobAssetStore.Dispose();



        //EntityManager.DestroyEntity(EntityManager.UniversalQuery);
        
        // entityManager.DestroyEntity(entityManager.UniversalQuery);

        //foreach (var e in entityManager.GetAllEntities())
        //    entityManager.DestroyEntity(e);
    }


    //public float3 GetRandomPosition(Vector3 spawnPos, float3 minRange, float3 maxRange)
    //{
    //    uint seed = (uint)count;
    //    seed = (uint)( (seed * 123) ^ (int)(spawnPos.x * 321)  );
    //    seed = (uint)((seed * 123) ^ ( (spawnedCountForSeed + 4) * 56) );
    //    var random = new Random(seed);

    //    var tempX = random.NextFloat(minRange.x, maxRange.x);
    //    var tempY = random.NextFloat(minRange.y, maxRange.y);
    //    var tempZ = random.NextFloat(minRange.z, maxRange.z);
        
        
    //    spawnedCountForSeed++;
    //    return new float3(tempX, tempY, tempZ);
    //}
    public float3 GetRandomPosition(Vector3 position, float rangeX, float rangeY)
    {
        
        // if(bIsTransformRandomInit == false)
        if(transformRandom.state == 0)
        {
            uint seed = (uint)1;
            seed = (uint)((seed * 4) ^ (int)((Time.realtimeSinceStartup + 1) * 2));
            seed = (uint)((seed * 45) ^ ((spawnedCountForSeed + 4) * 21));
            transformRandom = new Random(seed);
            spawnedCountForSeed++;
            // bIsTransformRandomInit = true;
        }
        // var random = new Random(seed);

        var debugX = position.x - rangeX;
        var debugX2 = position.x + rangeX;

        var tempX = transformRandom.NextFloat(position.x - rangeX, position.x + rangeX );
        var tempY = transformRandom.NextFloat(position.y - rangeY, position.y + rangeY);
        var tempZ = 100f;//random.NextFloat(minRange.z, maxRange.z);


        
        return new float3(tempX, tempY, tempZ);
    }
    public float GetRandomAxis()
    {
        
        // if (bIsRotationRandomInit == false)
        if(rotationRandom.state == 0)
        {
            uint seed = (uint)1;
            seed = (uint)((seed * 41) ^ (int)(rangeX * 321));
            seed = (uint)((seed * 4) ^ (int)((Time.realtimeSinceStartup + 1) * 2));
            seed = (uint)((seed * 45) ^ ((spawnedCountForSeed + 12) * 79));
            rotationRandom = new Random(seed);
            spawnedCountForSeed++;
            // bIsRotationRandomInit = true;
        }


        return rotationRandom.NextFloat(0f, 360f);
        // return random.NextFloat(-25f, 25f);


    }


    //public void SpawnChipsInNormalWay(Vector3 chipMidPos, Vector3 chipTargetPos, ChipKind_AnimalDice paramChipKind)
    //{
    //    var spawnPosition = spawnPos;
    //    for (int i = 0;i<count;++i)
    //    {
    //        spawnPosition = GetRandomPosition(spawnPos, rangeX, rangeY);
    //        var instance = Instantiate(NoneECSchipPrefab) as GameObject;// , new Vector3(spawnPosition.x, spawnPosition.y, spawnPosition.z));
    //        instance.transform.localPosition = spawnPosition;
    //    }
    //}

    public void CreateChipFromResultStep()
    {
        if(ResourceContainer.Get<ADChipBettingManager>().bCanMoveToBoard)
        {

        }
        var winInfo = ResourceContainer.Get<ADResultPartInfoStoring>();
        var bettingFunction = ResourceContainer.Get<ADChipBettingManager>();
        var currentChips = new Dictionary<eAD_BUTTONLIST, int>(capacity: 4);
        // from dealer to board
        // 1. win bet place
        // 2. win users
        // 3. how much money to translate
        foreach (var winBetPlace in winInfo.winBetPlace) // from win bet place
        {
            var users = winInfo.winningUsersChips[winBetPlace];
            for (int userIndex = 0; userIndex < users.Count; userIndex++) // from users
            {
                if (users[userIndex] > 0)
                {
                    currentChips = bettingFunction.GetChipsFromMoney(users[userIndex]);

                    foreach (var item in currentChips) // from chip kind's count
                    {

                        ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().SpawnChipsToSpawnPos(
                            chipStartPos: ResourceContainer.Get<Transform>("NPC").transform.position,
                            chipMidPos: Vector3.zero,
                            chipTargetPos: bettingFunction.GetTablePositionFromBetPlace(winBetPlace),
                            paramBettingPlace: winBetPlace,
                            paramChipKind: item.Key,
                            bettingCount: item.Value,
                            winUserIndex: userIndex
                            );

                    }
                    users[userIndex] = 0;
                }
            }
        }
    }

    public BlobAssetReference<SimpleAnimationBlob> GetBlobAnimCurveWithECS()
    {
        AnimationCurve tempCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        var blob = SimpleAnimationBlob.CreateBlob(tempCurve, Unity.Collections.Allocator.Persistent);
        return blob;

    }

}
