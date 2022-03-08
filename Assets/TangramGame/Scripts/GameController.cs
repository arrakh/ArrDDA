using System;
using System.Collections.Generic;
using UnityEngine;

namespace TangramGame.Scripts
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GridController grid;
        [SerializeField] private Transform spawnPos;
        [SerializeField] private TileContentController contentPrefab;

        private TileContent currentContent;
        private Vector2 lastPos;
        
        static HashSet<Vector2Int> square = new HashSet<Vector2Int>()
        {
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, 1),
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

            var content = new TileContent(plus, Color.blue);
            
            var tcc = Instantiate(contentPrefab.gameObject, spawnPos.position, Quaternion.identity)
                .GetComponent<TileContentController>();

            tcc.Setup(content, OnContentPicked, OnContentDropped, OnContentDragged);
        }

        private void OnContentPicked(TileContentController obj) => currentContent = obj.Content;

        private void OnContentDropped(TileContentController obj)
        {
            currentContent = null;
            obj.ResetPos();
            if (!grid.IsValid(obj.Content, lastPos))
            {
                return;
            }
            grid.PlacePiece(obj.Content, lastPos);
            //obj.transform.position = (Vector2) Vector2Int.RoundToInt(lastPos);
        }

        private void OnContentDragged(TileContentController obj, Vector2 pos)
        {
            lastPos = pos;
            
            if(grid.IsValid(obj.Content, pos)) Debug.Log("<color=lime>Valid Placement!</color>");
            else Debug.Log("<color=red>NOT VALID</color>");
        }
    }
}