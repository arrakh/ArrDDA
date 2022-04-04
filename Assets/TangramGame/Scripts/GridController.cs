using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TangramGame.Scripts
{
    public class GridController : MonoBehaviour
    {
        public Vector3 GridWorldPos => tileParent.position;
        public Grid Grid => grid;
        
        [SerializeField] private TileController tilePrefab;
        [SerializeField] private Transform tileParent;
        
        private Grid grid;
        private int width, height;
        private Dictionary<Vector2Int, TileController> tileControllers = new Dictionary<Vector2Int, TileController>();
        private List<Vector2Int> lastPreShowPositions = new List<Vector2Int>();
        private Vector2Int lastPreShowGridPos = Vector2Int.one * 99999;

        public void ClearGrid()
        {
            foreach (var tileController in tileControllers.Values)
                Destroy(tileController.gameObject);
            
            tileControllers.Clear();
        }
        
        public void CreateGrid(int w, int h)
        {
            tileControllers = new Dictionary<Vector2Int, TileController>(w * h);
            
            width = w;
            height = h;
            grid = new Grid(width, height, OnTileCreated);
        }

        private void OnTileCreated(Tile t)
        {
            var controller = Instantiate(tilePrefab.gameObject, GridToWorldPos(t.Position), Quaternion.identity, tileParent)
                .GetComponent<TileController>();
            
            controller.Setup(t);
            
            tileControllers[t.Position] = controller;
        }

        public void RemovePiece(Vector2 worldPos)
        {
            var gridPos = WorldToGridPos(worldPos);
            if (!grid.IsInBounds(gridPos)) return;

            var contentToRemove = tileControllers[gridPos].Tile.CurrentContent;

            foreach (var tileController in tileControllers.Values)
            {
                var toCheck = tileController.Tile.CurrentContent;
                if (toCheck != null && toCheck.Equals(contentToRemove))
                    tileController.Tile.CurrentContent = null;
            }
        }

        public void PlacePiece(TileContent content, Vector2 worldPos)
            => grid.SetPiece(content, WorldToGridPos(worldPos));

        public bool IsValid(TileContent content, Vector2 worldPos)
            => grid.IsValidPlacement(content, WorldToGridPos(worldPos));

        public void PreShowTile(TileContent content, Vector2 worldPos)
        {
            var gridPos = WorldToGridPos(worldPos);

            if (gridPos.Equals(lastPreShowGridPos)) return;
            lastPreShowGridPos = gridPos;
            ClearLastPreShows();
            
            var isValid = grid.IsValidPlacement(content, gridPos);

            if(grid.IsInBounds(gridPos))
            {
                tileControllers[gridPos].SetPreShow(true, isValid);
                if (!lastPreShowPositions.Contains(gridPos)) lastPreShowPositions.Add(gridPos);
            }

            foreach (var offset in content.OffsetPieces)
            {
                var pos = gridPos + offset;
                if (!grid.IsInBounds(pos)) continue;
                tileControllers[pos].SetPreShow(true, isValid);
                if (!lastPreShowPositions.Contains(pos)) lastPreShowPositions.Add(pos);
            }
        }

        public void ClearLastPreShows()
        {
            foreach (var pos in lastPreShowPositions)
                tileControllers[pos].SetPreShow(false, false);
            lastPreShowPositions.Clear();
        }
        
        public Vector2Int WorldToGridPos(Vector2 worldPos)
        {
            var x = worldPos.x + width / 2f - GridWorldPos.x;
            var y = worldPos.y + height / 2f - GridWorldPos.y;
            return Vector2Int.RoundToInt(new Vector2(x, y));
        }
        
        public Vector2 GridToWorldPos(Vector2Int gridPos)
        {
            var x = GridWorldPos.x + gridPos.x - width / 2f;
            var y = GridWorldPos.y + gridPos.y - height / 2f;
            return new Vector2(x, y);
        }
    }
}