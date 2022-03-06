using UnityEngine;

namespace TangramGame.Scripts
{
    public class TilePieceController : MonoBehaviour
    {
        private TileContent tileContent;

        public void Setup(TileContent tp)
        {
            tileContent = tp;
        }
    }
}