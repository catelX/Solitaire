using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class Card : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private Sprite faceUpImage;
        public Sprite faceDownImage;
        private bool isFaceUp;
        private int value;
        public enum Symbols
        {
            Spades,
            Clubs,
            Hearts,
            Diamonds
        }
        public string symbol;
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            isFaceUp = false;
        }

        public void ConfigureCard(Sprite _faceUpImage, int _value, string _symbol)
        {
            faceUpImage = _faceUpImage;
            value = _value;
            symbol = _symbol;
        }

        private void Update()
        {
            if (isFaceUp)
            {
                spriteRenderer.sprite = faceUpImage;
            }
            else
            {
                spriteRenderer.sprite = faceDownImage;
            }

        }

        public void SetFaceUp(bool _isFaceUp)
        {
            isFaceUp = _isFaceUp;
        }

        public bool IsFaceUp()
        {
            return isFaceUp;
        }

        public int GetValue()
        {
            return value;
        }

        public bool IsRed()
        {
            switch(symbol)
            {
                case "Spades": return false;
                case "Clubs": return false;
                case "Hearts": return true;
                case "Diamonds": return true;
            }
            return false;
        }

        public string GetSymbol()
        {
            return symbol;
        }
    }
}
