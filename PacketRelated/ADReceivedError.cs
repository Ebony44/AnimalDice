using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADReceivedError : PacketHandler
{
    public override int GetNumber()
    {
        return (int)ANIMALDICE_PK.R_09_ERROR;
    }
    public override void Func()
    {
        var rec = new R_09_ERROR(SubGameSocket.m_bytebuffer);
        Debug.LogError("[R_09_ERROR], error type is " + rec.nERROR_TYPE
            + " error message: " + rec.szMSG);
        
    }

}
