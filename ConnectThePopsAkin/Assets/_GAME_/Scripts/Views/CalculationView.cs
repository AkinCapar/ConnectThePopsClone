using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectThePops.Views
{
    public class CalculationView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public void SetSprite(Sprite sprite)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
            _spriteRenderer.sprite = sprite;
        }
    }
}
