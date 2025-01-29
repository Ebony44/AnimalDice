using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AD_Chat : PacketHandler
{
    // public float testOffsetPosY;
    public override int GetNumber()
    {
        return (int)COMMON_PK.R_97_CHAT;
    }

    public override void Func()
    {
        var rec = new R_97_CHAT(SubGameSocket.m_bytebuffer);
        Debug.Log("[R_97_CHAT] id : " + rec.szMSGID + ", msg : " + rec.szMSG);
        var tempPlayer = ResourcePool.Find<GamePlayer>(p => p.Nick == rec.szMSGID);

        //Debug.Log("[R_97_CHAT] id : " + rec.szMSGID + ", msg : " + rec.szMSG);

        var player = ResourcePool.Find<GamePlayer>(p => p.Nick == rec.szMSGID);

        //var chat = ResourcePool.Pop<ADGameChatItem, string>(rec.szMSG);
        ADChatPoolItemInfo info = new ADChatPoolItemInfo(rec.szMSG,player.roomIdx);
        var chat = ResourcePool.Pop<ADGameChatItem, ADChatPoolItemInfo>(info);

        // chat.bHasToDeletePrevious = true;
        // chat.playerIndex = player.roomIdx;
        var tempYOffSet = 0f;
        //if(rec.szMSG.Length <= 14)
        //{
        //    tempYOffSet = 4f;
        //    Debug.Log("[AD_Chat], length is less than or equal to 14, length: " + rec.szMSG.Length
        //        + " Y offset " + tempYOffSet);
            
        //    chat.posOffset = Vector3.up * tempYOffSet;
        //}
        //else
        //{
        //    Debug.Log("[AD_Chat], length is greater than 14, length: " + rec.szMSG.Length
        //        + " Y offset " + tempYOffSet);
            
        //    chat.posOffset = Vector3.zero;
        //}
        

        chat.transform.position = player.transform.position;
        chat.transform.position += chat.posOffset;
        chat.back.transform.localRotation = new Quaternion(0, 0, 0, 1);
        chat.lbText.transform.localRotation = new Quaternion(0, 0, 0, 1);

        // var tempMiscInfo = ResourceContainer.Get<ADMiscInfo>();
        if (player.roomSerial != 0)
        {
            if (player.roomIdx >= 1 && player.roomIdx <= 3)
            {
                chat.back.transform.localRotation = new Quaternion(0, 0, 0, 1);
                chat.lbText.transform.localRotation = new Quaternion(0, 0, 0, 1);
                chat.transform.position = new Vector3(chat.transform.position.x + ADMiscInfo.CHAT_LEFT_X_VALUE, chat.transform.position.y, chat.transform.position.z);
            }
            else if (player.roomIdx >= 4 && player.roomIdx <= 7)
            {
                chat.back.transform.localRotation = new Quaternion(ADMiscInfo.CHAT_UP_X_FLIP_VALUE, 0, 0, 1);
                chat.lbText.transform.localRotation = new Quaternion(-ADMiscInfo.CHAT_UP_X_FLIP_VALUE, 0, 0, 1);
                chat.transform.position = new Vector3(chat.transform.position.x, chat.transform.position.y + ADMiscInfo.CHAT_UP_Y_VALUE, chat.transform.position.z);
            }
            else if (player.roomIdx >= 8 && player.roomIdx <= 10)
            {
                chat.back.transform.localRotation = new Quaternion(0, 0, 0, 1);
                chat.lbText.transform.localRotation = new Quaternion(0, 0, 0, 1);
                chat.transform.position = new Vector3(chat.transform.position.x + ADMiscInfo.CHAT_RIGHT_X_VALUE, chat.transform.position.y, chat.transform.position.z);
            }
        }

        //tempPlayer.chatBalloon.gameObject.SetActive(true);

        //if (ResourceContainer.Get<ADMiscInfo>().bHasPlayerChat[tempPlayer.roomIdx] == false &&
        //    tempPlayer.roomIdx == 0)
        //{
        //    tempPlayer.chatBalloon.gameObject.AddComponent<Canvas>();
        //    var tempCanvas = tempPlayer.chatBalloon.gameObject.GetComponent<Canvas>();
        //    tempCanvas.overridePixelPerfect = true;
        //    tempCanvas.overrideSorting = true;
        //    tempCanvas.sortingLayerName = "WorldForward";
        //    tempCanvas.sortingOrder = 6055;
        //    ResourceContainer.Get<ADMiscInfo>().bHasPlayerChat[0] = true;
        //}

        //tempPlayer.Chat(rec.szMSG);

        
        

        GameChat.Instance.AddChat(rec);
    }

    [TestMethod(false)]
    public void TestChat(int serial, string msg)
    {
        var player = ResourcePool.Find<GamePlayer>(p => p.roomSerial == serial);
        ADChatPoolItemInfo info = new ADChatPoolItemInfo(msg, player.roomIdx);
        var chat = ResourcePool.Pop<ADGameChatItem, ADChatPoolItemInfo>(info);
        
        var tempYOffSet = 0f;

        chat.transform.position = player.transform.position;
        chat.transform.position += chat.posOffset;
        chat.back.transform.localRotation = new Quaternion(0, 0, 0, 1);
        chat.lbText.transform.localRotation = new Quaternion(0, 0, 0, 1);



        if (player.roomIdx >= 1 && player.roomIdx <= 3)
        {
            chat.back.transform.localRotation = new Quaternion(0, 0, 0, 1);
            chat.lbText.transform.localRotation = new Quaternion(0, 0, 0, 1);
            chat.transform.position = new Vector3(chat.transform.position.x + ADMiscInfo.CHAT_LEFT_X_VALUE, chat.transform.position.y, chat.transform.position.z);
        }
        else if (player.roomIdx >= 4 && player.roomIdx <= 7)
        {
            chat.back.transform.localRotation = new Quaternion(ADMiscInfo.CHAT_UP_X_FLIP_VALUE, 0, 0, 1);
            chat.lbText.transform.localRotation = new Quaternion(-ADMiscInfo.CHAT_UP_X_FLIP_VALUE, 0, 0, 1);
            chat.transform.position = new Vector3(chat.transform.position.x, chat.transform.position.y + ADMiscInfo.CHAT_UP_Y_VALUE, chat.transform.position.z);
        }
        else if (player.roomIdx >= 8 && player.roomIdx <= 10)
        {
            chat.back.transform.localRotation = new Quaternion(0, 0, 0, 1);
            chat.lbText.transform.localRotation = new Quaternion(0, 0, 0, 1);
            chat.transform.position = new Vector3(chat.transform.position.x + ADMiscInfo.CHAT_RIGHT_X_VALUE, chat.transform.position.y, chat.transform.position.z);
        }
        


        // GameChat.Instance.AddChat(rec);


    }
}
