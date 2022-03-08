using UnityEngine;

namespace TangramGame.Scripts
{
    public class TileContentElement : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sprite;

        public void Setup(Color color)
        {
            sprite.color = color;
        }
    }
}