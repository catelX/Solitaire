using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public class DeckOfCards : MonoBehaviour
{
    public static DeckOfCards instance;
    public GameObject cardPrefab;
    public List<Sprite> backImages = new();
    public List<Sprite> spadeSprites = new();
    public List<Sprite> clubSprites = new();
    public List<Sprite> heartSprites = new();
    public List<Sprite> diamondSprites = new();

    private List<GameObject> deckOfCards = new();
    private List<Card> referenceToAllCards = new();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        CreateDeckOfCards();
        AddToReferenceList();
    }

    public void ChangeBackImageToRed()
    {
        foreach(Card card in referenceToAllCards)
        {
            card.ChangeBackImage(backImages[0]);
        }
    }
    public void ChangeBackImageToGreen()
    {
        foreach (Card card in referenceToAllCards)
        {
            card.ChangeBackImage(backImages[1]);
        }
    }
    public void ChangeBackImageToBlue()
    {
        foreach (Card card in referenceToAllCards)
        {
            card.ChangeBackImage(backImages[2]);
        }
    }
    public void ChangeBackImageToBlack()
    {
        foreach (Card card in referenceToAllCards)
        {
            card.ChangeBackImage(backImages[3]);
        }
    }

    private void CreateDeckOfCards()
    {
        for (int i = 0; i < 13; i++)
        {
            GameObject card = Instantiate(cardPrefab);
            Sprite cardSprite = spadeSprites[i];
            card.GetComponent<Card>().ConfigureCard(cardSprite, i + 1, Symbols.Spades);
            deckOfCards.Add(card);

            card = Instantiate(cardPrefab);
            cardSprite = clubSprites[i];
            card.GetComponent<Card>().ConfigureCard(cardSprite, i + 1, Symbols.Clubs);
            deckOfCards.Add(card);
        }

        for (int i = 0; i < 13; i++)
        {
            GameObject card = Instantiate(cardPrefab);
            Sprite cardSprite = heartSprites[i];
            card.GetComponent<Card>().ConfigureCard(cardSprite, i + 1, Symbols.Hearts);
            deckOfCards.Add(card);

            card = Instantiate(cardPrefab);
            cardSprite = diamondSprites[i];
            card.GetComponent<Card>().ConfigureCard(cardSprite, i + 1, Symbols.Diamonds);
            deckOfCards.Add(card);
        }
    }

    private void AddToReferenceList()
    {
        foreach(GameObject card in deckOfCards)
        {
            Card newCard = card.GetComponent<Card>();
            referenceToAllCards.Add(newCard);
        }
    }

    public void AddCardsToDeckFromList(List<Card> cardList)
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            deckOfCards.Add(cardList[i].gameObject);
        }
    }

    public void AddCard(Card card)
    {
        deckOfCards.Add(card.gameObject);
    }

    public Card GetRandomCardFromDeck()
    {
        int ranIndex = Random.Range(0, deckOfCards.Count);
        if (deckOfCards.Count == 0) return null;
        GameObject card = deckOfCards[ranIndex];
        deckOfCards.Remove(card);
        return card.GetComponent<Card>();
    }
}
