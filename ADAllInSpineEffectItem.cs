using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADAllInSpineEffectItem : PoolItemBase
{
    public SkeletonAnimation spine;
    public MeshRenderer meshRender;
    EffectData _data;

    public override void Back()
    {
        ResourcePool.Push(this);
    }
    public void SetSortingLayer(string layerName, int order)
    {
        meshRender.sortingLayerName = layerName;
        meshRender.sortingOrder = order;
    }
    public void PlaySpineStaying()
    {
        spine.Play(_data.animationName);
    }
    public Coroutine Play()
    {
        return StartCoroutine(SpinePlayRoutine());
    }
    IEnumerator SpinePlayRoutine()
    {
        yield return spine.Play(_data.animationName);
        Back();
    }
    public void OnPop(EffectData data)
    {
        _data = data;

        var scale = transform.localScale;
        scale *= data.scaleFactor;
        transform.localScale = scale;

        spine.skeletonDataAsset = data.asset;
        spine.Initialize(true);
        spine.ClearState();
    }
}
