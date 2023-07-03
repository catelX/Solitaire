using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public class CardHolder : MonoBehaviour
{
    public List<Card> cards = new();
    private readonly Color[] colors = new Color[2];

    public void Awake()
    {
        colors[0] = Color.yellow;
        colors[1] = Color.white;
    }

    public int Count()
    {
        return cards.Count;
    }

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
        int topCardValue = cards[^1].GetValue();

        if (card.GetValue() == topCardValue + 1 && card.GetSymbol() == cards[^1].GetSymbol())
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

    public virtual void ActivateCardHighlight()
    {
        if (cards.Count != 0)cards[^1].gameObject.GetComponent<SpriteRenderer>().color = colors[0];
    }

    public virtual void DeactivateCardHighlight()
    {
        if (cards.Count != 0)cards[^1].gameObject.GetComponent<SpriteRenderer>().color = colors[1];
    }

    public bool IsTopCard(Card card)
    {
        if(card == cards[^1])
        {
            return true;
        }
        return false;
    }


    public virtual void AddCardsFromList(List<Card> cardList)
    {
        DeactivateCardHighlight();
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

    public virtual List<Card> GetAllCardsList()
    {
        List<Card> cardList = new();

        for (int i = 0; i < cards.Count; i++)
        {
            cardList.Add(cards[i]);
        }

        return cardList;
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
