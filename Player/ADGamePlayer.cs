//#if !UNITY_EDITOR
//#define DEBUG_PURPOSE
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ���� ���õȰ� ���⿡ �ۼ����� ����. ����κи�
/// </summary>
public class ADGamePlayer : PoolItemBase
{
    //���� �ʿ� ������ ����. �� serial �ִ����ڼ��� ���ϴ� ��ġ�� �ٷ� ���� �� ����.GameUtils.Convert ����
    public int roomSerial = -1;
    public int roomIdx = -1;
    public RawImage userPhoto;
    public TextMeshProUGUI lbNickname;
    public TextMeshProUGUI lbHave;
    public TextMeshProUGUI lbGab;
    public GameObject exit;

    public Texture userDefaultPhoto; //test\

    public UGUIButtonChildImageSwap vip;

    [SerializeField]
    bool isName = true;

    private long _atEnterHave;
    private long _have;
    private long _gab;
    public long Have
    {
        get
        {
            return _have;
        }
        set
        {
            var prev = _have;
            _have = value;
            lbHave.StopAllCoroutines();
            lbHave.NumberTween(prev, _have, g => g.ToStringWithKMB(false, 3, false), GameUtils.st_globalTweenTime);
        }
    }
    [TestMethod]
    public void SetHave(long gold)
    {
        Have = gold;
    }

    public long Gap
    {
        get
        {
            return _gab;
        }
        set
        {
            //if (roomIdx != 0 || isName)
            //    return;
            if (roomIdx == 0)
            {
                return;
            }

            var prev = _gab;
            _gab = value;
            if (lbGab == null)
                return;
            if (!isName)
            {
                lbGab.StopAllCoroutines();
                lbGab.NumberTween(prev, _gab, g => g.ToStringWithKMB(), GameUtils.st_globalTweenTime);
                if (_gab == 0)
                    lbGab.color = Color.white;
                else
                    lbGab.ColorTween(_gab > 0 ? plus : minus, GameUtils.st_globalTweenTime);
            }
            else
            {
                lbGab.text = _gab.ToStringWithKMB();
                if (_gab == 0)
                    lbGab.color = Color.white;
                else
                    lbGab.ColorTween(_gab > 0 ? plus : minus, GameUtils.st_globalTweenTime);
            }
        }
    }

    public string Nick => _nick;
    private string _nick;
    public Color plus = Color.green;
    public Color minus = Color.red;
    public bool isMale;
    public void OnPop(GamePlayerInitData data)
    {
        Debug.Log("[GamePlayer.OnPop] nick : " + data.nick + ", photoURL : " + data.phoroURL);
        _nick = data.nick;
        lbNickname.text = data.nick;
        _have = data.have;
        _gab = data.gab;
        lbGab.text = _gab.ToStringWithKMB(false, 3, false);
        lbHave.text = _have.ToStringWithKMB(false, 3, false);

        roomSerial = data.serial;
        roomIdx = data.roomIdx;
        isMale = data.isMale;
        exit.SetActive(data.exit);

        DBManager.SetVIPImage(vip, data.vip);

        if (!string.IsNullOrEmpty(data.phoroURL))
        {
            var photo = new st00_PHOTO01();
            photo.szPHOTO = data.phoroURL;

            Texture defaultTex = DBManager.Instance.GetDefaultSprite(isMale ? 0 : 1,_nick);
            userPhoto.texture = defaultTex;
            WooriImageCache.Instance.LoadProfileTexture(photo, t => userPhoto.texture = t, defaultTex);
        }
        else
        {
            userPhoto.texture = DBManager.Instance.GetDefaultSprite(isMale ? 0 : 1,_nick);
        }
    }

    public Coroutine SetView(string nick, long gold, string photoUrl, int serial, TimeContainer time)
    {
        _nick = nick;
        lbNickname.text = nick;
        this.lbHave.text = gold.ToStringWithKMB();

        WooriImageCache.Instance.LoadTexture(photoUrl, t => userPhoto.texture = t, DBManager.Instance.GetDefaultSprite(isMale ? 0 : 1,_nick));
        return MoveToActivePosition(time);
    }

    [TestMethod]
    public void SetupTestUser(string nick = "testNick", long gold = 1000000)
    {
        SetView(nick, gold, "te", 0, 1f);
    }

    public Coroutine MoveToActivePosition(TimeContainer tc = null)
    {
        if (tc == null)
            tc = 1f;
        var pad = ResourceContainer.Get<GameUserPosition>(roomIdx);
        return this.Move(pad.userPos.position, tc);
    }

    public Coroutine MoveToHidePosition(TimeContainer tc = null)
    {
        if (tc == null)
            tc = 1f;
        var pad = ResourceContainer.Get<GameUserPosition>(roomIdx);
        return this.Move(pad.userPos.position, tc);
    }

    private bool isRoutining = false;

    public void SwitchNameAndGap()
    {
        if (roomIdx != 0)
        {
            OnPhoto();
            return;
        }

        StartCoroutine(SwitchRoutine());
    }

    public void SetGap()
    {
        isName = false;
        lbGab.alpha = 1;
        lbNickname.alpha = 0f;
    }

    IEnumerator SwitchRoutine()
    {
        if (isRoutining)
            yield break;

        isRoutining = true;
        if (isName)
        {
            isName = false;
            yield return lbNickname.AlphaTween(0f, 0.2f);
            yield return lbGab.AlphaTween(1f, 0.2f);
        }
        else
        {
            isName = true;
            yield return lbGab.AlphaTween(0f, 0.2f);
            yield return lbNickname.AlphaTween(1f, 0.2f);
        }

        isRoutining = false;
    }

    public void Clear()
    {
        roomSerial = -1;
        _nick = "";
        userPhoto.texture = DBManager.Instance.GetDefaultSprite(isMale ? 0 : 1,_nick);
        userPhoto.color = Color.white;
    }

    public override void Back()
    {
        ResourcePool.Push(this);
    }

    public void OnPhoto()
    {
        Sound.Instance.EffPlay("E_CLICK");
        Common_SendPk.Req_97_REQ_USERINFO(_nick);
    }

    public TextMeshProUGUI chat;
    public RectTransform chatBalloon;
    bool isView = false;
    public Coroutine chatRoutine = null;


    [TestMethod]
    public void Chat(string text)
    {
        if (chatRoutine != null)
            StopCoroutine(chatRoutine);

        chatRoutine = StartCoroutine(ChatRoutine(text));
    }

    public int chatLimit = 50;

    IEnumerator ChatRoutine(string text)
    {
        chatBalloon.gameObject.SetActive(true);
        if (chatLimit < text.Length)
        {
            text = text.Remove(chatLimit - 3);
            text = text + "...";
        }

        chat.text = text;
        yield return null;
        yield return null;
        var size = chatBalloon.sizeDelta;
        size.y = chat.renderedHeight + 43f;
        chatBalloon.sizeDelta = size;
        var img = chatBalloon.GetComponent<Image>();

        yield return img.AlphaTween(1f, 0.3f, true);
        yield return img.Wait(2f);
        yield return chat.AlphaTween(0f, 0.3f, true);
        chatBalloon.gameObject.SetActive(false);
        chatRoutine = null;
    }

    [TestMethod]
    public void SetUserImageDefault()
    {
        userPhoto.texture = userDefaultPhoto;
    }

    public void OnPointerDown()
    {
        lbNickname.SetAlpha(1f);
        lbGab.SetAlpha(0f);
    }

    public void OnPointerUp()
    {
        lbNickname.SetAlpha(0f);
        lbGab.SetAlpha(1f);
    }

}

// #endif