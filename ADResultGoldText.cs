using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ADResultGoldText : PoolItemBase
{
    public TextMeshProUGUI render;
    public TMP_FontAsset plus;
    public TMP_FontAsset minus;

    public void OnPop(long gold)
    {
        // render.font = gold >= 0 ? plus : minus;
        render.font = gold >= 0 ? plus : minus;
        render.text = (gold >= 0 ? "+" : "") + gold.ToStringWithKMB().Replace(" ", "");
        render.color = gold >= 0 ? Color.green : Color.red;// new Color(1, 1, 1, 0);
    }

    public override void Back()
    {
        ResourcePool.Push(this);
    }
}
