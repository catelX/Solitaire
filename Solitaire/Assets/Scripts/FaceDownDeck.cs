using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public class FaceDownDeck : MonoBehaviour, ISnapCardToPosition
{
    private List<Card> closedCards = new List<Card>();
    public FaceUpDeck faceUpDeck;

    public void SnapCardsToPosition()
    {
        for (int i = 0; i < closedCards.Count; i++)
        {
            closedCards[i].gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (0.1f * (i + 1)));
        }
    }

    public void AddToFaceUpDeck()
    {
        Card card = closedCards[closedCards.Count - 1];
        closedCards.Remove(card);
        card.SetFaceUp(true);
        faceUpDeck.AddCard(card);
    }

    public void MoveCardsFromOpenedToClosed()
    {
        Card card = faceUpDeck.RemoveAndReturnTopCard();
        while(card != null)
        {
            card.SetFaceUp(false);
            closedCards.Add(card);
            card = faceUpDeck.RemoveAndReturnTopCard();
        }
        SnapCardsToPosition();
    }

    public void AddCardToFaceDownDeck(Card card)
    {
        closedCards.Add(card);
    }
}
