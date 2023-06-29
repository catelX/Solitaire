using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public class DeckOfCards : MonoBehaviour
{
    public static DeckOfCards instance;
    public GameObject cardPrefab;
    public List<Sprite> spadeSprites = new();
    public List<Sprite> clubSprites = new();
    public List<Sprite> heartSprites = new();
    public List<Sprite> diamondSprites = new();

    private List<GameObject> deckOfCards = new();

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

    public Card GetRandomCardFromDeck()
    {
        int ranIndex = Random.Range(0, deckOfCards.Count);
        if (deckOfCards.Count == 0) return null;
        GameObject card = deckOfCards[ranIndex];
        deckOfCards.Remove(card);
        return card.GetComponent<Card>();
    }
}
