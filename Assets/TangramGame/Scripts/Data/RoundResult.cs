namespace TangramGame.Scripts
{
    public struct RoundResult
    {
        public bool isWin;
        public GameDifficulty difficulty;
        public float timeSpent;

        public RoundResult(bool isWin, GameDifficulty difficulty, float timeSpent)
        {
            this.isWin = isWin;
            this.difficulty = difficulty;
            this.timeSpent = timeSpent;
        }
    }
}