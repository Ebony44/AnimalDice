using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADDishSpineItem : ResourceItemBase
{
    public SkeletonAnimation spine;
    public MeshRenderer mesh;
    

    [TestMethod(false)]
    public void SetUpWithSpecialRank()
    {
        // 1_top
        // 1_under
        // 2_top
        // 2_under
        // 3_top
        // 3_under
        spine.skeletonDataAsset = ResourceContainer.Get<ADDishDiceManager>().dishSpineList[1];
        spine.Initialize(true);
        spine.ClearState();
    }
    [TestMethod(false)]
    public void SetUpWithOrdinaryRank()
    {
        // close_top
        // close_under
        // open_top
        // open_under
        
        spine.skeletonDataAsset = ResourceContainer.Get<ADDishDiceManager>().dishSpineList[0];
        spine.Initialize(true);
        spine.ClearState();
    }
    [TestMethod(false)]
    public void TestPlayCurrentSpine(string animationID)
    {
        spine.Play(animationID);
    }
}
