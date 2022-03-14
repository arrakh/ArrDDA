using System;
using UnityEngine;
using UnityEngine.UI;

namespace TangramGame.Scripts
{
    public class TileController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Color normal, valid, invalid;

        public Tile Tile { get; private set; }

        public void Setup(Tile t)
        {
            this.Tile = t;
            Tile.OnContentChanged += OnContentChanged;
        }

        private void OnDestroy()
        {
            if (Tile != null) Tile.OnContentChanged -= OnContentChanged;
        }

        private void OnContentChanged(TileContent newContent)
        {
            
        }

        public void SetPreShow(bool isOn, bool isValid)
        {
            if (Tile.CurrentContent != null) return;
            sprite.color = !isOn ? normal : isValid ? valid : invalid;
        }
    }
}