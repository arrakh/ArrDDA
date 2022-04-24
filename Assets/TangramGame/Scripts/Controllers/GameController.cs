using System;
using System.Collections;
using System.Collections.Generic;
using TangramGame.Scripts.Controllers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TangramGame.Scripts
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GridController grid;
        [SerializeField] private TileContentObject contentPrefab;
        [SerializeField] private CameraController camera;

        private List<TileContentObject> placedContents = new List<TileContentObject>();
        private List<TileContentObject> spawnedContents = new List<TileContentObject>();

        private TileContent currentContent;
        private Vector2 lastPos;

        private GameTimer currentTimer;
        private bool shouldUpdateTimer = false;

        public void Start()
        {
            GenerateGame(new GameDifficulty(Random.Range(3, 6), Random.Range(3, 6), 10f));
            
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                GenerateGame(new GameDifficulty(Random.Range(3, 6), Random.Range(3, 6), 10f));
            
            if (shouldUpdateTimer) currentTimer.Update(Time.deltaTime);
        }

        public void GenerateGame(GameDifficulty gameDifficulty)
        {
            ClearGame();
            
            var w = gameDifficulty.width;
            var h = gameDifficulty.height;
            grid.CreateGrid(w, h);
            
            var patterns = GridUtility.GenerateRandomContent(w, h, gameDifficulty.minPieceSize, gameDifficulty.maxPieceSize);
            var placementOffset = Mathf.Max(w, h);

            float delay = 0f;
        
            foreach (var pattern in patterns)
            {
                var circle = Random.insideUnitCircle.normalized;
                var oval = new Vector2(circle.x / 1.8f, circle.y);
                var pos = oval * placementOffset /2f;
                var controller = Instantiate(contentPrefab.gameObject, pos, Quaternion.identity)
                    .GetComponent<TileContentObject>();
                
                controller.Setup(pattern.Value, OnContentPicked, OnContentDropped, OnContentDragged);
                controller.transform.localScale = Vector3.zero;
                LeanTween.delayedCall(delay, () => { controller.AnimateScale(0f, 1f, 0.3f); });
                spawnedContents.Add(controller);
                
                delay += 0.1f;
            }
            
            camera.MoveToGrid(grid);

            currentTimer = new GameTimer(gameDifficulty.roundTime);
            currentTimer.OnTimerEnd.AddListener(OnTimerEnded);
            shouldUpdateTimer = true;
        }

        public void OnTimerEnded()
        {
            shouldUpdateTimer = false;
        }

        public void ClearGame()
        {
            grid.ClearGrid();
            foreach (var content in spawnedContents)
                Destroy(content.gameObject);
            
            spawnedContents.Clear();
            placedContents.Clear();
        }

        private void CheckForWin()
        {
            if (grid.IsAllFilled())
            {
                GenerateGame(new GameDifficulty(Random.Range(3, 6), Random.Range(3, 6), 10f));
            }
        }

        private void OnContentPicked(TileContentObject obj)
        {
            currentContent = obj.Content;
            if (placedContents.Contains(obj))
            {
                placedContents.Remove(obj);
                grid.RemovePiece(obj.transform.position);
            }
        }

        private void OnContentDropped(TileContentObject obj)
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

            CheckForWin();
        }

        private void OnContentDragged(TileContentObject obj, Vector2 pos)
        {
            lastPos = pos;
            
            grid.PreShowTile(obj.Content, pos);
        }
    }
}