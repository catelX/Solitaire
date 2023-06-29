using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public class CardHolderTop : CardHolder
{
    public bool IsStackComplete()
    {
        if(cards.Count == 13)
        {
            return true;
        }
        return false;
    }
}
