using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public class FaceDownDeck : MonoBehaviour
{
    private List<GameObject> closedCards = new List<GameObject>();
    private List<GameObject> openedCards = new List<GameObject>();
    private Vector3 myPos;
    public GameObject faceUpDeck;

    private void Awake()
    {
        myPos = transform.position;
    }

    public void SnapCardsToPosition()
    {
        for (int i = 0; i < closedCards.Count; i++)
        {
            closedCards[i].transform.position = new Vector3(myPos.x, myPos.y, myPos.z - (0.1f * (i + 1)));
        }

        Vector3 faceUpDeckPos = faceUpDeck.transform.position;

        for (int i = 0; i < openedCards.Count; i++)
        {
            openedCards[i].transform.position = new Vector3(faceUpDeckPos.x, faceUpDeckPos.y, faceUpDeckPos.z - (0.1f * (i + 1)));
        }
    }

    public void AddToFaceUpDeck()
    {
        GameObject card = closedCards[closedCards.Count - 1];
        closedCards.Remove(card);
        card.GetComponent<Card>().SetFaceUp(true);
        openedCards.Add(card);
        SnapCardsToPosition();
    }

    public void MoveCardsFromOpenedToClosed()
    {
        int currentCount = openedCards.Count;
        for (int i = currentCount-1; i >= 0; i--)
        {
            Debug.Log("ASDSDA");
            GameObject card = openedCards[i];
            openedCards.Remove(card);
            card.GetComponent<Card>().SetFaceUp(false);
            closedCards.Add(card);
        }
        SnapCardsToPosition();
    }

    public void AddCardToFaceDownDeck(GameObject card)
    {
        closedCards.Add(card);
    }
}
