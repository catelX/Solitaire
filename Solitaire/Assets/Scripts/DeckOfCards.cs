using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public class DeckOfCards : MonoBehaviour
{
    public static DeckOfCards instance;
    public GameObject cardPrefab;
    public List<Sprite> spadeSprites = new List<Sprite>();
    public List<Sprite> clubSprites = new List<Sprite>();
    public List<Sprite> heartSprites = new List<Sprite>();
    public List<Sprite> diamondSprites = new List<Sprite>();

    private List<GameObject> deckOfCards = new List<GameObject>();

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
            card.GetComponent<Card>().ConfigureCard(cardSprite, i + 1, false, "Spades");
            deckOfCards.Add(card);

            card = Instantiate(cardPrefab);
            cardSprite = clubSprites[i];
            card.GetComponent<Card>().ConfigureCard(cardSprite, i + 1, false, "Clubs");
            deckOfCards.Add(card);
        }

        for (int i = 0; i < 13; i++)
        {
            GameObject card = Instantiate(cardPrefab);
            Sprite cardSprite = heartSprites[i];
            card.GetComponent<Card>().ConfigureCard(cardSprite, i + 1, true, "Hearts");
            deckOfCards.Add(card);

            card = Instantiate(cardPrefab);
            cardSprite = diamondSprites[i];
            card.GetComponent<Card>().ConfigureCard(cardSprite, i + 1, true, "Diamonds");
            deckOfCards.Add(card);
        }
    }

    public GameObject GetRandomCardFromDeck()
    {
        int ranIndex = Random.Range(0, deckOfCards.Count);
        if (deckOfCards.Count == 0) return null;
        GameObject card = deckOfCards[ranIndex];
        deckOfCards.Remove(card);
        return card;
    }
}
