using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public List<CardHolderTop> cardHoldersTop = new();
    public List<CardHolderBottom> cardHoldersBottom = new();
    public FaceDownDeck faceDownDeck;
    public FaceUpDeck faceUpDeck;
    public LayerMask holderMask;
    public ParticleSystem cardHighlightParticle;
    Texture2D screenShot;
    public Image winScreen;

    private Vector3 mousePos;
    private Vector3 offSet;

    private List<Card> pickedUpCards = new();
    private CardHolder sourceCardHolder;

    private CardHolder highlightedHolder;
    private bool mouseClickedOnce;
    private float mouseSecondClickTimer;

    public GameObject gameWinPanel;
    private bool hasWon;
    public bool sfxEnabled;
    public bool isPanel;

    private void Start()
    {
        Camera.onPostRender += OnPostRenderCallBack;
        cardHighlightParticle.Stop();
        sfxEnabled = true;
        isPanel = false;
        hasWon = false;
        Initialize();
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = -5f;

        if (hasWon)
        {
            WinScreen();
            return;
        }
        if (isPanel) return;
        if(sfxEnabled) HandleMouseHoverhighLight(mousePos);

        if (Input.GetKeyDown(KeyCode.Z))
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
                pickedUpCards.Clear();
                sourceCardHolder = null;
                HandleDoubleMouseClick(mousePos);
                mouseClickedOnce = false;
                return;
            }
            else
            {
                mouseClickedOnce = true;
                HandleMouseClickRayCast(mousePos);
            }
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
    private void LateUpdate()
    {
        if(screenShot != null)
        {
            Sprite sprite = Sprite.Create(screenShot, new Rect(0, 0, Screen.width, Screen.height), new Vector2(0, 0));
            if (winScreen)
                winScreen.sprite = sprite;
        }
    }

    public void SFXEnabled(bool isEnabled)
    {
        sfxEnabled = isEnabled;
    }


    private void OnPostRenderCallBack(Camera cam)
    {
        if (!hasWon) return;
        if (cam == Camera.main)
        {
            screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenShot.Apply();
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

    // Mouse Hover, Move, Click, Release, Double Click Handle -----------------------------------------/
    private void HandleMouseHoverhighLight(Vector3 _mousePos)
    {
        if (pickedUpCards.Count == 0)
        {
            RaycastHit2D[] rays = Physics2D.RaycastAll(_mousePos, transform.forward, 10);

            for (int i = 0; i < rays.Length; i++)
            {
                if (rays[0].collider == null || rays.Length == 1)
                {
                    cardHighlightParticle.Stop();
                    return;
                }
                if (rays[^1].collider.CompareTag("FaceUpDeck"))
                {
                    if(rays[^1].collider.GetComponent<CardHolder>().IsTopCard(rays[0].collider.GetComponent<Card>()))
                    {
                        cardHighlightParticle.gameObject.transform.position = rays[0].collider.gameObject.transform.position;
                        cardHighlightParticle.Play();
                    }
                    else
                    {
                        cardHighlightParticle.Stop();
                    }
                }
                else if(rays[0].collider.GetComponent<Card>().IsFaceUp())
                {
                    cardHighlightParticle.gameObject.transform.position = rays[0].collider.gameObject.transform.position;
                    cardHighlightParticle.Play();
                }
                else
                {
                    cardHighlightParticle.Stop();
                }
            }
        }
        else
        {
            cardHighlightParticle.Stop();
        }
    }

    private void HandleMouseMoveBoxCast()
    {
        if (pickedUpCards.Count == 0) return;

        GameObject firstCard;
        firstCard = pickedUpCards[0].gameObject;

        RaycastHit2D hit = Physics2D.BoxCast(firstCard.transform.position, firstCard.GetComponent<BoxCollider2D>().size, 0, Vector2.zero, 10, holderMask);
        if (hit.collider == null)
        {
            if (highlightedHolder != null)
            {
                highlightedHolder.DeactivateCardHighlight();
                highlightedHolder = null;
            }
            return;
        }

        if (hit.collider.gameObject.GetComponent<CardHolder>().IsCardTransferable(firstCard.GetComponent<Card>()))
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
                List<Card> cardList;
                if (rays.Length - 1 == 0)
                {
                    cardList = faceUpDeck.GetAllCardsList();
                    faceDownDeck.MoveCardsFromOpenedToClosed();
                    UndoManager.instance.RegisterMove(faceUpDeck, faceDownDeck, cardList, false);
                    AudioManager.instance.PlayCardShuffleClip();
                }
                else
                {
                    cardList = faceDownDeck.GetThreeCardList();
                    faceUpDeck.AddCardsFromList(cardList);
                    List<Card> inverseList = new();
                    for (int j = cardList.Count - 1; j >= 0; j--)
                    {
                        inverseList.Add(cardList[j]);
                    }
                    UndoManager.instance.RegisterMove(faceDownDeck, faceUpDeck, inverseList, false);
                    AudioManager.instance.PlayCardReleasedClip();
                }
                return;
            }

            if (rays[i].collider.CompareTag("Card") && pickedUpCards.Count == 0)
            {
                if (rays[^1].collider.CompareTag("FaceUpDeck"))
                {
                    if(rays[^1].collider.GetComponent<CardHolder>().IsTopCard(rays[0].collider.GetComponent<Card>()))
                    {
                        sourceCardHolder = rays[^1].collider.gameObject.GetComponent<CardHolder>();
                        pickedUpCards = sourceCardHolder.GetCardListAfter(rays[0].collider.GetComponent<Card>());
                        SetCardOffSet(mousePos, pickedUpCards[0].gameObject.transform.position);
                        AudioManager.instance.PlayCardPickedClip();
                    }
                }
                else
                {
                    Card card = rays[i].collider.gameObject.GetComponent<Card>();
                    if (card.IsFaceUp())
                    {
                        sourceCardHolder = rays[^1].collider.gameObject.GetComponent<CardHolder>();
                        pickedUpCards = sourceCardHolder.GetCardListAfter(card);
                        SetCardOffSet(mousePos, pickedUpCards[0].gameObject.transform.position);
                        AudioManager.instance.PlayCardPickedClip();
                    }
                }
            }  
        }
    }

    private void HandleMouseReleaseBoxCast(Vector3 _mousePos)
    {
        GameObject firstCard;
        if (pickedUpCards.Count == 0) return;

        List<Card> cardList = new();
        foreach(Card item in pickedUpCards)
        {
            cardList.Add(item);
        }
        firstCard = cardList[0].gameObject;

        RaycastHit2D[] hits = Physics2D.BoxCastAll(_mousePos, firstCard.gameObject.GetComponent<BoxCollider2D>().size, 0, Vector2.zero,10,holderMask);
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

            if(hits[i].collider.gameObject.GetComponent<CardHolder>().IsCardTransferable(cardList[0]))
            {
                bool isFaceUp = sourceCardHolder.IsCardAboveFaceUp(cardList[0]);
                sourceCardHolder.RemoveCardsFromList(cardList);
                transferedHolder.AddCardsFromList(cardList);
                UndoManager.instance.RegisterMove(sourceCardHolder, transferedHolder, cardList, isFaceUp);
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
                if(cardHoldersTop[i].IsCardTransferable(card) && !cardHolder.CompareTag("CardHolderTop"))
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

    private void SetCardOffSet(Vector3 firstPoint, Vector3 secondPoint)
    {
        offSet.x = firstPoint.x - secondPoint.x;
        offSet.y = firstPoint.y - secondPoint.y;
        offSet.z = 0;
    }

    // Start And Restart Game-------------------------------------------------/

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
            winScreen.gameObject.SetActive(true);
        }
    }

    private int holderIndex = 0;
    private float timer = 3f;
    private float duration;
    private List<Card> jumpedCards = new();

    private void WinScreen()
    {
        timer += Time.deltaTime;
        duration += Time.deltaTime;
        if(timer >= 3f)
        {
            timer = 0;
            Card card = cardHoldersTop[holderIndex].cards[^1];
            card.GetComponent<CardJump>().ConfigureForJump();
            cardHoldersTop[holderIndex].RemoveCard(card);
            jumpedCards.Add(card);
            if(holderIndex < 3)
            {
                holderIndex++;
            }
            else
            {
                holderIndex = 0;
            }
        }
        if(duration >= 60f || Input.GetMouseButtonDown(0))
        {
            DeactivateWinScreen();
            winScreen.enabled = false;
            hasWon = false;
            UIManager.instance.ActivateWinPanel();
        }
    }

    private void DeactivateWinScreen()
    {
        foreach(Card card in jumpedCards)
        {
            card.gameObject.GetComponent<CardJump>().SetJump(false);
            card.gameObject.SetActive(true);
        }
    }

    public void RestartGame()
    {
        for (int i = 0; i < cardHoldersTop.Count; i++)
        {
            List<Card> cardList = cardHoldersTop[i].GetAllCardsList();
            DeckOfCards.instance.AddCardsToDeckFromList(cardList);
        }
        foreach(Card card in jumpedCards)
        {
            DeckOfCards.instance.AddCard(card);
        }
        Initialize();
    }

    private void InitializeTest()
    {
        Card card;
        for (int i = 0; i < cardHoldersTop.Count; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                card = DeckOfCards.instance.GetRandomCardFromDeck();
                cardHoldersTop[i].AddCardWithoutCondition(card);
            }
            cardHoldersTop[i].SnapCardsToPosition();
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
