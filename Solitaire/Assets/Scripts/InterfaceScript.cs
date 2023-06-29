using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public interface IConditionCheck
{
    public bool IsCardTransferable(Card card);
}

public interface IHandleCards
{
    public void AddCardsFromList(List<Card> cardList);

    public void RemoveCardsFromList(List<Card> cardList);

    public List<Card> GetCardListAfter(Card card);
}

public interface ISnapCardToPosition
{
    public void SnapCardsToPosition();
}
