using System;
using UnityEngine;

namespace TangramGame.Scripts.Controllers
{
    public class HealthController : MonoBehaviour
    {
        public int maxHealth = 5;
        
        private int currentHealth;
        
        public void OnEnable()
        {
            currentHealth = maxHealth;
            
            Events.OnRoundCompleted += OnGameCompleted;
        }

        private void OnDisable()
        {
            Events.OnRoundCompleted -= OnGameCompleted;
        }

        private void OnGameCompleted(RoundResult result)
        {
            if (result.isWin) return;

            currentHealth--;
            Events.OnHealthChanged?.Invoke(currentHealth);
        }
    }
}
