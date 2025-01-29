using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADButtonIndexTag : MonoBehaviour
{
    [SerializeField] private eAD_BUTTONLIST buttonIndex;
    public void OnChipSelecting()
    {
        ResourceContainer.Get<ADChipBettingManager>().OnChipSelecting(buttonIndex);
    }
    public void OnPrevButtonTapped()
    {
        ResourceContainer.Get<ADChipBettingManager>().OnPrevButtonTapped(buttonIndex);
    }

}
