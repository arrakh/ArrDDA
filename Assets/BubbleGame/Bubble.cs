using UnityEngine;

namespace BubbleGame
{
    public class Bubble : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float moveSpeed;

        public void Blow(Vector2 direction)
        {
            rb.velocity = direction * moveSpeed;
        }
    }
}