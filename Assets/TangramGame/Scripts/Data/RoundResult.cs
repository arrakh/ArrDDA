namespace TangramGame.Scripts
{
    public struct RoundResult
    {
        public bool isWin;
        public GameDifficulty difficulty;

        public RoundResult(bool isWin, GameDifficulty difficulty)
        {
            this.isWin = isWin;
            this.difficulty = difficulty;
        }
    }
}