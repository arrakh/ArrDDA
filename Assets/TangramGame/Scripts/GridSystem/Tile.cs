using System;
using UnityEngine;

namespace TangramGame.Scripts.GridSystem
{
    public class Tile
    {
        protected bool Equals(Tile other)
        {
            return position.Equals(other.position);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Tile) obj);
        }

        public override int GetHashCode()
        {
            return position.GetHashCode();
        }

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
