using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Entities;
using Unity.Mathematics;

public class ADSpawnSettings : MonoBehaviour
{

}

#region move it if needed

interface ISpawnSettings
{
    Entity Prefab { get; set; }
    float3 Position { get; set; }
    quaternion Rotation { get; set; }
    float3 Range { get; set; }
    int Count { get; set; }
}
struct SpawnSettings : IComponentData, ISpawnSettings
{
    public Entity Prefab { get; set; }
    public  float3 Position { get; set; }
    public  quaternion Rotation { get; set; }
    public  float3 Range { get; set; }
    public float3 MinRange { get; set; }
    public float3 MaxRange { get; set; }
    public  int Count { get; set; }

    public float3 TargetPosition { get; set; }
    
}

#endregion