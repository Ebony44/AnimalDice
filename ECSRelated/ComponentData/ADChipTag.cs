using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
// [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Size = 176)]
// [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
public struct ADChipTag : IComponentData
{
    public eAD_BUTTONLIST chipKind;

    #region moving related... maybe make another component..
    public float3 srcPos;
    // public float3 midPos;
    public float3 dstPos;

    public float traveledTime;
    public float travelTimeStandard;


    public float rotationZAxis;
    

    #region betting pos for retrieve
    public eADBetPlace bettingPlace;
    
    public bool bIsLose;
    // public bool b
    public int winUserIndex;
    #endregion

    #endregion

    #region destroying related.. maybe make another component..
    public float alphaTime;
    public float alphaTimeStandard;
    public bool bCanBeDestroyed;

    public bool bCanBeDestroyedWithAlpha;



    #endregion

    // public bool bNeedToBeDestroyed;

}
