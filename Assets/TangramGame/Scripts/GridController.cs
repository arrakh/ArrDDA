using System;
using System.Collections.Generic;
using UnityEngine;

namespace TangramGame.Scripts
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private TileController tilePrefab;
        [SerializeField] private Transform tileParent;
        
        private Grid grid;
        private int width, height;

        public void CreateGrid(int w, int h)
        {
            width = w;
            height = h;
            grid = new Grid(width, height, OnTileCreated);
        }

        private void OnTileCreated(Tile t)
        {
            var controller = Instantiate(tilePrefab.gameObject, GridToWorldPos(t.Position), Quaternion.identity, tileParent)
                .GetComponent<TileController>();
            
            controller.Setup(t);
        }

        public void PlacePiece(TileContent content, Vector2 worldPos)
            => grid.SetPiece(content, WorldToGridPos(worldPos));

        public bool IsValid(TileContent content, Vector2 worldPos)
            => grid.IsValidPlacement(content, WorldToGridPos(worldPos));
        
        public Vector2Int WorldToGridPos(Vector2 worldPos)
            => Vector2Int.RoundToInt(new Vector2(/*tileParent.position.x - */worldPos.x + width / 2f, /*tileParent.position.y - */worldPos.y + height / 2f));
        
        public Vector2 GridToWorldPos(Vector2Int gridPos)
            => new Vector2(/*tileParent.position.x + */gridPos.x - width / 2f, /*tileParent.position.y + */gridPos.y - height / 2f);
    }
}