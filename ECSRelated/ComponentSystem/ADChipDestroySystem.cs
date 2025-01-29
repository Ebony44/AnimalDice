#if !UNITY_IOS

using Unity.Entities;


[DisableAutoCreation]
public class ADChipDestroySystem : SystemBase
{
    EntityCommandBufferSystem entityCommandBuffer;
    EntityQuery entityQuery;
    

    protected override void OnCreate()
    {
        entityCommandBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        
    }
    protected override void OnUpdate()
    {
        if (ResourceContainer.Instance == null)
        {
            return;
        }
        if (ResourceContainer.Get<ADChipBettingManager>() == null)
        {
            return;
        }
        var temp = ResourceContainer.Get<ADChipBettingManager>().gameObject;
        //if (ResourceContainer.Get<ADChipBettingManager>().gameObject.activeInHierarchy == true)
        //{
        //    if (ResourceContainer.Get<ADChipBettingManager>().bDestroyingChipEntities == false)
        //    {
        //        return;
        //    }
        //}

        //if (ADChipManager.Instance.bDestroyingChipEntities == false)
        //{
        //    return;
        //}

        // var commandBuffer = entityCommandBuffer.CreateCommandBuffer().ToConcurrent(); // as upper version of entities package, use AsParallelWriter().... instead of ToConcurrent()....

        var commandBuffer = entityCommandBuffer.CreateCommandBuffer().AsParallelWriter();

        var deltaTime = Time.DeltaTime;

        if (ResourceContainer.Get<ADChipBettingManager>().bDestroyingChipEntities == true)
        {

            Entities
            // .WithoutBurst()
            // .WithAny<LinkedEntityGroup>()
            .ForEach((Entity entity, int nativeThreadIndex, in ADChipTag chip) =>
           {
               //if (chip.bNeedToBeDestroyed)
               //{
               //}
               // spriteRenderer.color = new Color(1, 1, 1, 0);

               if (chip.bCanBeDestroyed)
               {
                   commandBuffer.DestroyEntity(nativeThreadIndex, entity);

               }

           }).ScheduleParallel();


        }

        // destroy lose chips
        //if (ResourceContainer.Get<ADChipBettingManager>().bDestroyingOnlyLoseChipsWithAlpha == true)
        //{
        //    Entities
        //        // .WithoutBurst()
        //        // .WithAny<LinkedEntityGroup>()
        //        .ForEach((Entity entity, int nativeThreadIndex, in ADChipTag chip) =>
        //        {
        //            if (chip.bIsLose == true)
        //            {
        //                commandBuffer.DestroyEntity(nativeThreadIndex, entity);

        //            }

        //        }).ScheduleParallel();
        //}


        entityCommandBuffer.AddJobHandleForProducer(Dependency);

    }
}

#endif