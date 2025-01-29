using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ADGameChatItem : PoolItemBase
{
    public TextMeshProUGUI lbText;
    public Image back;

    public float alphaTweenTime = 0.5f;
    public float waitTime = 3f;

    #region
    // turn it "true" only new balloon should exsist.
    public bool bHasToDeletePrevious = false;
    // public int playerIndex = 99;

    public const float ONELINE_LIMIT = 66f;
    public Coroutine textRoutine = null;

    #endregion
    [TestMethod]
    public void OnPop(ADChatPoolItemInfo info)
    {
        
        lbText.text = info.message;
        if (bHasToDeletePrevious)
        {
            
            // ResourcePool.GetAll<ADGameChatItem>();
            StartCoroutine(BackRoutine(info.message,info.playerIndex));
            //if (textRoutine == null)
            //{
            //    textRoutine = StartCoroutine(BackRoutine(text));
            //}
            //else
            //{
            //    StopCoroutine(textRoutine);
                
            //    StartCoroutine(BackRoutine(text));
            //}
        }

        //CoroutineChain.Start
        //    .Wait(0.01f)
        //    .Call(() => StartCoroutine(BackRoutine(text)));
        // StartCoroutine(BackRoutine(text));

    }
    public int maxTextCount = 50;
    public float offset = 55f;
    public Vector3 posOffset;
    public IEnumerator BackRoutine(string text,int playerIndex)
    {
        //yield return 
        //while (playerIndex == 99)
        //{
        //    yield return null;
        //}

        TimeContainer.ContainClear("COMMON_CHAT_ALPHA_" + playerIndex.ToString());
        TimeContainer.ContainClear("COMMON_CHAT_WAIT_" + playerIndex.ToString());

        TimeContainer.Stack alphaTweenTC = new TimeContainer.Stack(3, alphaTweenTime, "COMMON_CHAT_ALPHA_" + playerIndex.ToString());
        TimeContainer waitTC = new TimeContainer("COMMON_CHAT_WAIT_" + playerIndex.ToString(), waitTime);
        TimeContainer waitTotalTC = new TimeContainer("COMMON_CHAT_WAIT_" + playerIndex.ToString(), waitTime);
        TimeContainer tempWaitTC = new TimeContainer("COMMON_CHAT_WAIT_" + playerIndex.ToString(), alphaTweenTime / 5);
        //var tempTC = alphaTweenTC.Pop();
        //while (true)
        //{
        //    yield return null;
        //    if (alphaTweenTC.Pop().t >= 1)
        //    {
        //        ResourcePool.Push(this);
        //        yield break;
        //    }
        //}


        if (text.Length >= maxTextCount)
        {
            lbText.text = text.Remove(maxTextCount);
            lbText.text += "...";
        }
        else
        {
            lbText.text = text;
        }
        
        yield return this.Wait(tempWaitTC);

        var size = back.GetComponent<RectTransform>().sizeDelta;
        size.y = lbText.renderedHeight + offset;
        back.GetComponent<RectTransform>().sizeDelta = size;

        

        Debug.Log("[AD_Chat], height is " + back.rectTransform.sizeDelta.y
            + " player index " + playerIndex
            + " ");
        if (back.rectTransform.sizeDelta.y <= ONELINE_LIMIT)
        // if ((lbText.renderedHeight + offset) <= ONELINE_LIMIT)
        {

            Debug.Log("[AD_Chat], transform position before " + transform.position);
            // transform.localPosition += Vector3.up * 4f;
            
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 25f , transform.localPosition.z);

            // lbText.transform.position += Vector3.up * 4f;
            Debug.Log("[AD_Chat], transform position after " + transform.position
                + " left value " + (lbText.renderedHeight + offset));
        }
        else
        {
            Debug.Log("[AD_Chat] it's more than 1 line " + transform.position);
        }


        
        //while(waitTotalTC.t < 1)
        //{
            
        //    ResourcePool.Push(this);
        //}
        yield return back.AlphaTween(0f, 1f, alphaTweenTC.Pop(), true);
        yield return this.Wait(waitTC);
        yield return back.AlphaTween(1f, 0f, alphaTweenTC.Pop(), true);
        yield return new WaitForEndOfFrame();
        back.SetAlpha(0f, true);

        ResourcePool.Push(this);
    }

    public override void Back()
    {
        ResourcePool.Push(this);
    }
    [TestMethod(false)]
    public void TestAdjust(float offset)
    {
        lbText.rectTransform.sizeDelta = new Vector2(lbText.rectTransform.sizeDelta.x, lbText.rectTransform.sizeDelta.y + offset);
    }

}
public class ADChatPoolItemInfo
{
    public string message;
    public int playerIndex;

    public ADChatPoolItemInfo(string message, int playerIndex)
    {
        this.message = message;
        this.playerIndex = playerIndex;
    }
}