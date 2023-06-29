using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public class CardHolder : MonoBehaviour
{
    public List<Card> cards = new List<Card>();


    public void AddCardWithoutCondition(Card card)
    {
        cards.Add(card);
    }

    public void RemoveCard(Card card)
    {
        cards.Remove(card);
    }

    public void AddCardsFromList(List<Card> cardList)
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            cards.Add(cardList[i]);
        }
    }

    public void RemoveCardsFromList(List<Card> cardList)
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            cards.Remove(cardList[i]);
        }
    }

    public List<Card> GetCardListAfter(Card _card)
    {
        int index = 0;
        List<Card> cardList = new();

        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i] == _card)
            {
                index = i;
                break;
            }
        }

        for (int i = index; i < cards.Count; i++)
        {
            cardList.Add(cards[i]);
        }

        return cardList;
    }
}
