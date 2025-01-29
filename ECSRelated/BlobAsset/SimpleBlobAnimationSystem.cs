using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

// currently not used, test purpose only.
[DisableAutoCreation]
public class SimpleBlobAnimationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var dt = Time.DeltaTime;
        Entities.ForEach((ref SimpleBlobAnimationTag anim, ref Translation translation) =>
        {
            anim.t += dt;
            translation.Value.y = anim.anim.Value.Evaluate(anim.t);
        }).Run();
    }
}
