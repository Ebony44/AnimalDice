using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ADTest_1 : MonoBehaviour//,IPointerDownHandler,IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Canvas tempCanvas;

    public Button tempButton;
    public Image tempImage;

    public bool tempDownBool;
    public bool tempExitBool;

    
    public void OnPointerDown()
    {
        tempImage.color = Color.red;
        tempDownBool = true;
        Debug.Log("temp down is " + tempDownBool);

    }
    public void OnPointerUp()
    {
        tempImage.color = Color.blue;
        tempDownBool = false;
        Debug.Log("temp down is " + tempDownBool);
    }
    public void OnPointerExit()
    {
        // tempImage.color = Color.green;
        tempExitBool = true;
        Debug.Log("temp exit is " + tempExitBool);
        if(tempDownBool == true)
        {
            OnPointerUp();
        }
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    throw new System.NotImplementedException();
        
    //}

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    throw new System.NotImplementedException();
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    throw new System.NotImplementedException();
    //}

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    throw new System.NotImplementedException();
    //}

    [TestMethod]
    public void TestCanvasChange()
    {
        tempCanvas.overridePixelPerfect = true;
        tempCanvas.overrideSorting = true;
        tempCanvas.sortingLayerName = "WorldForward";
    }

    [TestMethod]
    public void TestCanvasChange2()
    {
        var pp = ResourceContainer.Get<GameUserPosition>(0);
        //var player = ResourcePool.Pop<GamePlayer,GamePlayerInitData>(new GamePlayerInitData(_nick, _serial, _have, _gab, _url,_idx,false));
        var player = ResourcePool.Find<GamePlayer>(p => p.roomIdx == 0);



        // player.OnPop(new GamePlayerInitData(_nick, _serial, _have, _gab, _url, _idx, false, _isMale, _vip));



        player.chatBalloon.gameObject.AddComponent<Canvas>();
        var tempCanvas = player.chatBalloon.gameObject.GetComponent<Canvas>();
        // yield return new WaitForSeconds(0.02f);

        player.gameObject.GetComponent<Image>().SetAlpha(0f, true);


        //tempCanvas.pixelPerfect = false;
        tempCanvas.overridePixelPerfect = true;
        tempCanvas.overrideSorting = true;
        tempCanvas.sortingLayerName = "WorldForward";
        tempCanvas.sortingOrder = 6055;
    }


    [TestMethod]
    public void TestOnpress()
    {

    }

    

}
