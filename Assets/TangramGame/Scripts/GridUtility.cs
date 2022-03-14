using System.Collections.Generic;
using UnityEngine;

namespace TangramGame.Scripts
{
    public static class GridUtility
    {
        public static List<TileContent> GenerateRandomContent(int w, int h)
        {
            var contents = new List<TileContent>();
            var tiles = new List<Vector2Int>(w * h);

            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    tiles.Add(new Vector2Int(x, y));

            while (tiles.Count > 0)
            {
                
            }
            
            
            return contents;
        }
    }
}