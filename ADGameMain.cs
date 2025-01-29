using System;
using System.Collections;
using System.Collections.Generic;
// using System.Diagnostics;
using System.Linq;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ADGameMain : ResourceItemBase
{
    #region ECS related
    private ADChipMovementSystem movementSystem;
    private ADChipMarkSystem markSystem;
    // ADChipCreatingSystem creatingSystem;
    private ADChipModifyingSpriteSystem spriteModifyingSystem;
    private ADChipDestroySystem destroySystem;
    // SimpleBlobAnimationSystem blobCurveSystem;

    public World customWorld;

    public BlobAssetStore animationCurveBlobStored;

    public AnimationCurve Curve = AnimationCurve.EaseInOut(0, 0, 1, 1);


    #endregion

    public Camera RootCam;

    #region player related
    public int mySerial;

    public Coroutine gapMoneyTweenRoutine;

    public Dictionary<GamePlayer, long> gapMoneysDic = new Dictionary<GamePlayer, long>(capacity: 11);
    private Dictionary<int, ADPlayerGapNameSwitch> gapScripts = new Dictionary<int, ADPlayerGapNameSwitch>(capacity: 11);
    #endregion

    #region test purpose
    public Texture tempTexture;


    public System.Diagnostics.Stopwatch bettingWatch = new System.Diagnostics.Stopwatch();

    #endregion

    #region gamestate
    public bool bJoinMiddleOfGame = true;
    #endregion

    private void Awake()
    {
        Debug.Log("can issue for replay? " + PacketTracker.IsExistsAndEnableToIssue());
        if (PacketTracker.IsExistsAndEnableToIssue())
        {
            // PacketTracker.Instance.InitializeBeforeReplayStart();
        }

        

    }
    protected override void Start()
    {
        base.Start();

        CreateSystems();
        SetupScreenCut();

        //Debug.Log("[ScreenSetup] 4:3\nwidth : " +
        //        Screen.width + ", height : " + Screen.height + ", " +
        //        (float)Screen.width / (float)Screen.height);

        //if (PacketTracker.IsExistsAndEnableToIssue(true))
        //{
        //    StartCoroutine(PacketTracker.Instance.IssuePackets(EGAMELIST.GAME09_ANIMALDICE));
        //}
        if(cGlobalInfos.GetIntoRoomInfo_97() != null)
        {
            // mySerial = cGlobalInfos.GetIntoRoomInfo_97().nSERIAL;
        }
        mySerial = cGlobalInfos.GetIntoRoomInfo_97().nSERIAL;
        // ResourceContainer.Get<ResourcePool>().prefabList

        // ResourceContainer.Get<GameActionEmoticon>()

        // var temp = ResourcePool.GetAll<GamePlayer>().ToList();


        //gapMoneys.Add()

        CoroutineChain.Start
            .Wait(0.1f)
            .Call(() => ResourceContainer.Get<GameMenuFolding>().EnableVoiceless(true));
        // ResourceContainer.Get<GameMenuFolding>().EnableVoiceless(true);

        if (PacketTracker.IsExistsAndEnableToIssue(true))
        {
            // StartCoroutine(PacketTracker.Instance.IssuePackets(EGAMELIST.GAME09_ANIMALDICE));
        }

    }
    
    [TestMethod(false)]
    public void TestDisplayDictionary()
    {
        var temp = gapMoneysDic;
    }

    private void SetupScreenCut()
    {
        if(cScreenMgr.self == null)
        {
            return;
        }
        if ((float)Screen.width / (float)Screen.height <= cScreenMgr.self.screenPadRatioCut)
        {
            Debug.Log("[ScreenSetup], screen is under 1.6 ratio... 4:3\nwidth : " +
                Screen.width + ", height : " + Screen.height + ", " +
                (float)Screen.width / (float)Screen.height);
            RootCam.orthographicSize = 68;
        }

        //throw new NotImplementedException();
    }

    #region ECS system handle
    private void CreateSystems()
    {

        movementSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<ADChipMovementSystem>();
        markSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<ADChipMarkSystem>();
        // creatingSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<ADChipCreatingSystem>();
        spriteModifyingSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<ADChipModifyingSpriteSystem>();
        destroySystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<ADChipDestroySystem>();
        // blobCurveSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<SimpleBlobAnimationSystem>();

        movementSystem.Enabled = true;
        markSystem.Enabled = true;
        // creatingSystem.Enabled = true;
        spriteModifyingSystem.Enabled = true;
        destroySystem.Enabled = true;
        // blobCurveSystem.Enabled = true;


        World.DefaultGameObjectInjectionWorld.GetExistingSystem<SimulationSystemGroup>().AddSystemToUpdateList(movementSystem);
        World.DefaultGameObjectInjectionWorld.GetExistingSystem<SimulationSystemGroup>().AddSystemToUpdateList(markSystem);
        // World.DefaultGameObjectInjectionWorld.GetExistingSystem<SimulationSystemGroup>().AddSystemToUpdateList(creatingSystem);
        World.DefaultGameObjectInjectionWorld.GetExistingSystem<SimulationSystemGroup>().AddSystemToUpdateList(spriteModifyingSystem);
        World.DefaultGameObjectInjectionWorld.GetExistingSystem<SimulationSystemGroup>().AddSystemToUpdateList(destroySystem);
        // World.DefaultGameObjectInjectionWorld.GetExistingSystem<SimulationSystemGroup>().AddSystemToUpdateList(blobCurveSystem);



    }
    public void CheckSystemEnabled()
    {

    }
    private void OnDestroy()
    {
        movementSystem.Enabled = false;
        markSystem.Enabled = false;
        // creatingSystem.Enabled = false;
        spriteModifyingSystem.Enabled = false;
        destroySystem.Enabled = false;
        // blobCurveSystem.Enabled = false;
        if (World.DefaultGameObjectInjectionWorld != null)
        {
            var temp = World.DefaultGameObjectInjectionWorld.EntityManager.GetAllEntities();

            if (World.DefaultGameObjectInjectionWorld.EntityManager.GetAllEntities().Count() != 0)
            {
                World.DefaultGameObjectInjectionWorld.EntityManager.DestroyEntity(World.DefaultGameObjectInjectionWorld.EntityManager.GetAllEntities());

            }
        }
        


    }

    public void EnableMarkSystem(bool bEnable)
    {
        markSystem.Enabled = bEnable;
    }
    public void EnableSpriteModifySystem(bool bEnable)
    {
        spriteModifyingSystem.Enabled = bEnable;
    }
    public void EnableDestroySystem(bool bEnable)
    {
        destroySystem.Enabled = bEnable;
    }
    

    #endregion

    public void ClearAll()
    {
        ResourceContainer.Get<ADResultPartInfoStoring>().ResetForNewGame();
        ResourceContainer.Get<ADChipBettingManager>().ClearForNewGame();

        // ResourceContainer.Get<ADChipBettingManager>().ClearAllChips();
        // ResourceContainer.Get<ADChipBettingManager>().ClearMyTotalBetting();
        // ResourceContainer.Get<ADChipBettingManager>().DisableAllMyBettingMoneyLabel();
        ResourceContainer.Get<ADHistoryPopup>().ResetCurrentDiceValue();
        // animationCurveBlobStored.
        if(animationCurveBlobStored != null)
        {
            // animationCurveBlobStored.Dispose();
            animationCurveBlobStored.ResetCache(true);
        }

        ResourceContainer.Get<ADGameMain>().HandleBackgroundMusicWithDelay(0.5f, 1f);

        #region delete after stress test
        for (int i = 0; i < playersBettingCount.Count; i++)
        {
            playersBettingCount[i] = 0;
        }
        #endregion

    }
    public void StoreAnimCurveBlob(BlobAssetStore blobAsset)
    {
        // animationCurveBlobStored.TryAdd<>(blobAsset.GetHashCode(), ref blobAsset);
        // animationCurveBlobStored.TryAdd<SimpleAnimationBlob>(ref blobAsset);
    }

    [TestMethod(false)]
    public void TestSceneLoad()
    {
        // SceneManager.LoadScene("Game09", LoadSceneMode.Single);
        // var floatTime = Mathf.Abs((float)dueTime.TotalSeconds);
        System.DateTime testTime = System.DateTime.Now.AddSeconds(3f);
        Debug.Log("current time " + testTime.ToString());
        Debug.Log(" time abstracted " + testTime.CompareTo(System.DateTime.Now));
    }


    #region player related

    public void SetPlayerGabMoney(GamePlayer player, long startGap, long endGap)
    {
        if (player.roomSerial == cGlobalInfos.GetIntoRoomInfo_97().nSERIAL)
        {
            ResourceContainer.Get<ADMyInfoTag>("MyInfo").gapMoney = endGap;
        }
        #region delete gap/nick of player -> obsolete
        //else
        //{
            
        //    if (gapMoneysDic.ContainsKey(player) == false)
        //    {
        //        Debug.Log("custom method, gap if caught " + player.Nick );
        //        gapMoneysDic.Add(player, 0);
        //    }
        //    else
        //    {
        //        Debug.Log("custom method, gap else caught" + player.Nick);
        //        if(gapScripts.ContainsKey(player.roomIdx) == false)
        //        {
        //            gapScripts[player.roomIdx] = player.gameObject.GetComponent<ADPlayerGapNameSwitch>();
        //        }
        //        if (gapScripts[player.roomIdx].isName)
        //        {

        //            player.lbGab.text = endGap.ToStringWithKMB(false, 3, false);
        //            player.lbGab.color = endGap >= 0 ? new Color(0,1,0,0): new Color(1, 0, 0, 0);
        //            if (endGap == 0)
        //            {
        //                player.lbGab.color = new Color(1, 1, 1, 0);
        //            }
        //        }
        //        else
        //        {
        //            player.lbGab.NumberTween(startGap, endGap, g => g.ToStringWithKMB(false, 3, false), 0.25f);

        //            player.lbGab.color = endGap >= 0 ? Color.green : Color.red;
        //            if (endGap == 0)
        //            {
        //                player.lbGab.color = Color.white;
        //            }
        //        }
        //        gapMoneysDic[player] = endGap;
        //    }
        //    //player.lbGab.NumberTween(startGap, endGap, g => g.ToStringWithKMB(false, 3, false), 0.25f);
        //    //// player.lbGab.text = endGap.ToStringWithKMB(false, 3, false);
        //    //player.lbGab.color = endGap >= 0 ? Color.green : Color.red;
        //    //if (endGap == 0)
        //    //{
        //    //    player.lbGab.color = Color.white;
        //    //}

        //    // player.Gap = endGap;
        //    // ResourceContainer.Get<ADGameMain>().gapMoneys[user] = betItem.stHAVEMONEY.stGAPMONEY;
        //    Debug.Log("custom method, player start gap " + player.Gap
        //    + " player end gap " + endGap);
        //}
        #endregion

    }
    //public IEnumerator SetPlayerGapMoneyRoutine(GamePlayer player, long startGap, long endGap)
    //{
    //    if (player.roomSerial == mySerial)
    //    {
    //        ResourceContainer.Get<ADMyInfoTag>("MyInfo").gapMoney = endGap;
    //    }
    //    else
    //    {
    //        //player.lbGab.NumberTween(startGap, endGap, g => g.ToStringWithKMB(false, 3, false), 0.25f);
    //        //// player.lbGab.text = endGap.ToStringWithKMB(false, 3, false);
    //        //player.lbGab.color = endGap >= 0 ? Color.green : Color.red;
    //        //if (endGap == 0)
    //        //{
    //        //    player.lbGab.color = Color.white;
    //        //}

    //        player.Gap = endGap;
    //        Debug.Log("custom method, player start gap " + player.Gap
    //        + " player end gap " + endGap);
    //    }
    //    yield return null;
    //}
    public void SetPlayerGapMoney(GamePlayer player, long endGap)
    {
        // var player = ResourcePool.Find<GamePlayer>(p => p.roomIdx == playerIndex);
        //Debug.Log("custom method, player start gap " + player.Gap
        //    + " player end gap " + endGap);
        //SetPlayerGabMoney(player, player.Gap, endGap);

        if(player == null)
        {
            return;
        }
        
        if (gapMoneysDic.ContainsKey(player) == false)
        {
            gapMoneysDic.Add(player, 0);
        }
        Debug.Log("custom method, player start gap " + gapMoneysDic[player]
            + " player end gap " + endGap);
        SetPlayerGabMoney(player, gapMoneysDic[player], endGap);

        //if(gapMoneyTweenRoutine == null)
        //{
        //    StartCoroutine(SetPlayerGapMoneyRoutine(player, player.Gap,endGap));
        //}
        //else
        //{

        //}

    }

    public void SetPlayerHaveMoney(GamePlayer player, long startHave, long EndHave)
    {
        if(player.roomSerial == cGlobalInfos.GetIntoRoomInfo_97().nSERIAL)
        {
            ResourceContainer.Get<ADMyInfoTag>("MyInfo").haveMoney = EndHave;
        }
        else
        {
            
        }

        player.Have = EndHave;
    }
    public void SetPlayerHaveMoney(GamePlayer player, long EndHave)
    {
        // var player = ResourcePool.Find<GamePlayer>(p => p.roomIdx == playerIndex);
        SetPlayerHaveMoney(player, player.Have, EndHave);
    }

    public void PlayAllInEffect(int playerIndex, bool bStaying, bool bIsMine)
    {
        // var tempBettingmanager = ResourceContainer.Get<ADChipBettingManager>();
        //var tempPos = deployPos;


        var pp = ResourceContainer.Get<GameUserPosition>(playerIndex);
        var player = ResourcePool.Find<GamePlayer>(p => p.roomIdx == playerIndex);
        //if(player != null)
        //{
        //}
            Debug.Log("AD_ALL_IN, player is null ? " + (player == null ? true : false));
        Debug.Log("AD_ALL_IN, player index is  " + playerIndex);
        if (player == null)
        {
            Debug.LogError("AD_ALL_IN, all in player is NULL, index is " + playerIndex);
            return;
        }


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

    #endregion



    #region android background issue related...

    [TestMethod(false)]
    public void TestTimeShow()
    {
        Debug.LogError("time is " + Time.time);
    }

    public float pausedTime;
    private bool bIsPaused = false;
    private bool bIsFocused = false;

    // private System.DateTime focusTime;
    private float focusTime;
    private float outStandardTime = 11f;


    // private System.DateTime backgroundTime;
    private float backgroundTime;


    private void OnApplicationPause(bool pause)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        // On Android, when the on-screen keyboard is enabled, it causes a OnApplicationFocus(false) event. 
        //Additionally, if you press "Home" at the moment the keyboard is enabled, the OnApplicationFocus() event is not called, but OnApplicationPause() is called instead.
        if (pause)
        {
            pausedTime++;
            if (bIsPaused == false)
            {
                Debug.LogError("Application is on pause");
                bIsPaused = true;
            }

            // focusTime = System.DateTime.Now.AddSeconds(outStandardTime);
            focusTime = Time.time + outStandardTime;
            // backgroundTime = System.DateTime.Now;
            backgroundTime = Time.realtimeSinceStartup;
            Debug.LogError("Application is on pause"
                + "focus time is " + focusTime
                + " background time is " + backgroundTime);

        }
        else
        {
            // pausedTime = 0;
            if (bIsPaused == true)
            {
                Debug.LogError("Application is not on pause");
                bIsPaused = false;
            }
        }
#endif
    }

    private void OnApplicationFocus(bool focus)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        //if (focus && bIsFocused && focusTime.CompareTo(System.DateTime.Now) == -1) //focus 가 크면 1 작으면 -1, (now가 크면 -1)
        if(focus && bIsFocused && focusTime.CompareTo(Time.realtimeSinceStartup) == -1 )
        {
            Debug.LogError("will be disconnected out time is " + focusTime
                + " background time is " + Time.realtimeSinceStartup);
            // SubGameSocket.End();

        }
        else
        {

        }

        if (focus)
        {
            if (bIsFocused == false)
            {
                Debug.LogError("Application is on focus "
                    + " ping request check time " + PacketManager.Instance.pingReqCheck);

                //Debug.LogError("background time is " + backgroundTime.ToString()
                //    + " current time is " + System.DateTime.Now.ToString());


                // PacketManager.Instance.pingReqCheck += Mathf.Abs((float)(backgroundTime - System.DateTime.Now).TotalSeconds);
                // Debug.LogError("added time " + (float)((backgroundTime - System.DateTime.Now).TotalSeconds));
                PacketManager.Instance.pingReqCheck += Math.Abs(backgroundTime - (Time.time + outStandardTime) );
                Debug.LogError("added time " + Math.Abs(backgroundTime - (Time.time + outStandardTime)) 
                    + " background time : " + backgroundTime
                    + " focus time : " + (Time.time + outStandardTime) );



                S_97_PING ping = new S_97_PING();
                ping.send();

                bIsFocused = true;
            }
        }
        else
        {
            if (bIsFocused == true)
            {
                Debug.LogError("Application is not on focus");
                bIsFocused = false;
            }
        }
#endif


    }

    #endregion

    #region delete it after stress test is done

    // public int myBettingCount = 0;
    public Dictionary<int, int> playersBettingCount = new Dictionary<int, int>(capacity: 11);

    public void IncrementCounts(int playerIndex)
    {
        if (playersBettingCount.ContainsKey(playerIndex) == false)
        {
            playersBettingCount.Add(playerIndex, 0);
        }
        else
        {
            playersBettingCount[playerIndex]++;
        }
    }

    [TestMethod(false)]
    public void TestEmote(int index, int iterationCount)
    {
        // 0 ~ 11 sprite
        // 12 ~ 18 spine
        // EmoticonReq(index);

        var tempEmote = GameEmoticon.Instance;
        // StartCoroutine(tempEmote.TestEmoteContinuously(index, iterationCount));
    }

    #endregion

    #region betting performance related
    public void StartArbitRoutine()
    {
        StartCoroutine(BettingArbitRoutine());
    }
    public IEnumerator BettingArbitRoutine()
    {
        yield return null;
    }
    #endregion

    #region compile noticing... no need to call this method
    public void AddCallendars()
    {
        // for using system.datetime... without this, error message will pop up on log
        // ArgumentOutOfRangeException: Not a valid calendar for the given culture.

        // new System.Globalization.GregorianCalendar();
        // new System.Globalization.PersianCalendar();
        // new System.Globalization.UmAlQuraCalendar();
        new System.Globalization.ThaiBuddhistCalendar();
    }
    #endregion


    #region
    //public void HandleBackgroundMusic(float volume)
    //{
    //    Sound.Instance.asBG.volume = 0.5f;
    //}
    public void HandleBackgroundMusicWithDelay(float volume, float time)
    {
        //        배경음 기본 사운드 출력 볼륨 50 %
        //접시가 나타나고 오픈하기 1초전 볼륨 10 % 로 줄이기
        //  결과가 끝나고 새로운 게임이 시작되면 볼륨 50 % 로 되돌아가기

        // Sound.Instance.asBG.volume = 0.5f;

        // TimeContainer.ContainClear(time._id);
        
        if(volume == Sound.Instance.asBG.volume)
        {
            return;
        }

        TimeContainer.ContainClear("MUSIC_HANDLE_TIME");
        TimeContainer musicHandleTime = new TimeContainer("MUSIC_HANDLE_TIME", time);
        Sound.Instance.asBG.VolumeTween(volume, musicHandleTime);
    }
    #endregion

}
