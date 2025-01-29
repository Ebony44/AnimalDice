using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AD_SubMoney : PacketHandler
{
    public override int GetNumber()
    {
        return (int)COMMON_PK.R_97_SUBMONEY;
    }
    public override void Func()
    {
        var rec = new R_97_SUBMONEY(SubGameSocket.m_bytebuffer);
        Debug.Log("[R_97_SUBMONEY] submoney : " + (long)rec.stSUBMONEY);
        if(ResourceContainer.Get<GameTimeBonusBox>().gameObject.activeInHierarchy == false)
        {
            var tempBox = ResourceContainer.Get<GameTimeBonusBox>();
            tempBox.gameObject.SetActive(true);
            tempBox.receiveButton.SetActive(false);
            tempBox.timer.SetActive(false);
        }
        ResourceContainer.Get<GameTimeBonusBox>().SetCurrentValue(2);
        ResourceContainer.Get<GameTimeBonusBox>().UpdateSubMoney(rec.stSUBMONEY);
        ResourceContainer.Get<GameTimeBonusBox>().OpenBox();
        ResourceContainer.Get<GamePanelGiftQuest>().UpdateSubMoney(rec.stSUBMONEY);
        ResourceContainer.Get<GameTimeBonusBox>().readyButton.SetActive(false);

    }

    [TestMethod(false)]
    public void TestSubmoney()
    {
        var tem = new R_97_SUBMONEY(SubGameSocket.m_bytebuffer);
        tem.stSUBMONEY = 1000;
        if (ResourceContainer.Get<GameTimeBonusBox>().gameObject.activeInHierarchy == false)
        {
            var tempBox = ResourceContainer.Get<GameTimeBonusBox>();
            tempBox.gameObject.SetActive(true);
            tempBox.receiveButton.SetActive(false);
            tempBox.timer.SetActive(false);
        }
        ResourceContainer.Get<GameTimeBonusBox>().SetCurrentValue(2);
        ResourceContainer.Get<GameTimeBonusBox>().UpdateSubMoney(tem.stSUBMONEY);
        ResourceContainer.Get<GameTimeBonusBox>().OpenBox();
        ResourceContainer.Get<GamePanelGiftQuest>().UpdateSubMoney(tem.stSUBMONEY);
    }
}
