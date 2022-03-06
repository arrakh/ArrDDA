using System;
using UnityEngine;
using UnityEngine.UI;

namespace TangramGame.Scripts
{
    public class TileController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sprite;
        
        private Tile tile;

        public void Setup(Tile t)
        {
            this.tile = t;
            tile.OnContentChanged += OnContentChanged;
        }

        private void OnDestroy()
        {
            if (tile != null) tile.OnContentChanged -= OnContentChanged;
        }

        private void OnContentChanged(TileContent newContent)
        {
            sprite.color = newContent.color;
        }
    }
}