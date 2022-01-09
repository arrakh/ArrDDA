using System;
using Arr.DDA.Script;
using UnityEngine;
using UnityEngine.Internal;

namespace Arr.DDA
{
    [CreateAssetMenu(fileName = "New Metric", menuName = ProjectConst.PROJECT_NAME + "/Metric", order = 0)]
    public class MetricObject : ScriptableObject
    {
        public float DefaultValue;
        public Vector2 MinMaxValue;
        public Action<float> OnChanged;

        private Metric metric = null;
        public float Value => Get().Value;

        private void Awake()
        {
            if(metric == null) CreateMetric();
        }

        public void CreateMetric()
        {
            if (metric != null) return;
            metric = new Metric(DefaultValue, MinMaxValue.x, MinMaxValue.y, OnChanged);
        }

        public Metric Get()
        {
            if(metric == null) CreateMetric();
            return metric;
        }


        public void Set(float value) => metric.SetValue(value);
        public void Add(float delta) => metric.SetValue(metric.Value + delta);
    }
}