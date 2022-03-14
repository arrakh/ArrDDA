using System;
using System.Collections.Generic;
using UnityEngine;

namespace TangramGame.Scripts
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GridController grid;
        [SerializeField] private Transform plusPos, pShapePos;
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
            grid.CreateGrid(6, 5);

            var plusContent = new TileContent(plus, Color.blue);
            var plusController = Instantiate(contentPrefab.gameObject, plusPos.position, Quaternion.identity)
                .GetComponent<TileContentController>();

            var pShapeContent = new TileContent(pShape, Color.magenta);
            var pShapeController = Instantiate(contentPrefab.gameObject, pShapePos.position, Quaternion.identity)
                .GetComponent<TileContentController>();

            plusController.Setup(plusContent, OnContentPicked, OnContentDropped, OnContentDragged);
            pShapeController.Setup(pShapeContent, OnContentPicked, OnContentDropped, OnContentDragged);
        }

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
                obj.ResetPos();
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