using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public class FaceUpDeck : CardHolder
{
    public FaceDownDeck faceDownDeck;

    public override void SnapCardsToPosition()
    {
        int index = 0;
        int zIndex = 0;
        for (int i = 0; i < cards.Count-3; i++)
        {
            cards[i].gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (0.1f * (i + 1)));
            zIndex++;
        }
        for (int i = 3; i > 0; i--)
        {
            if (cards.Count-i < 0)
            {
                continue;
            }
            cards[^i].gameObject.transform.position = new Vector3(transform.position.x + (index * 0.4f), transform.position.y, transform.position.z - (0.1f * (zIndex + 1)));
            index++;
            zIndex++;
        }
    }

    public void AddCard(Card card)
    {
        AddCardWithoutCondition(card);
        SnapCardsToPosition();
    }

    public Card RemoveAndReturnTopCard()
    {
        if (cards.Count == 0) return null;
        Card card = cards[^1];
        cards.Remove(card);
        return card;
    }
}
