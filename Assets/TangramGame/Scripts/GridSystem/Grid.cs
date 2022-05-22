using System;
using UnityEngine;

namespace TangramGame.Scripts.GridSystem
{
    public class Grid
    {
        private Tile[,] tiles;
        public int width { get; private set; }
        public int height { get; private set; }

        public Grid(int w, int h, Action<Tile> onCreated = null)
        {
            width = w;
            height = h;
            tiles = new Tile[width,height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var newTile = new Tile(x, y);
                    tiles[x, y] = newTile;
                    onCreated?.Invoke(newTile);
                }
            }
        }

        public bool TryGetTile(Vector2 position, out Tile tile)
        {
            var pos = Vector2Int.RoundToInt(position);
            return TryGetTile(pos, out tile);
        }

        public bool TryGetTile(Vector2Int position, out Tile tile)
        {
            tile = null;
            
            if (!IsInBounds(position)) return false;

            tile = tiles[position.x, position.y];
            return true;
        }

        public void SetPiece(TileContent content, Vector2Int gridPosition)
        {
            tiles[gridPosition.x, gridPosition.y].CurrentContent = content;

            foreach (var offset in content.OffsetPieces)
            {
                var pos = offset + gridPosition;
                tiles[pos.x, pos.y].CurrentContent = content;
            }
        }

        public bool IsValidPlacement(TileContent content, Vector2Int position)
        {
            if (!IsInBounds(position)) return false;
            if (HasContent(position)) return false;

            foreach (var offset in content.OffsetPieces)
            {
                var pos = position + offset;
                if (!IsInBounds(pos)) return false;
                if (HasContent(pos)) return false;
            }
            
            return true;
        }

        public bool IsInBounds(Vector2Int position)
        {
            if (position.x < 0 || position.x >= width) return false;
            if (position.y < 0 || position.y >= height) return false;
            return true;
        }

        public bool HasContent(Vector2Int position)
            => tiles[position.x, position.y].CurrentContent != null;

    }
}