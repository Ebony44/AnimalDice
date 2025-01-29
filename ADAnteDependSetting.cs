using OneLine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ADAnteDependSetting : ResourceItemBase
{
    #region obsolete variables
    #region can be Asset Bundle dependent...
    public List<Sprite> smallChipSprites;
    public List<Sprite> chipButtonsSprites;
    #endregion

    #region obsolete, currently not used.
    public List<long> anteSetList = new List<long> { 500, 1000, 5000, 10000, 50000, 100000 };
    public List<long> anteModList = new List<long> { 1, 2, 10, 20};
    #endregion
    #region variables for using from outside scripts
    public int currentAnteModifier = -1;

    public Dictionary<eAD_BUTTONLIST, long> chipValueInThisRoom = new Dictionary<eAD_BUTTONLIST, long>() {
        { eAD_BUTTONLIST._BTN_NONE, -1 },
        { eAD_BUTTONLIST._BTN_BETTING_1, -1 },
        { eAD_BUTTONLIST._BTN_BETTING_2, -1 },
        { eAD_BUTTONLIST._BTN_BETTING_3, -1 },
        { eAD_BUTTONLIST._BTN_BETTING_4, -1 },
        // { eAD_BUTTONLIST._BTN_BETTING_5, -1 },
    };
    #endregion

    #endregion

    public bool bHasBeenSet = false;
    public Dictionary<long, Sprite> smallChipSpritesDic = new Dictionary<long, Sprite>(20);
    public Dictionary<long, Sprite> chipButtonSpritesDic = new Dictionary<long, Sprite>(20);

    [OneLineWithHeader]
    [HideLabel]
    public List<ADBetButtonDataStore> smallChipDatas;

    [OneLineWithHeader]
    [HideLabel]
    public List<ADSmallChipDataStore> chipButtonDatas;


    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        foreach (var smallChip in smallChipDatas)
        {
            smallChipSpritesDic.Add(smallChip.buttonMoney, smallChip.chipButtonSprite);
        }
        foreach (var chipButton in chipButtonDatas)
        {
            chipButtonSpritesDic.Add(chipButton.buttonMoney, chipButton.smallChipSprite);
        }
        // smallChipSpritesDic.Add()

        if (cGlobalInfos.GetIntoRoomInfo_97() != null)
        {

            // var tempCurrentAnte = cGlobalFunctions.Get_INT64(cGlobalInfos.GetIntoRoomInfo_97().stANTE.nHI, cGlobalInfos.GetIntoRoomInfo_97().stANTE.nHI);

            var tempCurrentAnte = cGlobalInfos.GetIntoRoomInfo_97().stMIN_BETMONEY;

            //currentAnteModifier = anteSetList.FindIndex(x => x == tempCurrentAnte);
            //if (currentAnteModifier != -1)
            //{
            //    SetupChipValueDependOnAnte(currentAnteModifier);
                
            //    CoroutineChain.Start
            //        .Wait(0.02f)
            //        .Call(() => SetupSpritesDependOnAnte(currentAnteModifier));
            //}
        }
        if(currentAnteModifier == -1)
        {
            // SetupSpritesDependOnAnte(2);
        }




    }
    #region obsolete
    //[TestMethod]
    //public void SetupSpritesDependOnAnte(int anteSetIndex)
    //{
    //    SetupCurrentAnte(anteSetIndex);
    //    var tempMod = 4 * anteSetIndex;
    //    ResourceContainer.Get<Image>("BettingChipButtonX1").sprite = chipButtonsSprites[tempMod + 0];
    //    ResourceContainer.Get<Image>("BettingChipButtonX2").sprite = chipButtonsSprites[tempMod + 1];
    //    ResourceContainer.Get<Image>("BettingChipButtonX10").sprite = chipButtonsSprites[tempMod + 2];
    //    ResourceContainer.Get<Image>("BettingChipButtonX20").sprite = chipButtonsSprites[tempMod + 3];

    //    ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().chipSpriteList[1] = smallChipSprites[tempMod + 0];
    //    ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().chipSpriteList[2] = smallChipSprites[tempMod + 1];
    //    ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().chipSpriteList[3] = smallChipSprites[tempMod + 2];
    //    ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().chipSpriteList[4] = smallChipSprites[tempMod + 3];

    //    // ResourceContainer.Get<ResourceTag>("BettingChipButtonX2").GetComponent<Image>().sprite = chipButtonsSprites[1];
    //    // ResourceContainer.Get<ResourceTag>("BettingChipButtonX10").GetComponent<Image>().sprite = chipButtonsSprites[2];
    //    // ResourceContainer.Get<ResourceTag>("BettingChipButtonX20").GetComponent<Image>().sprite = chipButtonsSprites[3];

    //}

    //[TestMethod]
    //public void SetupChipValueDependOnAnte(int anteSetIndex)
    //{
    //    SetupCurrentAnte(anteSetIndex);
    //    for(int i = 1;i<chipValueInThisRoom.Count;++i)
    //    {
    //        chipValueInThisRoom[eAD_BUTTONLIST._BTN_NONE + i] = anteSetList[currentAnteModifier] * anteModList[(i - 1)]; // x1, x2, x10, x20


    //    }

    //}
    //public void SetupCurrentAnte(int anteSetIndex)
    //{
    //    if (cGlobalInfos.GetIntoRoomInfo_97() != null)
    //    {
    //        // var tempCurrentAnte = cGlobalFunctions.Get_INT64(cGlobalInfos.GetIntoRoomInfo_97().stANTE.nHI, cGlobalInfos.GetIntoRoomInfo_97().stANTE.nHI);

    //        var tempCurrentAnte = cGlobalInfos.GetIntoRoomInfo_97().stMIN_BETMONEY;

    //        currentAnteModifier = anteSetList.FindIndex(x => x == tempCurrentAnte);
            
    //    }
    //    else
    //    {
    //        currentAnteModifier = anteSetIndex;
    //    }
    //}
