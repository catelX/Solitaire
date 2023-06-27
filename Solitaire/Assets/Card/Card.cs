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
        private bool isRed;
        private string symbol;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            isFaceUp = false;
        }

        public void ConfigureCard(Sprite _faceUpImage, int _value, bool _isRed, string _symbol)
        {
            faceUpImage = _faceUpImage;
            value = _value;
            isRed = _isRed;
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

        public bool GetIsFaceUp()
        {
            return isFaceUp;
        }

        public int GetValue()
        {
            return value;
        }

        public bool IsRed()
        {
            return isRed;
        }

        public string GetSymbol()
        {
            return symbol;
        }
    }
}
