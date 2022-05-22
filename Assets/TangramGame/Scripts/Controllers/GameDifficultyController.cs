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
        [SerializeField] private ChannelObject boardWidthChannel;
        [SerializeField] private ChannelObject boardHeightChannel;
        [SerializeField] private ChannelObject roundTimerChannel;

        private void Start() => gameController.GenerateGame(GetDifficulty());

        private void OnEnable() => Events.OnRoundCompleted += OnRoundCompleted;
        private void OnDisable() => Events.OnRoundCompleted -= OnRoundCompleted;

        private void OnRoundCompleted(RoundResult result)
        {
            //Evaluate difficulty here based on previous result

            var param = new AdaptParameter(result.isWin);
            boardWidthChannel.EvaluateDelta(1f, param);
            boardHeightChannel.EvaluateDelta(1f, param);
            roundTimerChannel.EvaluateDelta(1f, param);
            
            gameController.GenerateGame(GetDifficulty());
        }

        private GameDifficulty GetDifficulty()
        {
            return new GameDifficulty
            (
                boardWidthChannel.GetDifficultyRounded(),
                boardHeightChannel.GetDifficultyRounded(), 
                roundTimerChannel.GetDifficulty()
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