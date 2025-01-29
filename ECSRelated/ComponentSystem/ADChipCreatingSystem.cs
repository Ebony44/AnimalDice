#if UNITY_IOS

// Create from ADChipSpawner_FromMonoBehaviour.cs
// create middle of result part.

using System.Collections.Generic;

using Unity.Entities;

// using Unity.Physics.Systems;
using UnityEngine;

// using Random = Unity.Mathematics.Random;

// Systems can schedule work to run on worker threads.
// However, creating and removing Entities can only be done on the main thread to prevent race conditions.
// The system uses an EntityCommandBuffer to defer tasks that can't be done inside the Job.

// ReSharper disable once InconsistentNaming
// [UpdateInGroup(typeof(SimulationSystemGroup))]
[DisableAutoCreation]
// [UpdateBefore(typeof(BuildPhysicsWorld))]
public class ADChipCreatingSystem : SystemBase
{
    protected override void OnUpdate()
    {
        if(ResourceContainer.Instance == null)
        {
            
            return;
        }
        if (ResourceContainer.Get<ADChipBettingManager>() == null)
        {
            return;
        }
        if(ResourceContainer.Get<ADChipBettingManager>().bCanMoveToBoard == false)
        {
            return;
        }
        var winInfo = ResourceContainer.Get<ADResultPartInfoStoring>();
        var bettingFunction = ResourceContainer.Get<ADChipBettingManager>();
        var currentChips = new Dictionary<eAD_BUTTONLIST, int>(capacity: 4);
        if (ResourceContainer.Get<ADChipBettingManager>().bCanMoveToBoard) // from dealer to board
        {
            Job
                .WithoutBurst()
                .WithCode(() =>
           {
               // 1. win bet place
               // 2. win users
               // 3. how much money to translate
               foreach (var winBetPlace in winInfo.winBetPlace) // from win bet place
               {
                   var users = winInfo.winningUsersChips[winBetPlace];
                   for (int userIndex = 0; userIndex < users.Count; userIndex++) // from users
                   {
                       if (users[userIndex] > 0)
                       {
                           currentChips = bettingFunction.GetChipsFromMoney(users[userIndex]);

                           foreach (var item in currentChips) // from chip kind's count
                           {

                               ResourceContainer.Get<ADChipSpawner_FromMonoBehaviour>().SpawnChipsToSpawnPos(
                                   chipStartPos: ResourceContainer.Get<Transform>("NPC").transform.position,
                                   chipMidPos: Vector3.zero,
                                   chipTargetPos: bettingFunction.GetTablePositionFromBetPlace(winBetPlace),
                                   paramBettingPlace: winBetPlace,
                                   paramChipKind: item.Key,
                                   bettingCount: item.Value,
                                   winUserIndex: userIndex
                                   );

                           }
                           users[userIndex] = 0;
                       }
                   }
               }

           }).Run();
        }
        

    }
}


#endif