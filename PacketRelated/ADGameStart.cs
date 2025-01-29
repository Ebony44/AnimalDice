using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADGameStart : PacketHandler
{
    public override int GetNumber()
    {
        return (int)ANIMALDICE_PK.R_09_GAMESTART;
    }
    public override void Func()
    {
        var rec = new R_09_GAMESTART(SubGameSocket.m_bytebuffer);
        Debug.Log("[R_09_GAMESTART]");
        
    }
}
