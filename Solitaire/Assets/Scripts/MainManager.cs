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
    private GameObject pickedUpCard;

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
            if(pickedUpCard != null)
                pickedUpCard.transform.position = mousePos;
        }
        if(Input.GetMouseButtonUp(0))
        {
            SetAllCardsToPosition();
            pickedUpCard = null;
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

                rays[rays.Length - 1].collider.gameObject.GetComponent<FaceDownDeck>().AddToFaceUpDeck();
                return;
            }

            if (rays[i].collider.CompareTag("Card") && pickedUpCard == null)
            {
                pickedUpCard = rays[i].collider.gameObject;
            }  
        }
    }

    private void HandleMouseReleaseRayCast(Vector3 mousePos)
    {
        RaycastHit2D ray = Physics2D.Raycast(mousePos, transform.forward, 10, layerMask);

        if (LayerMask.LayerToName(ray.collider.gameObject.layer) == "CardHolder")
        {
            ray.collider.gameObject.GetComponents<IConditionCheck>().CheckAndAddCard(pickedUpCard);
        }
    }

    private void SetAllCardsToPosition()
    {
        foreach(GameObject cardHolder in cardHoldersTop)
        {
            cardHolder.GetComponent<CardHolderTop>().SnapCardsToPosition();
        }
        foreach(GameObject cardHolder in cardHoldersBottom)
        {
            cardHolder.GetComponent<CardHolderBottom>().SnapCardsToPosition();
        }
        faceDownDeck.GetComponent<FaceDownDeck>().SnapCardsToPosition();
    }

    public void Initialize()
    {
        GameObject card;

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
