using System;
using Arr.DDA.Script;
using Arr.DDA.Script.Evaluators;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TangramGame.Scripts
{
    public class GameDifficultyController : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        [SerializeField] private AdaptValueChannel boardWidthChannel;
        [SerializeField] private AdaptValueChannel boardHeightChannel;
        [SerializeField] private RestrainValueChannel boardRoundTime;

        private int currentRound = 0;

        private void Start()
        {
            boardHeightChannel.Initialize();
            boardWidthChannel.Initialize();
            gameController.GenerateGame(GetDifficulty());
        }

        private void OnEnable() => Events.OnRoundCompleted += OnRoundCompleted;
        private void OnDisable() => Events.OnRoundCompleted -= OnRoundCompleted;

        private void OnRoundCompleted(RoundResult result)
        {
            //Evaluate difficulty here based on previous result
            currentRound++;
            var param = new AdaptParameter(result.isWin);
            boardWidthChannel.Evaluate(currentRound, param, true);
            boardHeightChannel.Evaluate(currentRound, param, true);
            boardRoundTime.Evaluate(currentRound, true);
            
            gameController.GenerateGame(GetDifficulty());
        }

        private GameDifficulty GetDifficulty()
        {
            return new GameDifficulty
            (
                Mathf.CeilToInt(boardWidthChannel.Difficulty),
                Mathf.CeilToInt(boardHeightChannel.Difficulty),
                boardRoundTime.Difficulty
            );
        }
        
        private GameDifficulty GetDifficultyRandom()
        {
            return new GameDifficulty
            (
                Random.Range(3, 8),
                Random.Range(3, 8), 
                Random.Range(10, 20),
                Random.Range(2, 4),
                Random.Range(6, 9)
            );
        }
    }
}