#if UNITY_IOS
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;


public class TestChipWithECS : MonoBehaviour
{

}

public struct ECSRigidBody : IComponentData
{
    public float2 velocity;
}

class PhysicsSystem : ComponentSystem
{
    private ComponentSystemGroup group;
    // private ComponentGroup a;
    
    protected override void OnCreate()
    {
        // group = 
    }

    protected override void OnUpdate()
    {
        // throw new System.NotImplementedException();
    }
}

public class SpawnRandomPhysicsBodies : MonoBehaviour
{

}
#endif