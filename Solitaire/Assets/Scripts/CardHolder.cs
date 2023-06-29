using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public class CardHolder : MonoBehaviour
{
    public List<Card> cards = new();

    public virtual void SnapCardsToPosition()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (0.1f * (i + 1)));
            if (i == cards.Count - 1)
            {
                cards[i].SetFaceUp(true);
            }
        }
    }

    public virtual bool IsCardTransferable(Card card)
    {
        if (cards.Count == 0)
        {
            if (card.GetValue() == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        int topCardValue = cards[cards.Count - 1].GetValue();

        if (card.GetValue() == topCardValue + 1 && card.GetSymbol() == cards[cards.Count - 1].GetSymbol())
        {
            return true;
        }

        return false;
    }

    public virtual void AddCardWithoutCondition(Card card)
    {
        cards.Add(card);
    }

    public virtual void RemoveCard(Card card)
    {
        cards.Remove(card);
    }

    public virtual void AddCardsFromList(List<Card> cardList)
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            cards.Add(cardList[i]);
        }
        SnapCardsToPosition();
    }

    public virtual void RemoveCardsFromList(List<Card> cardList)
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            cards.Remove(cardList[i]);
        }
        SnapCardsToPosition();
    }

    public virtual List<Card> GetCardListAfter(Card _card)
    {
        int index = 0;
        List<Card> cardList = new();
        //Get the index of the card
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i] == _card)
            {
                index = i;
                break;
            }
        }
        //return a list of all following cards after index
        for (int i = index; i < cards.Count; i++)
        {
            cardList.Add(cards[i]);
        }

        return cardList;
    }
}
