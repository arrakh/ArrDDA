using System;
using UnityEngine;

namespace Arr.DDA
{
    public class Metric : IDisposable
    {
        public Metric(float value, float minValue = 0f, float maxValue = 0f, Action<float> onChanged = null)
        {
            OnChanged += onChanged;
            Value = value;
            MinValue = minValue;
            MaxValue = maxValue;
            shouldClamp = Math.Abs(minValue - 0f) > 0.01 || Math.Abs(maxValue - 0f) > 0.01f;
        }

        public float Value { get; private set; }

        public float MinValue { get; private set; } = float.MinValue;
        public float MaxValue { get; private set; } = float.MaxValue;

        public Action<float> OnChanged;

        private bool shouldClamp;

        public void SetValue(float newValue)
        {
            Value = shouldClamp? Mathf.Clamp(newValue, MinValue, MaxValue) : newValue;
            OnChanged?.Invoke(Value);
        }

        public void Dispose()
        {
            OnChanged = null;
        }
    }
}