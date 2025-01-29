using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADReceiveTimer : PacketHandler
{
    public override int GetNumber()
    {
        return (int)ANIMALDICE_PK.R_09_SEC;
    }

    public override void Func()
    {
        var rec = new R_09_SEC(SubGameSocket.m_bytebuffer);
        if(ResourceContainer.Get<ADBettingTimeCounter>().timerText.alpha != 1)
        {
            ResourceContainer.Get<ADBettingTimeCounter>().TurnOnWithAlpha();
        }
        if (rec.nSEC % 5 == 0)
        {
            Debug.Log("[R_09_SEC] : " + rec.nSEC);
        }
        if(rec.nSEC <= 1)
        {
            var betManager = ResourceContainer.Get<ADChipBettingManager>();
            if (betManager.bBettingSpinePlayed == false)
            {
                // betManager.PlayBettingEnding();
                betManager.bBettingSpinePlayed = true;
            }
        }
        if(1 <= rec.nSEC && rec.nSEC <= 3f)
        {
            Debug.Log("play sound for timer remaining, time is " + rec.nSEC);
            Sound.Instance.EffPlay("AD_3SecAlarm");
        }
        #region
// TODO: if background time exsists, remove some spine, sprite
// multiplier spine, highlighted betting board
// set alpha value of user's photo

#if UNITY_ANDROID
        Debug.Log("paused time is " + ResourceContainer.Get<ADGameMain>().pausedTime);
#endif
        if (ResourceContainer.Get<ADGameMain>().pausedTime >= 4f)
        {
            ResourcePool.ClearAll<ADSpineMultiplierEffectItem>(ef =>
            {
                ef.tag.Equals("ADMultiplier");
                return ef;
            });
            ResourceContainer.Get<ADChipBettingManager>().DisableAllBettingBoardHighlight();

            var tempUsers = ResourcePool.GetAll<GamePlayer>();
            foreach (var user in tempUsers)
            {
                if (user.roomSerial.Equals(-1) == false && user.userPhoto.color.a != 1f)
                {
                    user.userPhoto.AlphaTween(1f, 0.01f);
                }
            }


            ResourceContainer.Get<ADGameMain>().pausedTime = 0;

        }


#endregion
        ResourceContainer.Get<ADBettingTimeCounter>().SetNumber(rec.nSEC);
        // throw new System.NotImplementedException();
    }

}
