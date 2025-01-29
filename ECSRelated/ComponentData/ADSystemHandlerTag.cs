using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct ADSystemHandlerTag : IComponentData
{
    public bool bCanMove;

    public bool bCanMoveToDealer;
    public bool bCanMarkChips;
    public bool bCanMoveToBoard;
    public bool bCanMoveToWinPlayer;

    public bool bDestroyingChipEntities;

    public bool bDestroyingAllChipEntitiesWithAlpha;
    public bool bDestroyingOnlyLoseChipsWithAlpha;

}
