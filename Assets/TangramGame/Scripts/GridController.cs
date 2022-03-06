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

        private void Start()
        {
            CreateGrid(6, 6);

            var square = new HashSet<Vector2Int>()
            {
                new Vector2Int(0, 1),
                new Vector2Int(1, 0),
                new Vector2Int(1, 1),
            };
            
            var plus = new HashSet<Vector2Int>()
            {
                new Vector2Int(0, 1),
                new Vector2Int(1, 0),
                new Vector2Int(0, -1),
                new Vector2Int(-1, 0),
            };

            var piece1 = new TileContent(Vector2Int.zero, square, Color.red);
            var piece2 = new TileContent(Vector2Int.one * 3, square, Color.blue);
            var piece3 = new TileContent(Vector2Int.one * 1, square, Color.blue);
            var piece4 = new TileContent(Vector2Int.one * 2, plus, Color.green);
            
            PlacePiece(piece1);
            PlacePiece(piece2);
            PlacePiece(piece3);
            PlacePiece(piece4);
        }

        public void CreateGrid(int w, int h)
        {
            width = w;
            height = h;
            grid = new Grid(width, height, OnTileCreated);
        }

        private void OnTileCreated(Tile t)
        {
            var spawnPos = new Vector3(t.Position.x - width / 2f, t.Position.y - height / 2f);
            var controller = Instantiate(tilePrefab.gameObject, spawnPos, Quaternion.identity, tileParent)
                .GetComponent<TileController>();
            
            controller.Setup(t);
        }

        public void PlacePiece(TileContent content)
        {
            if (grid.TrySetPiece(content)) Debug.Log("Piece Set!");
            else Debug.Log("Setting piece failed!");
        }
    }
}