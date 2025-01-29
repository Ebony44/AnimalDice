// Obsolete, dont use this class...

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ADBettingManager : ResourceItemBase
{
    // TODO: move every method, variables into ADChipmanager.cs

    #region variables for sending
    // public int nIDX;
    // public stINT64 stGAME_IDX;
    // public int nBETPOS;
    // public int nBTNIDX;

    public long gameIndex;
    // public eADBetPlace bettingPosition;
    public eAD_BUTTONLIST currentButtonIndex;
    #endregion

    #region Betting Enable Handle
    public bool bCanBetting = false;
    public float timeRemainToBet = 0f;
    #endregion

    #region UI button holder
    public GameObject[] chipButtonHighlights;
    #endregion

    public static string[] BettingPositions = new string[] { "CenterPos1", "CenterPos2", "CenterPos3", "CenterPos4", "CenterPos5", "CenterPos6",
    "UpperPos1", "UpperPos2", "UpperPos3", "UpperPos4", 
    "LowerPos1", "LowerPos2", "LowerPos3", "LowerPos4", };

    public void OnChipButtonSelect(eAD_BUTTONLIST selectedButton)
    {
        currentButtonIndex = selectedButton;
        if( (currentButtonIndex) > 0) // first based
        {
            chipButtonHighlights[(int)currentButtonIndex - 1].SetActive(true);
            for (int i = 0; i<chipButtonHighlights.Length;++i)
            {
                if(i == (int)(currentButtonIndex - 1) )
                {
                    continue;
                }
                chipButtonHighlights[i].SetActive(false);
            }
        }
        else
        {
            currentButtonIndex = eAD_BUTTONLIST._BTN_BETTING_1;
            chipButtonHighlights[0].SetActive(true);
        }
    }
    private void ClearAllButtons()
    {
        foreach(var button in chipButtonHighlights)
        {
            button.SetActive(false);
            currentButtonIndex = eAD_BUTTONLIST._BTN_BETTING_1;
        }
    }

    public void OnBetting(eADBetPlace betPosIdx)
    {
        // pre-Error handle?
        if(currentButtonIndex <= 0)
        {
            currentButtonIndex = eAD_BUTTONLIST._BTN_BETTING_1;
        }

        Req_Betting(betPosIdx, currentButtonIndex);
    }

    public static void Req_Betting(eADBetPlace betPosIdx, eAD_BUTTONLIST betBtnIdx)
    {
        var packet = new S_09_REQ_BET();
        Debug.Log("[S_09_REQ_BET] idx :" + betPosIdx.ToString());
        packet.nBETPOS = (int)betPosIdx;
        packet.nBTNIDX = (int)betBtnIdx;
        packet.send();
    }
    public static void Rec_Betting()
    {

    }

    public void Bet(int userindex = 0, int chipIndex = 0, int gold = 500)
    {

    }

    private void Start()
    {
        base.Start();
        // var worldNamePos = Camera.main.WorldToScreenPoint(new Vector3(testBettingPosition.position.x, testBettingPosition.position.y + 0.5f, testBettingPosition.position.z));
        // Debug.Log("position of betting board " + testBettingPosition.position
        //     + " rect is " + testBettingPosition.GetComponent<RectTransform>().position
        //     + " world to gui" + GUIUtility.ScreenToGUIPoint(testBettingPosition.position)
        //     + " worldnamepos " + worldNamePos);

        // style.alignment = 
        // posX = testBettingPosition.position.x;
        // posY = testBettingPosition.position.y;
        
    }


    #region betting area indicating
    //[Header("Test purpose only")]
    //public Transform testBettingPosition;
    //public GUIStyle style = new GUIStyle();
    //public float boxWidth = 80;
    //public float boxHeight = 80;
    //public float posX = 0;
    //public float posY = 0;

    //public float range = 8f;

    //private void OnGUI()
    //{
    //    // GUI.Window()
    //    var tempX = (Screen.width / 2) - (boxWidth / 2);
    //    var tempY = (Screen.height/ 2) - (boxHeight/ 2);

    //    var worldNamePos = Camera.main.WorldToScreenPoint(new Vector3(testBettingPosition.position.x, testBettingPosition.position.y + 0.5f, testBettingPosition.position.z));

    //    GUI.Box(new Rect( (worldNamePos.x - boxWidth/2 + posX), (Screen.height - worldNamePos.y - boxHeight/2 + posY), boxWidth, boxHeight), "Betting chips area2");

    //    // GUI.Box(new Rect(tempX + posX, tempY + posY, boxWidth, boxHeight), "Betting chips area");
    //}
    //void FixedUpdate()
    //{
    //    // 338 : 154
    //    // 2.19....
    //    var leftBot = new Vector3(testBettingPosition.position.x -  (range * 2.2f), testBettingPosition.position.y - range);
    //    var rightBot = new Vector3(testBettingPosition.position.x + (range * 2.2f), testBettingPosition.position.y - range);
    //    var leftTop = new Vector3(testBettingPosition.position.x -  (range * 2.2f), testBettingPosition.position.y + range);
    //    var rightTop = new Vector3(testBettingPosition.position.x + (range * 2.2f), testBettingPosition.position.y + range);
    //    // Debug.DrawLine(minX, maxX);
    //    Debug.DrawLine(leftBot, rightBot);
    //    Debug.DrawLine(leftTop, rightTop);
    //    Debug.DrawLine(leftBot, leftTop);
    //    Debug.DrawLine(rightBot, rightTop);

    //}

    #endregion

}
