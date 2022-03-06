using System;
using UnityEngine;

namespace TangramGame.Scripts
{
    public class Tile
    {
        public Vector2Int Position
        {
            get => position;
            set => position = value;
        }

        public TileContent CurrentContent
        {
            get => currentContent;
            set
            {
                currentContent = value;
                OnContentChanged?.Invoke(value);
            }
        }

        public Action<TileContent> OnContentChanged;

        private Vector2Int position;

        private TileContent currentContent = null;

        public Tile(int x, int y)
        {
            position = new Vector2Int(x, y);
        }

        public Tile() { }
    }
}
