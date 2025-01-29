using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ADBettingBoardTag : ResourceTag
{
    public eADBetPlace bettingPlace;

    public GameObject myBetTotalTextObject;
    public TextMeshProUGUI myBettingTotalText;

    public Image bettingBoard;

    public Image highlightBettingBoard;

    public Image myBettingLabel;

    // public eAD_BettingPlaceSize bettingPlaceSize;

    new void Awake()
    {
        base.Awake();
        EnableTextObject(false);
        AlphaSetHighlightObject(0f);

        GetComponent<Button>().interactable = false; // initially false

    }
    public void EnableTextObject(bool bEnabled)
    {
        myBetTotalTextObject.SetActive(bEnabled);

        TimeContainer.ContainClear("bettingBoardAlphaTween");
        // TimeContainer t1 = new TimeContainer("bettingBoardAlphaTween", 0.01f);
        // myBetTotalTextObject.GetComponent<Image>().AlphaTween( 1f , t1, true);
        
        //myBetTotalTextObject.GetComponent<Image>().SetAlpha(0f, true);
        myBettingLabel.SetAlpha(0f, true);

    }
    public void AlphaSetHighlightObject(float alphaValue)
    {
        // highlightBettingBoard.gameObject.SetActive(bEnabled);
        highlightBettingBoard.SetAlpha(alphaValue);
    }
    public void SetText(string text)
    {
        myBettingTotalText.text = text;
    }

}
