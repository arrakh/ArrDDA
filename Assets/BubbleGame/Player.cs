using System;
using UnityEngine;

namespace BubbleGame
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float speed = 3f;
        private Vector3 movement;
        private Bubble bubble;
    
        void Update()
        {
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");

            transform.position += movement * (speed * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.Space) && bubble != null)
            {
                var dir = (bubble.transform.position - transform.position).normalized;
                bubble.Blow(dir);
                Debug.Log($"Blowing {dir}");
            }   
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<Bubble>(out var b))
            {
                Debug.Log("Got Bubble");
                bubble = b;
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<Bubble>(out var b)) bubble = null;
        }
    }
}
