using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADBetStart : PacketHandler
{
    public override int GetNumber()
    {
        return (int)ANIMALDICE_PK.R_09_BETSTART;
    }
    public override void Func()
    {
        var rec = new R_09_BETSTART(SubGameSocket.m_bytebuffer);
        Debug.Log("[R_09_BETSTART]" + rec.nSEC
            + " current game index " + rec.stGAME_IDX);

        ResourceContainer.Get<ADBettingTimeCounter>().SetNumber(rec.nSEC);

        // throw new System.NotImplementedException();
    }

}
