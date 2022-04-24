namespace TangramGame.Scripts
{
    public struct GameDifficulty
    {
        public int width, height, minPieceSize, maxPieceSize;
        public float roundTime;

        public GameDifficulty(int width, int height, float roundTime, int minPieceSize = 2, int maxPieceSize = 5)
        {
            this.width = width;
            this.height = height;
            this.minPieceSize = minPieceSize;
            this.maxPieceSize = maxPieceSize;
            this.roundTime = roundTime;
        }
    }
}