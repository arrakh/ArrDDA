using System;
using UnityEngine;
using UnityEngine.Events;

namespace TangramGame.Scripts
{
    public class GameTimer
    {
        public UnityEvent<float> OnTimerUpdated = new UnityEvent<float>();
        public UnityEvent OnTimerEnd = new UnityEvent();

        public bool IsRunning { get; private set; } = false;

        public float Current { get; private set; }
        public float Normalized => Mathf.Clamp01(Current / MaxTimer);
        
        public float MaxTimer { get; private set; }

        public GameTimer(float timer)
        {
            MaxTimer = timer;
            Current = timer;
            IsRunning = true;
        }

        public void Update(float delta)
        {
            if (!IsRunning) return;
            
            Current -= delta;
            OnTimerUpdated?.Invoke(Current);
            
            if (Current <= 0f) TimerEnd();
        }

        private void TimerEnd()
        {
            IsRunning = false;
            OnTimerEnd?.Invoke();
        }
    }
}
