using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADDishDiceSymbol : ResourceItemBase
{
    public List<Sprite> diceSymbols = new List<Sprite>(6);
    public int diceCount = 3;

    public Sprite[] GetRandomSymbols()
    {
        var tempInt1 = Random.Range(0, 5);
        var tempInt2 = Random.Range(0, 5);
        var tempInt3 = Random.Range(0, 5);
        


        return new Sprite[] { diceSymbols[tempInt1], diceSymbols[tempInt2], diceSymbols[tempInt3] };

    }

}
