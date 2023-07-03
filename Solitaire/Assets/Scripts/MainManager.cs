using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public class MainManager : MonoBehaviour
{
    public List<CardHolderTop> cardHoldersTop = new();
    public List<CardHolderBottom> cardHoldersBottom = new();
    public FaceDownDeck faceDownDeck;
    public LayerMask layerMask;

    private Vector3 mousePos;
    private Vector3 offSet;

    private List<Card> pickedUpCards = new();
    private List<CardHolder> sourceHolders = new();
    private List<CardHolder> transferedHolders = new();
    private List<List<Card>> transferedCardLists = new();
    private CardHolder sourceCardHolder;
    private int registeredMoves = 0;

    private CardHolder highlightedHolder;
    private bool mouseClickedOnce;
    private float mouseSecondClickTimer;

    public GameObject gameWinPanel;
    private bool hasWon;

    private void Start()
    {
        hasWon = false;
        Cursor.visible = false;
        Initialize();
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = -5f;

        if (hasWon) return;

        if(Input.GetKeyDown(KeyCode.Z))
        {
            UndoManager.instance.UndoMove();
            return;
        }

        if(mouseClickedOnce)mouseSecondClickTimer += Time.deltaTime;
        if(mouseSecondClickTimer >= 0.2f)
        {
            mouseSecondClickTimer = 0;
            mouseClickedOnce = false;
        }

        if(Input.GetMouseButtonDown(0))
        {
            if(mouseClickedOnce)
            {
                mouseSecondClickTimer = 0;
                HandleDoubleMouseClick(mousePos);
                mouseClickedOnce = false;
            }
            else
            {
                mouseClickedOnce = true;
                HandleMouseClickRayCast(mousePos);
            }
            return;
        }
        if(Input.GetMouseButton(0))
        {
            MoveCards(mousePos);
            HandleMouseMoveBoxCast();
        }
        if(Input.GetMouseButtonUp(0))
        {
            HandleMouseReleaseBoxCast(mousePos);
            CheckClearCondition();
            pickedUpCards.Clear();
            sourceCardHolder = null;
            if(highlightedHolder != null)
            {
                highlightedHolder.DeactivateCardHighlight();
                highlightedHolder = null;
            }
        }
    }

    private void MoveCards(Vector3 _mousePos)
    {
        for (int i = 0; i < pickedUpCards.Count; i++)
        {
            Vector3 newPos = new (_mousePos.x, _mousePos.y - (0.4f * i), _mousePos.z - (0.1f + (0.1f * i)));
            newPos.x -= offSet.x;
            newPos.y -= offSet.y;
            pickedUpCards[i].gameObject.transform.position = newPos;
        }
    }


    private void HandleMouseMoveBoxCast()
    {
        GameObject firstCard;
        if (pickedUpCards.Count == 0) return;

        firstCard = pickedUpCards[0].gameObject;

        RaycastHit2D hit = Physics2D.BoxCast(firstCard.transform.position, firstCard.GetComponent<BoxCollider2D>().size, 0, Vector2.zero, 10, layerMask);
        if (hit.collider == null)
        {
            if (highlightedHolder != null)
            {
                highlightedHolder.DeactivateCardHighlight();
                highlightedHolder = null;
            }
            return;
        }

        if(hit.collider.gameObject.GetComponent<CardHolder>().IsCardTransferable(firstCard.GetComponent<Card>()))
        {
            highlightedHolder = hit.collider.gameObject.GetComponent<CardHolder>();
            highlightedHolder.ActivateCardHighlight();
        }
    }

    private void HandleMouseClickRayCast(Vector3 _mousePos)
    {
        RaycastHit2D[] rays = Physics2D.RaycastAll(_mousePos, transform.forward, 10);
        for (int i = 0; i < rays.Length; i++)
        {
            if (rays[^1].collider.CompareTag("FaceDownDeck"))
            {
                if (rays.Length - 1 == 0)
                {
                    rays[0].collider.gameObject.GetComponent<FaceDownDeck>().MoveCardsFromOpenedToClosed();
                    AudioManager.instance.PlayCardShuffleClip();
                }
                else
                {
                    rays[^1].collider.gameObject.GetComponent<FaceDownDeck>().AddToFaceUpDeck();
                    AudioManager.instance.PlayCardReleasedClip();
                }
                return;
            }

            if (rays[i].collider.CompareTag("Card") && pickedUpCards.Count == 0)
            {
                Card card = rays[i].collider.gameObject.GetComponent<Card>();
                if (card.IsFaceUp())
                {
                    sourceCardHolder = rays[^1].collider.gameObject.GetComponent<CardHolder>();
                    pickedUpCards = sourceCardHolder.GetCardListAfter(card);
                    SetOffSet(mousePos, pickedUpCards[0].gameObject.transform.position);
                    AudioManager.instance.PlayCardPickedClip();
                }
            }  
        }
    }

    private void SetOffSet(Vector3 firstPoint, Vector3 secondPoint)
    {
        offSet.x = firstPoint.x - secondPoint.x;
        offSet.y = firstPoint.y - secondPoint.y;
        offSet.z = 0;
    }

    private void HandleMouseReleaseBoxCast(Vector3 _mousePos)
    {
        GameObject firstCard;
        if (pickedUpCards.Count == 0) return;

        firstCard = pickedUpCards[0].gameObject;

        RaycastHit2D[] hits = Physics2D.BoxCastAll(_mousePos, firstCard.gameObject.GetComponent<BoxCollider2D>().size, 0, Vector2.zero,10,layerMask);
        if(hits.Length == 0) sourceCardHolder.SnapCardsToPosition();

        for (int i = 0; i < hits.Length; i++)
        {
            CardHolder transferedHolder = hits[i].collider.gameObject.GetComponent<CardHolder>();
            if (hits[i].collider == null || hits[i].collider.gameObject == sourceCardHolder
                || hits[i].collider.CompareTag("FaceDownDeck") || hits[i].collider.CompareTag("FaceUpDeck"))
            {
                sourceCardHolder.SnapCardsToPosition();
                return;
            }

            if(hits[i].collider.gameObject.GetComponent<CardHolder>().IsCardTransferable(pickedUpCards[0]))
            {
                bool isFaceUp = sourceCardHolder.IsCardAboveFaceUp(pickedUpCards[0]);
                sourceCardHolder.RemoveCardsFromList(pickedUpCards);
                transferedHolder.AddCardsFromList(pickedUpCards);
                UndoManager.instance.RegisterMove(sourceCardHolder, transferedHolder, pickedUpCards, isFaceUp);
                return;
            }
        }
        sourceCardHolder.SnapCardsToPosition();
    }

    private void HandleDoubleMouseClick(Vector3 _mousePos)
    {
        RaycastHit2D[] rays = Physics2D.RaycastAll(_mousePos, transform.forward, 10);
        if (rays.Length <= 1) return;

        Card card = rays[0].collider.gameObject.GetComponent<Card>();
        CardHolder cardHolder = rays[^1].collider.gameObject.GetComponent<CardHolder>();

        if(card.IsFaceUp() && cardHolder.IsTopCard(card))
        {
            List<Card> cards = new();
            cards.Add(card);
            for (int i = 0; i < cardHoldersTop.Count; i++)
            {
                if(cardHoldersTop[i].IsCardTransferable(card))
                {
                    bool isFaceUp = cardHolder.IsCardAboveFaceUp(card);
                    cardHolder.RemoveCardsFromList(cards);
                    cardHoldersTop[i].AddCardsFromList(cards);
                    UndoManager.instance.RegisterMove(cardHolder, cardHoldersTop[i], cards, isFaceUp);
                    return;
                }
            }
        }
    }

    private void CheckClearCondition()
    {
        int stacksCompleted = 0;
        foreach(CardHolderTop cardholder in cardHoldersTop)
        {
            if(cardholder.IsStackComplete())
            {
                stacksCompleted++;
            }
        }

        if(stacksCompleted == 4)
        {
            hasWon = true;
            gameWinPanel.SetActive(true);
        }
    }

    public void RestartGame()
    {
        for (int i = 0; i < cardHoldersTop.Count; i++)
        {
            List<Card> cardList = cardHoldersTop[i].GetAllCardsList();
            DeckOfCards.instance.AddCardsToDeckFromList(cardList);
        }
        hasWon = false;
        Initialize();
        gameWinPanel.SetActive(false);
    }

    public void Initialize()
    {
        Card card;

        for(int i = 0; i < cardHoldersBottom.Count; i++)
        {
            for(int j = 0; j <= i; j++)
            {
                card = DeckOfCards.instance.GetRandomCardFromDeck();
                cardHoldersBottom[i].AddCardWithoutCondition(card);
            }
            cardHoldersBottom[i].SnapCardsToPosition();
        }
        card = DeckOfCards.instance.GetRandomCardFromDeck();
        while(card != null)
        {
            faceDownDeck.AddCardToFaceDownDeck(card);
            card = DeckOfCards.instance.GetRandomCardFromDeck();
        }
        faceDownDeck.SnapCardsToPosition();
    }
}
