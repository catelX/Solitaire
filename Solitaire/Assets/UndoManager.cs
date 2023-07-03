using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public class UndoManager : MonoBehaviour
{
    private List<CardHolder> sourceHolders = new();
    private List<CardHolder> transferedHolders = new();
    private List<List<Card>> transferedCardLists = new();
    private List<bool> isHeadCardFlipped = new();

    private int registeredMoves = 0;

    public static UndoManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(this.gameObject);
        }
    }

    public void RegisterMove(CardHolder sourceHolder, CardHolder transferedHolder, List<Card> cardList, bool isFaceUp)
    {
        registeredMoves++;
        sourceHolders.Add(sourceHolder);
        transferedHolders.Add(transferedHolder);
        transferedCardLists.Add(cardList);
        isHeadCardFlipped.Add(isFaceUp);
    }

    public void UndoMove()
    {
        if (registeredMoves == 0) return;
        registeredMoves--;
        bool isFaceUp = isHeadCardFlipped[^1];
        transferedHolders[^1].RemoveCardsFromList(transferedCardLists[^1]);
        if (!isFaceUp && sourceHolders[^1].cards.Count != 0)
        {
            sourceHolders[^1].cards[^1].SetFaceUp(false);
        }
        sourceHolders[^1].AddCardsFromList(transferedCardLists[^1]);

        transferedCardLists.Remove(transferedCardLists[^1]);
        transferedHolders.Remove(transferedHolders[^1]);
        sourceHolders.Remove(sourceHolders[^1]);
        AudioManager.instance.PlayCardUndoClip();
    }
}
