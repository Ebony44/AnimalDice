using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADMiscInfo : ResourceItemBase
{
    //public List<bool> bHasPlayerChat = new List<bool>(11);
    //protected override void Start()
    //{
    //    base.Start();

    //    for (int i = 0; i < 11; i++)
    //    {
    //        bHasPlayerChat.Add(false);
    //    }
    //}

    public Transform standardPos;
    public const float CHAT_LEFT_X_VALUE = 2f;
    public const float CHAT_RIGHT_X_VALUE = -2f;
    public const float CHAT_UP_Y_VALUE = 16f;
    public const float CHAT_UP_X_FLIP_VALUE = 180f;

    //protected override void Start()
    //{
    //    base.Start();


    //}
    [TestMethod(false)]
    public void TestFlip()
    {
        // back.transform.localRotation = new Quaternion(ADMiscInfo.CHAT_UP_X_FLIP_VALUE, 0, 0, 1);
        // lbText.transform.localRotation = new Quaternion(-ADMiscInfo.CHAT_UP_X_FLIP_VALUE, 0, 0, 1);
        transform.position = new Vector3(transform.position.x, transform.position.y + ADMiscInfo.CHAT_UP_Y_VALUE, transform.position.z);
    }
    [TestMethod(false)]
    public void TestUnDoFlip()
    {
        // back.transform.localRotation = new Quaternion(0, 0, 0, 1);
        // lbText.transform.localRotation = new Quaternion(0, 0, 0, 1);
        transform.position = new Vector3(transform.position.x, transform.position.y - ADMiscInfo.CHAT_UP_Y_VALUE, transform.position.z);
    }


}
