#if !UNITY_IOS
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisableAutoCreation]
public class ADChipModifyingSpriteSystem : SystemBase
{

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
        if (ResourceContainer.Get<ADChipBettingManager>().gameObject.activeInHierarchy == true)
        {
            if (ResourceContainer.Get<ADChipBettingManager>().bDestroyingAllChipEntitiesWithAlpha == true)
            {
                var deltaTime = Time.DeltaTime;
                var t = Time.DeltaTime / ResourceContainer.Get<ADChipBettingManager>().destroyingTime;
                // var tempWinPlaces = ResourceContainer.Get<ADResultPartInfoStoring>().winBetPlace;
                Entities
                    .WithoutBurst()
                    // .WithAll<ADChipExcludingTag>() // must contain this component to foreach iteration
                    .ForEach((SpriteRenderer spriteRenderer, ref ADChipTag adchip) =>
                    {
                        if(adchip.bIsLose == true)
                        {
                            return;
                        }
                        adchip.alphaTime += t;

                        if (adchip.alphaTime > 1)
                        {
                            adchip.alphaTime = 1;
                            adchip.bCanBeDestroyed = true;
                        }
                        var alphaValue = math.lerp(1, 0, adchip.alphaTime);
                        spriteRenderer.color = new Color(1, 1, 1, alphaValue);

                    }).Run();
            }

            if (ResourceContainer.Get<ADChipBettingManager>().bDestroyingOnlyLoseChipsWithAlpha == true)
            {
                var deltaTime = Time.DeltaTime;
                var t = Time.DeltaTime / ResourceContainer.Get<ADChipBettingManager>().destroyingTime;
                // var tempWinPlaces = ResourceContainer.Get<ADResultPartInfoStoring>().winBetPlace;
                Entities
                    .WithoutBurst()
                    // .WithAll<ADChipExcludingTag>() // must contain this component to foreach iteration
                    .ForEach((SpriteRenderer spriteRenderer, ref ADChipTag adchip) =>
                    {
                        if (adchip.bIsLose == false)
                        {
                            return;
                        }
                        adchip.alphaTime += t;

                        if (adchip.alphaTime > 1)
                        {
                            adchip.alphaTime = 1;
                            adchip.bCanBeDestroyed = true;
                        }
                        var alphaValue = math.lerp(1, 0, adchip.alphaTime);
                        spriteRenderer.color = new Color(1, 1, 1, alphaValue);

                    }).Run();
            }


        }
        
    }
}

#endif