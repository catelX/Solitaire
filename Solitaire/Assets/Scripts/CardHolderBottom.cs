using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public class CardHolderBottom : CardHolder, IConditionCheck, IHandleCards, ISnapCardToPosition
{

    public void SnapCardsToPosition()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - (0.4f * i), transform.position.z - (0.1f * (i + 1)));
            if (i == cards.Count - 1)
            {
                cards[i].SetFaceUp(true);
            }
        }
    }

    public bool IsCardTransferable(Card card)
    {
        if(cards.Count == 0)
        {
            if(card.GetValue() == 13)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        int topCardValue = cards[cards.Count - 1].GetValue();

        if (card.GetValue() == topCardValue - 1 && card.IsRed() != cards[cards.Count - 1].IsRed())
        {
            return true;
        }

        return false;
    }
}
