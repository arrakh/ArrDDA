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

        private void Start()
        {
            var widthJson = PlayerPrefs.GetString("width", String.Empty);
            var heightJson = PlayerPrefs.GetString("height", String.Empty);
            
            boardWidthChannel.InitializeFromJson(widthJson);
            boardHeightChannel.InitializeFromJson(heightJson);
            boardRoundTime.Initialize();
            
            var difficulty = new GameDifficulty
            (
                Mathf.CeilToInt(boardWidthChannel.GetDifficulty()),
                Mathf.CeilToInt(boardHeightChannel.GetDifficulty()),
                boardRoundTime.GetDifficulty()
            );
            
            gameController.GenerateGame(difficulty);
        }

        private void OnEnable() => Events.OnRoundCompleted += OnRoundCompleted;
        private void OnDisable() => Events.OnRoundCompleted -= OnRoundCompleted;

        private void OnRoundCompleted(RoundResult result)
        {
            //Evaluate difficulty here based on previous result
            var param = new AdaptParameter(result.isWin);

            var widthJson = boardWidthChannel.GetHistoryAsJson();
            var heightJson = boardHeightChannel.GetHistoryAsJson();
            
            PlayerPrefs.SetString("width", widthJson);
            PlayerPrefs.SetString("height", heightJson);

            var timeProg = (result.difficulty.roundTime - result.timeSpent) / 5f;
            var difficulty = new GameDifficulty
            (
                Mathf.CeilToInt(boardWidthChannel.Evaluate(1, param)),
                Mathf.CeilToInt(boardHeightChannel.Evaluate(1, param)),
                boardRoundTime.Evaluate(timeProg)
            );
            
            gameController.GenerateGame(difficulty);
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