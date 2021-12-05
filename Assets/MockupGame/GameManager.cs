using System;
using System.Collections.Generic;
using Arr.DDA.Script;
using UnityEngine;

namespace MockupGame
{
    public class GameManager : MonoBehaviour
    {
        public int Score;
        public float timeLeft;

        [SerializeField] private GameObject gridPrefab;
        [SerializeField] private GridLayout gridLayout;
        [SerializeField] private ChannelObject channel;

        private List<Tile> tiles = new List<Tile>();
        private bool isPlaying = false;

        private void Start()
        {
            GenerateGrid(3);
            timeLeft = 3f;
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

            if (timeLeft < 0) OnRoundOver();
        }

        private void OnRoundOver()
        {
            
        }

        public void GenerateGrid(int size)
        {
            ClearTiles();

            for (int i = 0; i < size * size; i++)
            {
                var tile = Instantiate(gridPrefab, gridLayout.transform).GetComponent<Tile>();
                tile.OnClicked += OnTileClicked;
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
                Score++;
            }
        }
    }
}
