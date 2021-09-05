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

        private void Awake()
        {
            if(metric == null) CreateMetric();
        }

        void CreateMetric()
        {
            metric = new Metric(DefaultValue, MinMaxValue.x, MinMaxValue.y, OnChanged);
        }

        public Metric GetMetric()
        {
            if(metric == null) CreateMetric();
            return metric;
        }
    }
}