using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TangramGame.Scripts
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GridController grid;
        [SerializeField] private TileContentController contentPrefab;

        private List<TileContentController> placedContents = new List<TileContentController>();

        private TileContent currentContent;
        private Vector2 lastPos;
        
        static HashSet<Vector2Int> pShape = new HashSet<Vector2Int>()
        {
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(1, 1),
            new Vector2Int(2, 1),
        };
            
        static HashSet<Vector2Int> plus = new HashSet<Vector2Int>()
        {
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
        };

        public void Start()
        {
            grid.CreateGrid(6, 4);

            var patterns = GridUtility.GenerateRandomContent(6, 4);
        
            foreach (var pattern in patterns)
            {
                var pos = grid.GridToWorldPos(pattern.Key);
                var controller = Instantiate(contentPrefab.gameObject, pos, Quaternion.identity)
                    .GetComponent<TileContentController>();
                
                controller.Setup(pattern.Value, OnContentPicked, OnContentDropped, OnContentDragged);
            }
        }

        // IEnumerator Start()
        // {
        //     var w = 6;
        //     var h = 4;
        //     
        //     grid.CreateGrid(w, h);
        //     
        //     var contents = new Dictionary<Vector2Int, TileContent>();
        //     var tiles = new List<Vector2Int>(w * h);
        //     var generatedTiles = new HashSet<Vector2Int>();
        //     
        //     //Generate Tiles and Hashset
        //     for (int x = 0; x < w; x++)
        //     for (int y = 0; y < h; y++)
        //     {
        //         var vector = new Vector2Int(x, y);
        //         tiles.Add(vector);
        //     }
        //
        //     while (tiles.Count > 0)
        //     {
        //         yield return new WaitForSeconds(0.05f);
        //         
        //         var generateCount = Random.Range(2, 5);           
        //         var randomIndex = Random.Range(0, tiles.Count);
        //         var randomOriginTile = tiles[randomIndex];
        //         tiles.RemoveAt(randomIndex);
        //         generatedTiles.Add(randomOriginTile);
        //         
        //         Debug.Log($"<color=red>Generating content at pos {randomOriginTile} with {generateCount + 1} pieces...</color>");
        //
        //         var newContent = new TileContent(new HashSet<Vector2Int>(), Random.ColorHSV(0, 1, 1f, 1f, 0.5f, 1f, 1f, 1f));
        //         var pivots = new List<Vector2Int>();
        //         pivots.Add(randomOriginTile);
        //         
        //         var isSingle = GridUtility.IsSurrounded(randomOriginTile, generatedTiles, w, h);
        //
        //         if (isSingle)
        //         {
        //             Debug.Log($"{randomOriginTile} is SINGLE!, will ignore");
        //         }
        //         else while (generateCount > 0 && tiles.Count > 0)
        //         {
        //             yield return new WaitForSeconds(0.05f);
        //
        //             bool canPutMore = false;
        //             foreach (var pivot in pivots)
        //             {
        //                 canPutMore = !GridUtility.IsSurrounded(pivot, generatedTiles, w, h);
        //                 if (canPutMore) break;
        //             }
        //
        //             if (!canPutMore) break;
        //             
        //             foreach (var pivot in pivots)
        //             {
        //                 var randomOffset = GridUtility.RandomCardinalDirection();
        //                 var pos = pivot + randomOffset;
        //
        //                 Debug.Log($"    <color=green>...Checking out position {pos} from pivot {pivot}</color>");
        //                 yield return new WaitForSeconds(0.05f);
        //                 
        //                 if (generatedTiles.Contains(pos)) continue;
        //
        //                 //Ignore if out of bounds
        //                 if (GridUtility.IsOutOfBounds(pos, w, h)) continue;
        //
        //                 //Ignore if no cardinal neighbour
        //                 if (!GridUtility.HasCardinalNeighbour(pos, generatedTiles)) continue;
        //
        //                 newContent.OffsetPieces.Add(pivot - randomOriginTile + randomOffset);
        //                 tiles.Remove(pos);
        //                 generatedTiles.Add(pos);
        //                 pivots.Add(pos);
        //                 Debug.Log($"    <color=lime>Generated content offset at {pos}</color>");
        //                 generateCount--;
        //                 break;
        //             }
        //         }
        //
        //         contents[randomOriginTile] = newContent;
        //         Debug.Log($"<color=cyan>Generated content at pos {randomOriginTile} with {newContent.OffsetPieces.Count + 1} pieces</color>");
        //
        //         var print = $"{tiles.Count} Pieces Left: ";
        //         foreach (var tile in tiles)
        //             print += $"\n    - {tile}";
        //         
        //         Debug.Log(print);
        //     }
        //     
        //     foreach (var pattern in contents)
        //     {
        //         var pos = grid.GridToWorldPos(pattern.Key);
        //         var controller = Instantiate(contentPrefab.gameObject, pos, Quaternion.identity)
        //             .GetComponent<TileContentController>();
        //         
        //         controller.Setup(pattern.Value, OnContentPicked, OnContentDropped, OnContentDragged);
        //     }
        //
        //     yield return null;
        // }

        private void OnContentPicked(TileContentController obj)
        {
            currentContent = obj.Content;
            if (placedContents.Contains(obj))
            {
                placedContents.Remove(obj);
                grid.RemovePiece(obj.transform.position);
            }
        }

        private void OnContentDropped(TileContentController obj)
        {
            currentContent = null;
            
            grid.ClearLastPreShows();
            if (!grid.IsValid(obj.Content, lastPos))
            {
                //obj.ResetPos();
                return;
            }
            grid.PlacePiece(obj.Content, lastPos);
            var actualPos = grid.GridToWorldPos(grid.WorldToGridPos(lastPos));
            obj.transform.position = actualPos;
            obj.initPos = actualPos;
            placedContents.Add(obj);
        }

        private void OnContentDragged(TileContentController obj, Vector2 pos)
        {
            lastPos = pos;
            
            grid.PreShowTile(obj.Content, pos);
        }
    }
}