using System.Collections.Generic;
using UnityEngine;

namespace TangramGame.Scripts
{
    public static class GridUtility
    {
        public static Dictionary<Vector2Int, TileContent> GenerateRandomContent(int w, int h)
        {
            var contents = new Dictionary<Vector2Int, TileContent>();
            var tiles = new List<Vector2Int>(w * h);
            var generatedTiles = new HashSet<Vector2Int>();
            
            //Generate Tiles and Hashset
            for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
            {
                var vector = new Vector2Int(x, y);
                tiles.Add(vector);
            }

            while (tiles.Count > 0)
            {
                var generateCount = Random.Range(2, 5);           
                var randomIndex = Random.Range(0, tiles.Count);
                var randomOriginTile = tiles[randomIndex];
                tiles.RemoveAt(randomIndex);
                generatedTiles.Add(randomOriginTile);

                var newContent = new TileContent(new HashSet<Vector2Int>(), Random.ColorHSV(0, 1, 1f, 1f, 0.5f, 1f, 1f, 1f));
                var pivots = new List<Vector2Int>();
                pivots.Add(randomOriginTile);
                
                var isSingle = IsSurrounded(randomOriginTile, generatedTiles, w, h);

                if (!isSingle) while (generateCount > 0 && tiles.Count > 0)
                {
                    bool canPutMore = false;
                    foreach (var pivot in pivots)
                    {
                        canPutMore = !IsSurrounded(pivot, generatedTiles, w, h);
                        if (canPutMore) break;
                    }

                    if (!canPutMore) break;
                    
                    foreach (var pivot in pivots)
                    {
                        var randomOffset = RandomCardinalDirection();
                        var pos = pivot + randomOffset;
                        if (generatedTiles.Contains(pos)) continue;

                        //Ignore if out of bounds
                        if (IsOutOfBounds(pos, w, h)) continue;

                        //Ignore if no cardinal neighbour
                        if (!HasCardinalNeighbour(pos, generatedTiles)) continue;

                        newContent.OffsetPieces.Add(pivot - randomOriginTile + randomOffset);
                        tiles.Remove(pos);
                        generatedTiles.Add(pos);
                        pivots.Add(pos);
                        generateCount--;
                        break;
                    }
                }

                contents[randomOriginTile] = newContent;
            }

            return contents;
        }

        public static Vector2Int RandomCardinalDirection()
        {
            var rand = Random.Range(0, 4);
            return rand switch
            {
                1 => Vector2Int.right,
                2 => Vector2Int.down,
                3 => Vector2Int.left,
                _ => Vector2Int.up
            };
        }

        public static bool HasCardinalNeighbour(Vector2Int toCheck, HashSet<Vector2Int> tiles)
        {
            return tiles.Contains(toCheck + Vector2Int.up) ||      
                   tiles.Contains(toCheck + Vector2Int.down) ||    
                   tiles.Contains(toCheck + Vector2Int.left) ||    
                   tiles.Contains(toCheck + Vector2Int.right);     
        }
        
        public static bool IsSurrounded(Vector2Int toCheck, HashSet<Vector2Int> tiles, int width, int height)
        {
            return (tiles.Contains(toCheck + Vector2Int.up) || IsOutOfBounds(toCheck + Vector2Int.up, width, height)) &&
                   (tiles.Contains(toCheck + Vector2Int.down) || IsOutOfBounds(toCheck + Vector2Int.down, width, height)) &&
                   (tiles.Contains(toCheck + Vector2Int.left) || IsOutOfBounds(toCheck + Vector2Int.left, width, height)) &&
                   (tiles.Contains(toCheck + Vector2Int.right) || IsOutOfBounds(toCheck + Vector2Int.right, width, height));
        }

        public static bool IsOutOfBounds(Vector2Int pos, int width, int height)
        {
            if (pos.x >= width || pos.x < 0) return true;
            if (pos.y >= height || pos.y < 0) return true;
            return false;
        }
    }
}