using System.Collections.Generic;
using UnityEngine;

namespace TangramGame.Scripts
{
    public class TileContent
    {
        public Color color;
        
        private HashSet<Vector2Int> offsetPieces;

        public TileContent(HashSet<Vector2Int> offsetPieces, Color color)
        {
            this.color = color;

            this.offsetPieces = offsetPieces;
        }


        public HashSet<Vector2Int> OffsetPieces
        {
            get => offsetPieces;
            set => offsetPieces = value;
        }
    }
}