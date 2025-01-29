using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiPacketManager : MonoBehaviour, IBatchUpdate
{
    public Dictionary<int, PacketHandler> packetListDic = new Dictionary<int, PacketHandler>();
    private GameCodeChecker gameCodeChecker;

    [HideInInspector]
    public bool _bBlockSocket = false;
    [HideInInspector]
    public float pingTickCount = 0f;
    [HideInInspector]
    public float pingReqCheck = 0f;

    [HideInInspector]
    public bool actionDisconnect = false;

    public GameObject p_DisconnectMsg;
    public bool isDisconnect = false;


    [TestMethod]
    public void TestShow()
    {
        foreach (var item in packetListDic)
        {
            Debug.Log("item object name is " + item.Value
                + " number is " + item.Value.GetNumber());
        }
    }

    public void Start()
    {
        GameCodeCheckerSet();
        Init();
        //CheckDic();
        // UpdateManager.Instance.RegisterSlicedUpdate(this, UpdateManager.UpdateMode.BucketA);
    }
    //private void OnDestroy()
    //{
    //    UpdateManager.Instance.DeregisterSlicedUpdate(this);
    //}
    private void FixedUpdate()
    {
        if (isDisconnect)
            return;

        _proc_NetState();

        if (!PacketManager.Instance._bBlockSocket)
            Socket_Process();

        PingProcess();
    }

    void Update()
    {
        //if (PacketTracker.IsExistsAndEnableToIssue())
        //{
        //    return;
        //}
        //fSysTime += Time.deltaTime;

        
        //if (isDisconnect)
        //    return;

        //_proc_NetState();

        //if (!PacketManager.Instance._bBlockSocket)
        //    Socket_Process();

        //PingProcess();
    }

    public void Init()
    {
        foreach (var packet in GetComponentsInChildren<PacketHandler>())
        {
            if (!packetListDic.ContainsKey(packet.GetNumber()))
                packetListDic.Add(packet.GetNumber(), packet);
            else
                Debug.LogError("Exist same key!!\npacket : " + packet);
        }
    }

    public void GameCodeCheckerSet()
    {
        foreach (var checker in FindObjectsOfType<GameCodeChecker>())
        {
            checker.gameObject.SetActive(checker.check(cGlobalInfos.GetGameCode()));
        }
    }

    void Socket_Process()
    {
        if (!SubGameSocket.GetPacket(SubGameSocket.m_iData, SubGameSocket.m_bytebuffer))
            return;

        int pkNumber = SubGameSocket.m_iData[0];

        //if (SubGameSocket.m_iData[0] == 990018)
        //    pkNumber = 2000;

        PacketHandler packetHandler;
        if (packetListDic.TryGetValue(pkNumber, out packetHandler))
            packetHandler.Func();
        else
            throw new Exception("해당하는 PK 메서드가 없습니다.\npkNumber : " + pkNumber.ToString());
    }

    public void _proc_NetState()
    {
        if (SubGameSocket.m_bdisMessage)
            DisconnectMsgOnoff(true);
        if (SubGameSocket.m_bfaileMsg)
            DisconnectMsgOnoff(true);
    }

    public void PingProcess()
    {

        PacketManager.Instance.pingTickCount += Time.deltaTime;
        PacketManager.Instance.pingReqCheck += Time.deltaTime;

        if (PacketManager.Instance.pingTickCount < 1f) return;
        PacketManager.Instance.pingTickCount = 0f;

        if (PacketManager.Instance.pingReqCheck > 5f)
        {
            Debug.Log("Ping Disconnect");
            SubGameSocket.End();
            //gameObject.SetActive(false);//이렇게 추가해도 괜찮을듯.
            DisconnectMsgOnoff(true);
            return;
        }
        else
        {
            req_Ping();
        }
    }

    [TestMethod]
    public void TestDisconnect()
    {
        PacketManager.Instance.pingReqCheck += 10f;
    }

    void req_Ping()
    {
        S_97_PING ping = new S_97_PING();
        ping.send();
    }

    public void DisConnectGotoLobby()
    {
        PacketManager.Instance._bBlockSocket = true;
        SubGameSocket.End();
        //대기실로 강제 전환.. 
        cSceneManager.LoadLevel("Lobby", 0.3f);
    }

    public void DisconnectMsgOnoff(bool onoff)
    {
        if (p_DisconnectMsg == null)
        {
            isDisconnect = true;
            if (onoff == true)
            {
                GamePopupManager.Open<GameMessagePopup, GameMessagePopupData>(new GameMessagePopupData()
                {
                    title = STM.Get("T_DIS_TITLE"),
                    contents = STM.Get("T_DIS_MESSAGE"),
                    isYN = false,
                    onOk = DisconnectMsgYes,

                });
                GamePopupManager.SetBackClose(false);

            }

            return;
        }

        if (onoff)
        {
            p_DisconnectMsg.SetActive(true);
        }
        else
        {
            p_DisconnectMsg.SetActive(false);
        }
    }

    public void DisconnectMsgYes()
    {
        //Sound.Instance.EffPlay("E_CLICK"); // 씬에서 버튼 클릭시 사운드 나오도록 추가해줘야함
        DisConnectGotoLobby();
        //gameObject.SetActive(true);
        DisconnectMsgOnoff(false);
    }

    public void CheckDic()
    {
        foreach (var packet in packetListDic)
        {
            Debug.Log("pkNumber : " + packet.Key + "\nname : " + packet.Value);
        }
    }

    public void BatchUpdate()
    {
        if (isDisconnect)
            return;

        _proc_NetState();

        if (!PacketManager.Instance._bBlockSocket)
            Socket_Process();

        PingProcess();
    }
}
