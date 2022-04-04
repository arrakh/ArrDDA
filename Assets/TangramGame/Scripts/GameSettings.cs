namespace TangramGame.Scripts
{
    public struct GameSettings
    {
        public int width, height, minPieceSieze, maxPieceSize;
        public float roundTime;

        public GameSettings(int width, int height, float roundTime, int minPieceSieze = 2, int maxPieceSize = 5)
        {
            this.width = width;
            this.height = height;
            this.minPieceSieze = minPieceSieze;
            this.maxPieceSize = maxPieceSize;
            this.roundTime = roundTime;
        }
    }
}