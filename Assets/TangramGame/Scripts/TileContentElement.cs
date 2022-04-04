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
            Debug.Log($"Setup Content Element at {transform.position} with color {color}");
        }

        public void SetOrder(int order) => sprite.sortingOrder = order;
    }
}