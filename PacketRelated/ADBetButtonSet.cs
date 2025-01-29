using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ADBetButtonSet : PacketHandler
{
    public override int GetNumber()
    {
        return (int)ANIMALDICE_PK.R_09_BETBTN;
    }
    public override void Func()
    {
        var rec = new R_09_BETBTN(SubGameSocket.m_bytebuffer);
        // Debug.Log("[R_09_BETBTN]");
        var bTempAllDisabled = true;
        foreach (var button in rec.lBTNS)
        {
            Debug.Log("[R_09_BETBTN] button is " + ((eAD_BUTTONLIST)button.nINDEX).ToString()
                + " state is " + button.nSTATE);

            if(button.nSTATE != 2)
            {
                bTempAllDisabled = false;
            }


        }
        //if(bTempAllDisabled)
        //{
        //    ResourceContainer.Get<ADChipBettingManager>().SetEnableBettingBoards(false);
        //}

        // public List<stBUTTONSET> 	lBTNS;

        // public class stBUTTONSET
        // 
        // public int nINDEX;
        // public int nSTATE;
        // public string szMSG;
        // public stINT64 stMONEY;
        // public int nHIGHLIGHT;

        
        // one time only, ante setting
        if(ResourceContainer.Get<ADAnteDependSetting>().bHasBeenSet == false)
        {
            List<long> buttonMoneyList = new List<long>(capacity: 5);
            // buttonMoneyList.Add(-1); // this goes for eAD_BUTTONLIST._BTN_NONE

            foreach (var button in rec.lBTNS)
            {
                Debug.Log("button moeny is " + (long)button.stMONEY
                    + " button kind is " + ((eAD_BUTTONLIST)button.nINDEX).ToString());
                if ((int)eAD_BUTTONLIST._BTN_BETTING_1 <= button.nINDEX ||
                    button.nINDEX <= (int)eAD_BUTTONLIST._BTN_BETTING_4)
                {

                    buttonMoneyList.Add((long)button.stMONEY);
                }
            }
            ResourceContainer.Get<ADAnteDependSetting>().SetSmallChipAndButtons(buttonMoneyList);
            ResourceContainer.Get<ADAnteDependSetting>().bHasBeenSet = true;


            #region set chip entity setting and store it

            var spawner = ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>();
            var anteSetting = ResourceContainer.Get<ADAnteDependSetting>();
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

            #region check my money and enable betting board
            if(ResourceContainer.Get<ADMyInfoTag>("MyInfo").haveMoney >= 
                ResourceContainer.Get<ADAnteDependSetting>().chipValueInThisRoom[eAD_BUTTONLIST._BTN_BETTING_1]
                && ResourceContainer.Get<ADBettingTimeCounter>().GetNumber() > 1)
            {
                ResourceContainer.Get<ADChipBettingManager>().SetEnableBettingBoards(true);
            }
            #endregion

            //if (bTempAllDisabled == false)
            //{
            //    ResourceContainer.Get<ADChipBettingManager>().SetEnableBettingBoards(true);
            //}

        }

        ResourceContainer.Get<ADChipBettingManager>().ButtonSet(rec.lBTNS);


        
        
    }
}

public enum eAD_BTN_STATE
{
    _BTN_NO_USE = 0,    //버튼을 사용안함
    _BTN_ENABLE = 1,    //활성화
    _BTN_DISABLE,       //비활성화
    _BTN_HIDE           //숨긴다..
};