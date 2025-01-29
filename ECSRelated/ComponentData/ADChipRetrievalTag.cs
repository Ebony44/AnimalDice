using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct ADChipRetrievalTag : IComponentData
{
    public eAD_BUTTONLIST chipKind;
    public eADBetPlace bettingPlace;

    public float3 srcPos;
    public float3 midPos;
    public float3 dstPos;

    public int chipIndex;
    public int winningUserIndex;

}
