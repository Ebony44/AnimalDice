using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct SimpleBlobAnimationTag : IComponentData
{
    public BlobAssetReference<SimpleAnimationBlob> anim;
    public float t;
}
