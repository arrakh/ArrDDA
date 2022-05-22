using UnityEngine;

namespace TangramGame.Scripts
{
    public class ScoreController : MonoBehaviour
    {
        public int score;
        
        private void OnEnable() => Events.OnRoundCompleted += OnRoundCompleted;
        private void OnDisable() => Events.OnRoundCompleted -= OnRoundCompleted;

        private void OnRoundCompleted(RoundResult result)
        {
            if (!result.isWin) return;
            score += GetScoreDelta(result.difficulty);
            Events.OnScoreChanged?.Invoke(score);
        }

        private int GetScoreDelta(GameDifficulty difficulty)
        {
            return 10;
        }
    }
}