#endregion

    //static public long Get_INT64(long nHiMoney, long nLoMoney)
    //{
    //    return nHiMoney * 100000000 + nLoMoney;
    //}

    //static public int Get_HighMoney(long lMoney)
    //{
    //    return (int)(lMoney / 100000000);
    //}


    // all above methods all obsolete...
    public void SetSmallChipAndButtons(List<long> buttonMoney)
    {
        ResourceContainer.Get<Image>("BettingChipButtonX1").sprite = chipButtonSpritesDic[buttonMoney[0]];
        ResourceContainer.Get<Image>("BettingChipButtonX2").sprite  = chipButtonSpritesDic[buttonMoney[1]];
        ResourceContainer.Get<Image>("BettingChipButtonX10").sprite = chipButtonSpritesDic[buttonMoney[2]];
        ResourceContainer.Get<Image>("BettingChipButtonX20").sprite = chipButtonSpritesDic[buttonMoney[3]];

        if(smallChipSprites.Count == 0)
        {
            smallChipSprites.Add(smallChipSpritesDic[buttonMoney[0]]);
            smallChipSprites.Add(smallChipSpritesDic[buttonMoney[1]]);
            smallChipSprites.Add(smallChipSpritesDic[buttonMoney[2]]);
            smallChipSprites.Add(smallChipSpritesDic[buttonMoney[3]]);
        }
        else
        {

            smallChipSprites[0] = smallChipSpritesDic[buttonMoney[0]];
            smallChipSprites[1] = smallChipSpritesDic[buttonMoney[1]];
            smallChipSprites[2] = smallChipSpritesDic[buttonMoney[2]];
            smallChipSprites[3] = smallChipSpritesDic[buttonMoney[3]];
        }

        // var tempModValue = eAD_BUTTONLIST._BTN_NONE;
        
        for (int i = 0; i < buttonMoney.Count; i++)
        {
            var tempModValue = (i + eAD_BUTTONLIST._BTN_BETTING_1);
            Debug.Log("chip value in this room " + buttonMoney[i]);
            chipValueInThisRoom[tempModValue] = buttonMoney[i];
        }

    }

    [TestMethod]
    public void ShowCurrentChipValues()
    {
        foreach (var chip in chipValueInThisRoom)
        {
            Debug.Log("chip is " + chip.Key
                + " chip value is " + chip.Value);
        }
    }
}
[System.Serializable]
public class ADBetButtonDataStore
{
    public long buttonMoney;
    public Sprite chipButtonSprite;
}
[System.Serializable]
public class ADSmallChipDataStore
{
    public long buttonMoney;
    public Sprite smallChipSprite;

}