using System;
using System.Collections.Generic;
using Arr.DDA;
using Arr.DDA.Script;
using Arr.DDA.Script.Evaluators;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MockupGame
{
    public class GameManager : MonoBehaviour
    {
        public MetricObject Score;
        public MetricObject TargetTile;
        public MetricObject TileSize;
        public ChannelObject TargetChannel;
        public ChannelObject SizeChannel;

        public AdaptParameter Parameter = new AdaptParameter();
        
        public float timeLeft;

        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private GridLayout gridLayout;
        [SerializeField] private Image timerFill;

        private List<Tile> tiles = new List<Tile>();
        private bool isPlaying = false;
        private float maxTime = 5f;
        private int currentTile = 0;

        private void Start()
        {
            TargetChannel.Initialize();
            SizeChannel.Initialize();

            GenerateGrid((int)TileSize.Value);
            timeLeft = maxTime;
            isPlaying = true;
        }

        private void Update()
        {
            TimerTick();
        }

        private void TimerTick()
        {
            if (!isPlaying) return;

            timeLeft -= Time.deltaTime;
            timerFill.fillAmount = timeLeft / maxTime;

            if (timeLeft < 0) OnRoundOver(false);
        }

        private void OnRoundOver(bool hasSucceded)
        {
            Score.Add(timeLeft / 5f);
            Parameter.isSuccess = hasSucceded;
            TargetChannel.Evaluate(Parameter);
            SizeChannel.Evaluate(Parameter);
            
            currentTile = 0;
            
            timeLeft = maxTime;
            GenerateGrid((int)TileSize.Value);
        }

        public void GenerateGrid(int size)
        {
            ClearTiles();

            for (int i = 0; i < size * size; i++)
            {
                var tile = Instantiate(tilePrefab, gridLayout.transform).GetComponent<Tile>();
                tile.OnClicked += OnTileClicked;
                tiles.Add(tile);
            }

            for (int i = 0; i < (int)TargetTile.Value; i++)
            {
                Tile randTile;

                do
                {
                    randTile = tiles[Random.Range(0, tiles.Count)];
                } while (randTile.IsOn);

                randTile.SetOn(true);
            }
        }

        private void ClearTiles()
        {
            foreach (var tile in tiles)
            {
                tile.OnClicked -= OnTileClicked;
                Destroy(tile.gameObject);
            }
        
            tiles.Clear();
        }
        
        private void OnTileClicked(Tile tile)
        {
            if (tile.IsOn)
            {
                tile.SetOn(false);
                currentTile++;
                if (currentTile >= (int)TargetTile.Value)
                    OnRoundOver(true);

            }
            else OnRoundOver(false);

        }
    }
}
