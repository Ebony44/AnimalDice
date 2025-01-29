using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ADClientAuto : ResourceItemBase
{
    public bool bIsAuto = false;
    public Coroutine autoRoutine;
    public Rotation rotationScript;
    public Image autoAImage;
    public Image autoRotateImage;

    public List<Sprite> autoPrepSprite;

    public float minBettingInterval = 0.02f;
    public float maxBettingInterval = 0.03f;

    

    public void OnAutoButtonClick()
    {
        
        bIsAuto = !bIsAuto;
        
        if(bIsAuto == true)
        {
            if (autoRoutine != null)
            {
                StopCoroutine(autoRoutine);
            }
            autoRoutine = StartCoroutine(StartAutoRoutine(1,minBettingInterval));
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

    public IEnumerator StartAutoRoutine(int iterationCount = 1, float bettingMinInterval = 0.3f)
    {
        yield return null;
        var betPlaceList = ResourceContainer.Get<ADChipBettingManager>().betPlaceSizeList;

        while(bIsAuto)
        {
            
            var tempRandomFloat = Random.Range(bettingMinInterval, maxBettingInterval);
            var tempRandomBetPlace = betPlaceList[Random.Range(0, betPlaceList.Count)].betPlace;
            // var tempRandomBetChipKind = Random.Range((int)eAD_BUTTONLIST._BTN_BETTING_1, (int)eAD_BUTTONLIST._BTN_BETTING_5);
            var tempRandomBetChipKind = Random.Range((int)eAD_BUTTONLIST._BTN_BETTING_1, (int)eAD_BUTTONLIST._BTN_BETTING_2);

            var tempBetString = (tempRandomBetPlace.ToString() + "_" + ((int)tempRandomBetPlace).ToString());

            yield return new WaitForSeconds(tempRandomFloat);
            for (int i = 0; i < iterationCount; i++)
            {
                ResourceContainer.Get<ADChipBettingManager>().currentButtonIndex = (eAD_BUTTONLIST)tempRandomBetChipKind;
                ResourceContainer.Get<ADChipBettingManager>().OnBetting(tempBetString);
                // yield return new WaitForSeconds(0.02f);
            }
            // ResourceContainer.Get<ADChipBettingManager>().Req_Betting(tempRandomBetPlace, (eAD_BUTTONLIST)tempRandomBetChipKind);
            yield return null;
        }
    }

    [TestMethod(false)]
    public void StartReqTabRoutine(int iterationCount)
    {
        bIsAuto = !bIsAuto;

        if (bIsAuto == true)
        {
            if (autoRoutine != null)
            {
                StopCoroutine(autoRoutine);
            }
            autoRoutine = StartCoroutine(StartReqTabAutoRoutine(iterationCount));
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

    public IEnumerator StartReqTabAutoRoutine(int iterationCount)
    {
        yield return null;
        var tempPanel = ResourceContainer.Get<GamePanelGiftQuest>();

        while(bIsAuto)
        {
            var tempRandomFloat = Random.Range(minBettingInterval, maxBettingInterval);
            yield return new WaitForSeconds(tempRandomFloat);
            for (int i = 0; i < iterationCount; i++)
            {
                // Common_SendPk.Req_97_REQ_RECLIST_TAB(tempPanel.questTabIdx);
                //SendAuto();
            }

        }
    }
    public void SendAuto()
    {
        Debug.Log("S_97_Auto");

        S_97_REQ_AUTOSET sAutoSet = new S_97_REQ_AUTOSET();
        sAutoSet.nAUTOSTATE = 0;
        sAutoSet.send();
    }
}
