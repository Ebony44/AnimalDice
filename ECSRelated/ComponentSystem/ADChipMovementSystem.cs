
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[DisableAutoCreation]
public class ADChipMovementSystem : SystemBase
{

    private EndSimulationEntityCommandBufferSystem endSimECBSystem;

    private EntityQuery entityQuery;


    protected override void OnCreate()
    {
        endSimECBSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
        // entityQuery = GetEntityQuery(ComponentType.ReadOnly<ADChipTag>(), ComponentType.ReadOnly<Translation>());

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
        //var ecb = endSimECBSystem.CreateCommandBuffer().ToConcurrent();

        var ecb = endSimECBSystem.CreateCommandBuffer().AsParallelWriter();

        var deltaTime = Time.DeltaTime;
        var spendTime = 0f;

        // var tempCount = entityQuery.CalculateEntityCount();

        var chipBettingInfo = ResourceContainer.Get<ADChipBettingManager>();


        //if (ADChipManager.Instance == null)
        //{
        //    spendTime = Time.DeltaTime / ADChipManager.Instance.chipMovingTime;
        //}

        // for upper version... AsParallelWriter(); instead of ToConcurrent();
        // ..?

        // current game setting(or phase) allows to move the chips...?
        if (ResourceContainer.Get<ADChipBettingManager>().gameObject.activeInHierarchy == true)
        {
            // move any chips which destinations are betting board... all chip movement only controls in this if statement
            if (chipBettingInfo.bCanMove)
            {
                Entities
            // .WithNone<ADChipExcludingTag>() // comment out it when testing movement
                                            // .WithAll<ADChipTag>()
            .ForEach((
                Entity chipEntity,
                ref Translation translation,
                ref Unity.Transforms.Rotation rotation,
                ref ADChipTag ADChip,
                ref SimpleBlobAnimationTag anim) =>
            {

                var startPos = ADChip.srcPos;
                // var midPos = ADChip.midPos;
                var endPos = ADChip.dstPos;
                // var zFloat = ADChip.rotationZAxis / 360;
                var zFloat = math.radians(ADChip.rotationZAxis);
                if (rotation.Value.value.z != zFloat)
                {
                    // rotation.Value = new quaternion(0, 0, zFloat, -0.1296479f);
                    rotation.Value = quaternion.RotateZ(zFloat);
                }


                // var vectorToTarget = ADChip.dstPos - translation.Value;
                var t = ADChip.traveledTime;
                //if (math.lengthsq(vectorToTarget) > 2f && t < 1
                //&& anim.t < 1 * ADChip.travelTimeStandard)
                if (ADChip.traveledTime < 1)
                {

                    // var moveDirection = math.normalize(vectorToTarget);

                    ADChip.traveledTime += deltaTime / ADChip.travelTimeStandard;
                    if (ADChip.traveledTime >= 1f)
                    {
                        ADChip.traveledTime = 1f;
                    }
                    if (t > 1)
                    {
                        t = 1;
                    }


                    //    var tempInterpolateValue = (startPos * (1 - t) * (1 - t))
                    //+ (midPos * (1 - t) * t)
                    //+ (endPos * t * t);

                    //    var temp = math.lerp(startPos, endPos, tempInterpolateValue);


                    // math.lerp(startPos, endPos, midPos);

                    // translation.Value = translation.Value + moveDirection * deltaTime; // * speed value?
                    // translation.Value = math.lerp(startPos, endPos, tempInterpolateValue);

                    // translation.Value = math.lerp(startPos, endPos, ADChip.traveledTime);

                    // -> modify with animation curve

                    
                    // anim.t += deltaTime;
                    
                    // anim.t += deltaTime / ADChip.travelTimeStandard;

                    // var animTime = anim.t / ADChip.travelTimeStandard;
                    // anim.anim.Value.Evaluate(anim.t);
                    // translation.Value = math.lerp(startPos, endPos, anim.t);
                    if (anim.t > 1)
                    {
                        anim.t = 1;
                    }
                    translation.Value = math.lerp(startPos, endPos, anim.anim.Value.FixedEvaluate(ADChip.traveledTime));

                    //anim.t += dt;
                    //translation.Value.y = anim.anim.Value.Evaluate(anim.t);


                }
                if (anim.t > 1)
                {
                    anim.t = 1;
                }
                // translation.Value = math.mul()
                // if(ADChip.srcPos.)
            }).ScheduleParallel(); // Schedule(Dependency);
            }

            // result part, move losing chips to npc dealer.
            if (chipBettingInfo.bCanMoveToDealer)
            {
                ADResultPartInfoStoring resultPart = ResourceContainer.Get<ADResultPartInfoStoring>();
                Entities
                // .WithNone<ADChipExcludingTag>() // comment out it when testing movement
                      .WithoutBurst()
                .ForEach((
                    Entity chipEntity,
                    ref ADChipTag ADChip,
                    in Translation translation) =>
                        {

                            var temp = ResourceContainer.Get<ADResultPartInfoStoring>().winBetPlace;
                            var tempDealerPos = ResourceContainer.Get<Transform>("NPC").position;
                            var tempDealerPosX = UnityEngine.Random.Range(tempDealerPos.x - 6f, tempDealerPos.x + 6f);
                            var tempDealerPosY = UnityEngine.Random.Range(tempDealerPos.y - 2f, tempDealerPos.y + 2f);
                            tempDealerPos = new float3(tempDealerPosX, tempDealerPosY, tempDealerPos.z);

                            if (temp.Contains(ADChip.bettingPlace) == false && ADChip.bIsLose == false)
                            {
                                ADChip.srcPos = translation.Value;
                                ADChip.dstPos = tempDealerPos;
                                ADChip.bIsLose = true;
                                ADChip.traveledTime = 0;
                                resultPart.currentLoseChips++;
                        // ADChip.bCanBeDestroyed = true; // destroy start when lose and retrive to dealer...
                        // ADChip.bCanBeDestroyedWithAlpha = true;

                    }

                        }).Run(); //.ScheduleParallel();


                // ResourceContainer.Get<ADChipBettingManager>().bCanReturn = false;
            }

            if (chipBettingInfo.bCanMoveToWinPlayer)
            {
                Entities
                    .WithoutBurst()
                    .ForEach((Entity entity, ref ADChipTag chip, in Translation translation) =>
                    {
                        var tempPlayerPos = ResourceContainer.Get<GameUserPosition>(chip.winUserIndex).userPos.position;
                        var tempPlayerPosX = UnityEngine.Random.Range(tempPlayerPos.x - 3f, tempPlayerPos.x + 3f);
                        var tempPlayerPosY = UnityEngine.Random.Range(tempPlayerPos.y - 1f, tempPlayerPos.y + 1f);
                        tempPlayerPos = new float3(tempPlayerPosX, tempPlayerPosY, tempPlayerPos.z);

                        if (chip.winUserIndex != -1 && chip.winUserIndex != 99)
                        {
                            chip.srcPos = translation.Value;
                            // chip.dstPos = ResourceContainer.Get<GameUserPosition>(chip.winUserIndex).userPos.position;
                            chip.dstPos = tempPlayerPos;
                            chip.traveledTime = 0;
                        }
                    }).Run();
            }

        }
        // throw new System.NotImplementedException();
    }

}
