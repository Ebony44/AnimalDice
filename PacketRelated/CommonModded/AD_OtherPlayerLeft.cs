using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Wooriline;

public class AD_OtherPlayerLeft : PacketHandler
{
    public override int GetNumber()
    {
        return (int)COMMON_PK.R_97_OTHERPLAYERLEFT;
    }
    public override void Func()
    {
        var rec = new R_97_OTHERPLAYERLEFT(SubGameSocket.m_bytebuffer);
        Debug.Log("[R_97_OTHERPLAYERLEFT]");
        if (GameUtils.st_maxPlayer == -1 || GameUtils.CheckMySerial(-1))
        {
            Debug.Log("loading isn't over yet");
            return;
        }
        int outIdx = rec.stOUT.nSERIAL.ConvertToRoomIdx();
        Debug.Log("OtherOut : " + rec.stOUT.szID + ", roomIdx" + outIdx);

        if (outIdx == 0)
            PacketManager.Instance.DisConnectGotoLobby();

        if (outIdx == -1) return;

        ActionPlayer.Play(new ADExit(outIdx));
        //ActionEmoticonManager.Instance.EmoticonCancle(outIdx);
    }
}

public class ADExit : IAction
{
    public string Log => "";

    public string ID => "Exit";

    public List<string> CancelID => null;

    int _idx;
    TimeContainer _move;
    public ADExit(int roomIdx)
    {
        _idx = roomIdx;
        _move = new TimeContainer("userOut" + _idx, 0.5f);
    }

    public IEnumerator ActionRoutine()
    {
        var player = ResourcePool.Find<GamePlayer>(p => p.roomIdx == _idx);
        var pp = ResourceContainer.Get<GameUserPosition>(_idx);

        var tempTimeCount = TimeContainer.GetActiveContainerCount("userOut" + _idx);
        Debug.Log("LEFTING, Tc count " + tempTimeCount);
        if (tempTimeCount != 0)
        {
            yield break;
        }

        Debug.Log("LEFTING, Tc is " + "userOut" + _idx);
        TimeContainer.ContainClear("userOut" + _idx);


        // yield return player.Move(pp.hideUserPos, _move);

        // player.gameObject.GetComponent<Image>().SetAlpha(1f, true);
        #region obsolete

        //bool tempIsPlaying = false;
        //if (TimeContainer.st_allList.ContainsKey("userOut"))
        //{
        //    foreach (var item in TimeContainer.st_allList["userOut"])
        //    {
        //        if(item.t < 1)
        //        {
        //            tempIsPlaying = true;
        //        }
        //    } 
        //}
        //if(tempIsPlaying == false)
        //{
        //    yield return player.gameObject.GetComponent<Image>().AlphaTween(0f, _move, true);
        //    player.roomSerial = -1;
        //    // player.userPhoto.texture = player.userDefaultPhoto;
        //    TimeContainer.ContainClear("userOut");
        //    player.userPhoto.SetAlpha(0f, true);
        //    player.transform.localPosition = ResourceContainer.Get<GameObject>("ADUserOutPos").transform.localPosition;
        //    // player.userPhoto.gameObject.SetActive(false);
        //}

        #endregion

        // player.gameObject.GetComponent<Image>().SetAlpha(1f);
        // yield return null;
        // player.gameObject.GetComponent<Image>().SetAlpha(1f, true);



        yield return player.gameObject.GetComponent<Image>().AlphaTween(0f, _move, true);
        player.roomSerial = -1;
        // player.userPhoto.texture = player.userDefaultPhoto;
        Debug.Log("LEFTING, Tc is " + "userOut" + _idx);
        TimeContainer.ContainClear("userOut" + _idx);
        player.userPhoto.SetAlpha(0f, true);
        player.transform.localPosition = ResourceContainer.Get<GameObject>("ADUserOutPos").transform.localPosition;

        Debug.Log("userOut end");
    }

    public IEnumerable<TimeContainer> GetAllTimeContainerList()
    {
        yield return _move;
    }
}
