using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public class FaceDownDeck : CardHolder
{
    public FaceUpDeck faceUpDeck;

    public override void SnapCardsToPosition()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (0.1f * (i + 1)));
        }
    }

    public void AddToFaceUpDeck()
    {
        Card card = cards[^1];
        cards.Remove(card);
        card.SetFaceUp(true);
        faceUpDeck.AddCard(card);
    }

    public void MoveCardsFromOpenedToClosed()
    {
        Card card = faceUpDeck.RemoveAndReturnTopCard();
        while(card != null)
        {
            card.SetFaceUp(false);
            cards.Add(card);
            card = faceUpDeck.RemoveAndReturnTopCard();
        }
        SnapCardsToPosition();
    }

    public void AddCardToFaceDownDeck(Card card)
    {
        cards.Add(card);
    }
}
