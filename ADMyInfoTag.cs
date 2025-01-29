using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ADMyInfoTag : ResourceTag
{
    public TextMeshProUGUI lbHave;
    public TextMeshProUGUI lbGap;

    private long _haveMoney;
    public long haveMoney
    {
        get
        {
            return _haveMoney;
        }
        set
        {
            var prev = _haveMoney;
            _haveMoney = value;

            TimeContainer.ContainClear("MyInfoHaveMoneyTime");
            // lbHave.StopAllCoroutines();

            // lbHave.NumberKMBTween(prev, _haveMoney, GameUtils.st_globalTweenTime);
            // if(prev % 1000 == 0 && _haveMoney)
            TimeContainer t1 = new TimeContainer("MyInfoHaveMoneyTime", 0.3f);
            lbHave.NumberTween(prev, _haveMoney, g => g.ToStringWithKMB(false, 3, false), t1);
            // NumberUtil.RollingKMB()
            
        }
    }
    [TestMethod]
    public void SetHave(long gold)
    {
        haveMoney = gold;
    }

    private long _gapMoney;
    public long gapMoney
    {
        get
        {
            return _gapMoney;
        }
        set
        {
            var prev = _gapMoney;
            _gapMoney = value;

            if (_gapMoney == 0)
            {
                lbGap.color = Color.white;
            }
            else
            {
                // lbGap.ColorTween(_gapMoney > 0 ? Color.green : Color.red, GameUtils.st_globalTweenTime);
                lbGap.color = _gapMoney > 0 ? Color.green : Color.red;
            }

            lbGap.StopAllCoroutines();
            // lbGap.NumberKMBTween(prev, _gapMoney, GameUtils.st_globalTweenTime);
            lbGap.NumberTween(prev,_gapMoney, g => g.ToStringWithKMB(false, 3, false), GameUtils.st_globalTweenTime);

            if (prev.Equals(_gapMoney))
            {
                lbGap.text = _gapMoney.ToStringWithKMB();
            }
        }
    }


    [TestMethod]
    public void TestHaveGapSet(long have, long gap)
    {
        // ResourceContainer.Get<ADMyInfoTag>("MyInfo").haveMoney = have;
        // ResourceContainer.Get<ADMyInfoTag>("MyInfo").gapMoney = gap;

        // haveMoney = have;
        // gapMoney = gap;
    }

}
