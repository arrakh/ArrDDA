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
        [SerializeField] private CameraController camera;

        private List<TileContentController> placedContents = new List<TileContentController>();
        private List<TileContentController> spawnedContents = new List<TileContentController>();

        private TileContent currentContent;
        private Vector2 lastPos;

        public void Start()
        {
            GenerateGame(new GameSettings(Random.Range(3, 6), Random.Range(3, 6), 10f));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                GenerateGame(new GameSettings(Random.Range(3, 6), Random.Range(3, 6), 10f));
        }

        public void GenerateGame(GameSettings gameSettings)
        {
            ClearGame();
            
            var w = gameSettings.width;
            var h = gameSettings.height;
            grid.CreateGrid(w, h);
            
            var patterns = GridUtility.GenerateRandomContent(w, h, gameSettings.minPieceSieze, gameSettings.maxPieceSize);
            var placementOffset = Mathf.Max(w, h);

            float delay = 0f;
        
            foreach (var pattern in patterns)
            {
                var circle = Random.insideUnitCircle.normalized;
                var oval = new Vector2(circle.x / 1.8f, circle.y);
                var pos = oval * placementOffset /2f;
                var controller = Instantiate(contentPrefab.gameObject, pos, Quaternion.identity)
                    .GetComponent<TileContentController>();
                
                controller.Setup(pattern.Value, OnContentPicked, OnContentDropped, OnContentDragged);
                controller.transform.localScale = Vector3.zero;
                LeanTween.delayedCall(delay, () => { controller.AnimateScale(0f, 1f, 0.3f); });
                spawnedContents.Add(controller);
                
                delay += 0.1f;
            }
            
            camera.MoveToGrid(grid);
        }

        public void ClearGame()
        {
            grid.ClearGrid();
            foreach (var content in spawnedContents)
                Destroy(content.gameObject);
            
            spawnedContents.Clear();
            placedContents.Clear();
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
                //obj.ResetPos();
                return;
            }
            grid.PlacePiece(obj.Content, lastPos);
            var actualPos = grid.GridToWorldPos(grid.WorldToGridPos(lastPos));
            obj.transform.position = actualPos;
            obj.initPos = actualPos;
            obj.SetOrder(0);
            placedContents.Add(obj);
        }

        private void OnContentDragged(TileContentController obj, Vector2 pos)
        {
            lastPos = pos;
            
            grid.PreShowTile(obj.Content, pos);
        }
    }
}