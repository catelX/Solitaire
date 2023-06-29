using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public class FaceUpDeck : CardHolder
{
    public FaceDownDeck faceDownDeck;

    public override void SnapCardsToPosition()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (0.1f * (i + 1)));
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
        Card card = cards[cards.Count - 1];
        cards.Remove(card);
        return card;
    }
}
