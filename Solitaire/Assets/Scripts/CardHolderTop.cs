using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public class CardHolderTop : MonoBehaviour, IConditionCheck
{
    private List<GameObject> cards = new List<GameObject>();
    private Vector3 myPos;

    private void Awake()
    {
        myPos = transform.position;
    }

    public void SnapCardsToPosition()
    {
        for(int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.position = new Vector3(myPos.x, myPos.y, myPos.z - (0.1f * (i + 1)));
        }
    }

    public void CheckAndAddCard(GameObject card)
    {
        if(cards.Count == 0)
        {
            if(card.GetComponent<Card>().GetValue() == 1)
            {
                cards.Add(card);
            }
            else
            {
                return;
            }
        }
        int topCardValue = cards[cards.Count - 1].GetComponent<Card>().GetValue();

        if (card.GetComponent<Card>().GetValue() == topCardValue + 1 && card.GetComponent<Card>().GetSymbol() == cards[cards.Count - 1].GetComponent<Card>().GetSymbol())
        { 
            cards.Add(card);
        }
    }
}
