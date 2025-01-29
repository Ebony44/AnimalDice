using ReuseScroller;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class st09_ModHistory : st09_HISTORY
{
    public int nWINTYPE;
    public st00_INT64 stWINMONEY;
}

public class ADHistoryItem : BaseCell<st09_HISTORY>
{
    [Header("Params to Change")]
    public Image[] diceDisplays;

    public Image winCrownDisplay;
    public TextMeshProUGUI winMoneyLabel;

    public Image tieOrNoBetImage;


    [Header("Params' stored")]

    public Sprite[] diceSprites; // 0~6, 0 is none
    public Sprite[] winCrownSprites; // 0~6, 0 is win, 1 is tie, 2 is lose

    public int winType;
    public long winMoney;

    

    public override void UpdateContent(st09_HISTORY item)
    {
        //for (int i = 0; i < diceDisplays.Length; ++i)
        //{
        //}
        
        diceDisplays[0].sprite = diceSprites[item.nDICE1];
        diceDisplays[1].sprite = diceSprites[item.nDICE2];
        diceDisplays[2].sprite = diceSprites[item.nDICE3];
        // item.stGAME_IDX
        winMoney = item.stRESULTMONEY;
        // winMoneyLabel.text = ((long)item.stRESULTMONEY).ToStringWithKMB();
        winMoneyLabel.text = ((long)item.stRESULTMONEY).ToStringWithKMB(false, 3, false);

        winCrownDisplay.gameObject.SetActive(true);
        winMoneyLabel.gameObject.SetActive(true);
        tieOrNoBetImage.gameObject.SetActive(false);
        if (winMoney > 0)
        {
            winMoneyLabel.color = Color.green;
            winCrownDisplay.sprite = winCrownSprites[0];
        }
        else if(winMoney == 0)
        {
            winMoneyLabel.text = string.Empty;
            // winCrownDisplay.sprite = winCrownSprites[1];
            winCrownDisplay.gameObject.SetActive(false);
            winMoneyLabel.gameObject.SetActive(false);
            tieOrNoBetImage.gameObject.SetActive(true);
        }
        else
        {
            winMoneyLabel.color = Color.red;
            winCrownDisplay.sprite = winCrownSprites[2];
        }
        
        



        // throw new System.NotImplementedException();
    }
    //public void UpdateContent(st09_HISTORY item, int winType, st00_INT64 winMoney)
    //{
    //    //for (int i = 0; i < diceDisplays.Length; ++i)
    //    //{
    //    //}

    //    diceDisplays[0].sprite = diceSprites[item.nDICE1];
    //    diceDisplays[1].sprite = diceSprites[item.nDICE2];
    //    diceDisplays[2].sprite = diceSprites[item.nDICE3];

    //    winMoneyLabel.text = ((long)winMoney).ToStringWithKMB();


    //    // throw new System.NotImplementedException();
    //}
}
