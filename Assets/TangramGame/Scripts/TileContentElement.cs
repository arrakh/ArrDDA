using System;
using UnityEngine;

namespace TangramGame.Scripts
{
    public class TileContentElement : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sprite;

        private int originalOrder;

        private void Awake()
        {
            originalOrder = sprite.sortingOrder;
        }

        public void Setup(Color color)
        {
            sprite.color = color;
        }

        public void BringToFront() => sprite.sortingOrder = 999;
        public void ResetOrder() => sprite.sortingOrder = originalOrder;
    }
}