using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public class MainManager : MonoBehaviour
{
    public List<GameObject> cardHoldersTop = new List<GameObject>();
    public List<GameObject> cardHoldersBottom = new List<GameObject>();
    public GameObject faceDownDeck;
    public LayerMask layerMask;

    private Vector3 mousePos;
    private List<Card> pickedUpCards = new();
    private GameObject currentCardHolder;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = -5f;

        if(Input.GetMouseButtonDown(0))
        {
            HandleMouseClickRayCast(mousePos);
        }
        if(Input.GetMouseButton(0))
        {
            MoveCards(mousePos);
        }
        if(Input.GetMouseButtonUp(0))
        {
            HandleMouseReleaseRayCast(mousePos);

            pickedUpCards.Clear();
            currentCardHolder = null;
        }
    }

    private void MoveCards(Vector3 _mousePos)
    {
        for (int i = 1; i < pickedUpCards.Count+1; i++)
        {
            pickedUpCards[i-1].gameObject.transform.position = new Vector3(_mousePos.x, _mousePos.y - (0.4f * i), _mousePos.z - (0.1f * i));
        }
    }

    private void HandleMouseClickRayCast(Vector3 mousePos)
    {
        RaycastHit2D[] rays = Physics2D.RaycastAll(mousePos, transform.forward, 10);
        for (int i = 0; i < rays.Length; i++)
        {
            if (rays[rays.Length-1].collider.CompareTag("FaceDownDeck"))
            {
                if (rays.Length - 1 == 0)
                {
                    rays[0].collider.gameObject.GetComponent<FaceDownDeck>().MoveCardsFromOpenedToClosed();
                }
                else
                {
                    rays[rays.Length - 1].collider.gameObject.GetComponent<FaceDownDeck>().AddToFaceUpDeck();
                    return;
                }
            }

            if (rays[i].collider.CompareTag("Card") && pickedUpCards.Count == 0)
            {
                Card card = rays[i].collider.gameObject.GetComponent<Card>();
                if (card.IsFaceUp())
                {
                    currentCardHolder = rays[rays.Length - 1].collider.gameObject;
                    pickedUpCards = currentCardHolder.GetComponent<IHandleCards>().GetCardListAfter(card);
                }
            }  
        }
    }

    private void HandleMouseReleaseRayCast(Vector3 mousePos)
    {
        if (pickedUpCards.Count == 0) return;

        RaycastHit2D ray = Physics2D.Raycast(mousePos, transform.forward, 10, layerMask);
        if(ray.collider == null || ray.collider.gameObject == currentCardHolder)
        {
            currentCardHolder.GetComponent<ISnapCardToPosition>().SnapCardsToPosition();
            return;
        }

        if (ray.collider.tag != "FaceDownDeck" && ray.collider.tag != "FaceUpDeck")
        {
            if(ray.collider.gameObject.GetComponent<IConditionCheck>().IsCardTransferable(pickedUpCards[0]))
            {
                ray.collider.gameObject.GetComponent<IHandleCards>().AddCardsFromList(pickedUpCards);
                ray.collider.gameObject.GetComponent<ISnapCardToPosition>().SnapCardsToPosition();
                currentCardHolder.GetComponent<IHandleCards>().RemoveCardsFromList(pickedUpCards);
                currentCardHolder.GetComponent<ISnapCardToPosition>().SnapCardsToPosition();
            }
            else
            {
                currentCardHolder.GetComponent<ISnapCardToPosition>().SnapCardsToPosition();
            }
        } 
    }

    private void CheckClearCondition()
    {
        int stacksCompleted = 0;
        foreach(GameObject cardholder in cardHoldersTop)
        {
            if(cardholder.GetComponent<CardHolderTop>().IsStackComplete())
            {
                stacksCompleted++;
            }
        }

        if(stacksCompleted == 4)
        {
            Debug.Log("You Win!");
        }
    }

    public void Initialize()
    {
        Card card;

        for(int i = 0; i < cardHoldersBottom.Count; i++)
        {
            for(int j = 0; j <= i; j++)
            {
                card = DeckOfCards.instance.GetRandomCardFromDeck();
                cardHoldersBottom[i].GetComponent<CardHolderBottom>().AddCardWithoutCondition(card);
            }
            cardHoldersBottom[i].GetComponent<CardHolderBottom>().SnapCardsToPosition();
        }
        card = DeckOfCards.instance.GetRandomCardFromDeck();
        while(card != null)
        {
            faceDownDeck.GetComponent<FaceDownDeck>().AddCardToFaceDownDeck(card);
            card = DeckOfCards.instance.GetRandomCardFromDeck();
        }
        faceDownDeck.GetComponent<FaceDownDeck>().SnapCardsToPosition();
    }
}
