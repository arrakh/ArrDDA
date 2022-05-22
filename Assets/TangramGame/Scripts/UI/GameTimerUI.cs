using UnityEngine;
using UnityEngine.UI;

namespace TangramGame.Scripts.UI
{
    public class GameTimerUI : MonoBehaviour
    {
        [SerializeField] [Range(0f, 1f)] private float currentFill;
        [SerializeField] private Gradient colorGradient;
        [SerializeField] private Image image;

        private GameTimer currentTimer;
        private void OnEnable()
        {
            Events.OnNewGameTimer += OnNewGameTimer;
        }

        private void OnDisable()
        {
            Events.OnNewGameTimer -= OnNewGameTimer;
        }

        private void OnNewGameTimer(GameTimer timer)
        {
            Debug.Log("NEW GAME TIMER!");
            currentTimer = timer;
            timer.OnTimerUpdated.AddListener(OnTimerUpdated);
            timer.OnTimerEnd.AddListener(OnTimerEnd);
        }

        private void OnTimerEnd()
        {
            
        }

        private void OnTimerUpdated(float t)
        {
            currentFill = currentTimer.Normalized;
            UpdateFill(currentFill);
        }

        private void UpdateFill(float t)
        {
            image.fillAmount = t;
            image.color = colorGradient.Evaluate(t);
        }

        private void OnValidate()
        {
            UpdateFill(currentFill);
        }
    }
}
