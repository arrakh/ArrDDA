using System;
using System.Collections.Generic;
using UnityEngine;

namespace TangramGame.Scripts
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
            
            if (position.x < 0 || position.x > width) return false;
            if (position.y < 0 || position.y > height) return false;

            tile = tiles[position.x, position.y];
            return true;
        }

        public bool TrySetPiece(TileContent content)
        {
            var allTiles = new HashSet<Tile>();
            
            //Try Get Origin
            if (!TryGetTile(content.Origin, out var originTile)) return false;
            if (originTile.CurrentContent != null) return false;
            allTiles.Add(originTile);

            foreach (var offset in content.OffsetPieces)
            {
                if (!TryGetTile(content.Origin + offset, out var tile)) return false;
                if (tile.CurrentContent != null) return false;
                allTiles.Add(tile);
            }

            foreach (var tile in allTiles)
                tile.CurrentContent = content;

            return true;
        }

    }
}