using System;
using System.Collections.Generic;
using UnityEngine;

namespace TangramGame.Scripts
{
    public class TileContent : IEquatable<TileContent>
    {
        public Guid id;
        
        public Color color;
        
        private HashSet<Vector2Int> offsetPieces;

        public TileContent(HashSet<Vector2Int> offsetPieces, Color color)
        {
            id = Guid.NewGuid();
            this.color = color;
            this.offsetPieces = offsetPieces;
        }


        public HashSet<Vector2Int> OffsetPieces
        {
            get => offsetPieces;
            set => offsetPieces = value;
        }

        public bool Equals(TileContent other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return id.Equals(other.id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TileContent) obj);
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}