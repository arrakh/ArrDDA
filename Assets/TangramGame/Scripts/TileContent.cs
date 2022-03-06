using System.Collections.Generic;
using UnityEngine;

namespace TangramGame.Scripts
{
    public class TileContent
    {
        public Color color;
        
        private Vector2Int origin;
        private HashSet<Vector2Int> offsetPieces;

        public TileContent(Vector2Int origin, HashSet<Vector2Int> offsetPieces, Color color)
        {
            this.origin = origin;
            this.color = color;

            this.offsetPieces = offsetPieces;
        }

        public Vector2Int Origin
        {
            get => origin;
            set => origin = value;
        }

        public HashSet<Vector2Int> OffsetPieces
        {
            get => offsetPieces;
            set => offsetPieces = value;
        }
    }
}