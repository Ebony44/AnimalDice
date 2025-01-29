#if !UNITY_IOS
using Unity.Entities;
using Unity.Transforms;


[DisableAutoCreation]
public class ADChipMarkSystem : SystemBase
{
    EntityQuery entityQuery;
    EndSimulationEntityCommandBufferSystem EndSimulationEcbSystem;
    protected override void OnCreate()
    {
        EndSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

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
        if (ResourceContainer.Get<ADChipBettingManager>().bCanMarkChips)
        {
            // var ecb = EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();

            var ecb = EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();


            if (ResourceContainer.Get<ADResultPartInfoStoring>().bHasCopiedWinLoseChipInfo == false)
            {
                ADResultPartInfoStoring resultPart = ResourceContainer.Get<ADResultPartInfoStoring>();
                #region obsolete
                // NativeArray<ADChipTag> loseChipDatas = new NativeArray<ADChipTag>(dataCount, Allocator.TempJob);
                // NativeArray<ADChipTag> winChipDatas = new NativeArray<ADChipTag>(dataCount, Allocator.TempJob);
                // // NativeArray<Entity> loseChipDataEntities = new NativeArray<Entity>(dataCount, Allocator.TempJob);
                // // NativeArray<Entity> winChipDataEntities = new NativeArray<Entity>(dataCount, Allocator.TempJob);

                // Entities.WithStoreEntityQueryInField(ref entityQuery)
                // // .WithoutBurst()
                // .ForEach((Entity entity, int entityInQueryIndex, in ADChipTag adChip) =>
                //{
                //    if(ResourceContainer.Get<ADResultPartInfoStoring>().bHasCopiedWinLoseChipInfo == false)
                //    {

                //        if (adChip.bIsLose)
                //        {
                //            loseChipDatas[entityInQueryIndex] = adChip;
                //            // loseChipDataEntities[entityInQueryIndex] = entity;

                //        }
                //        else if (adChip.bIsLose == false)
                //        {
                //            winChipDatas[entityInQueryIndex] = adChip;
                //            // winChipDataEntities[entityInQueryIndex] = entity;
                //        }
                //    }
                //}).ScheduleParallel();
                #endregion

                var resultInfo = ResourceContainer.Get<ADResultPartInfoStoring>();
                Entities
                    .WithoutBurst()
                    .ForEach((Entity entity, int entityInQueryIndex, ref ADChipTag adChip, in Translation translation) =>
                   {
                       // 1. plus that info to stored info
                       // adChip.userIndex
                       // if()

                       // 1. where to win
                       // 2. who is winner
                       // 3. how much winmoney is...
                       if(adChip.bIsLose == false) // win
                       {
                           if (adChip.winUserIndex == -1) // not decided
                           {
                               // check 5 bet places
                               // ResourceContainer.Get<ADResultPartInfoStoring>().TestMinusFromChipToWinInfo(adChip.bettingPlace, adChip.chipKind);
                               var tempMoney = ResourceContainer.Get<ADChipBettingManager>().GetMoneyFromChip(adChip.chipKind);
                               // ResourceContainer.Get<ADResultPartInfoStoring>().winPlaceMoney[adChip.bettingPlace] += tempMoney;
                               var lookUp = resultInfo.TestMinusFromChipToWinInfo(adChip.bettingPlace, adChip.chipKind);
                               if (lookUp.Item2 == true)
                               {
                                   adChip.srcPos = translation.Value;
                                   adChip.dstPos = ResourceContainer.Get<GameUserPosition>(lookUp.Item1).userPos.position;

                                   // adChip.traveledTime = 0; -> dont touch this... modify it when ResourceContainer.Get<ADChipBettingManager>().bCanMoveToWinPlayer is true...
                                   adChip.traveledTime = 1;
                                   // var temp = ResourceContainer.Get<GameUserPosition>(lookUp.Item1).userPos.position;
                                   adChip.winUserIndex = lookUp.Item1;
                                   resultPart.currentWinChips++;
                               }
                           }

                       }
                       

                   }).Run();

            }

            

            if(ResourceContainer.Get<ADChipBettingManager>().bDestroyChipImmediately == true)
            {
                Entities.ForEach
                    ((ref ADChipTag adChip) =>
                   {
                       adChip.bCanBeDestroyed = true;
                   }).ScheduleParallel();
            }

            EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
            
        }
        
        
        
    }

}

#endif