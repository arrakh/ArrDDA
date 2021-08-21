using System;
using UnityEngine;

namespace Arr.DDA
{
    public class Metric : IDisposable
    {
        public Metric(float value, float minValue = default, float maxValue = default, Action<float> onChanged = null)
        {
            OnChanged += onChanged;
            Value = value;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public float Value { get; private set; }

        public float MinValue { get; private set; } = float.MinValue;
        public float MaxValue { get; private set; } = float.MaxValue;

        public Action<float> OnChanged;

        public void SetValue(float newValue)
        {
            Value = Mathf.Clamp(newValue, MinValue, MaxValue);
            OnChanged?.Invoke(Value);
        }

        public void Dispose()
        {
            OnChanged = null;
        }
    }
}