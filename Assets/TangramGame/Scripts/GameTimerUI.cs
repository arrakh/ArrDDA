using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TangramGame.Scripts
{
    public class GameTimerUI : MonoBehaviour
    {
        [SerializeField] private Slider timeSlider;
        [SerializeField] private TextMeshProUGUI timeText;

        private GameTimer currentTimer;
        private void OnEnable()
        {
            Events.OnNewGameTimer += OnNewGameTimer;
        }

        private void OnDisable()
        {
            Events.OnNewGameTimer += OnNewGameTimer;
        }

        private void OnNewGameTimer(GameTimer timer)
        {
            currentTimer = timer;
            timer.OnTimerUpdated.AddListener(OnTimerUpdated);
            timer.OnTimerEnd.AddListener(OnTimerEnd);
        }

        private void OnTimerEnd()
        {
            
        }

        private void OnTimerUpdated(float t)
        {
            timeSlider.value = currentTimer.Normalized;
            timeText.text = t.ToString("F2");
        }
    }
}
