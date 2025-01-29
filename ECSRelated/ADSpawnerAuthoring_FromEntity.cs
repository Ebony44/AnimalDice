using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class ADSpawnerAuthoring_FromEntity : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
    public GameObject prefab;
    public int count;
    public float3 minRange;
    public float3 maxRange;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var spawnerData = new SpawnSettings
        {
            Prefab = conversionSystem.GetPrimaryEntity(prefab),
            MinRange = new float3(-10, -10, 97),
            MaxRange = new float3(10, 10, 100),
            Count = count
        };
        dstManager.AddComponentData(entity, spawnerData);
        
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(prefab);
    }
}
