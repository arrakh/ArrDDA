using System;
using UnityEngine;

namespace Arr.DDA.Script
{
    [CreateAssetMenu(fileName = "New Channel", menuName = ProjectConst.PROJECT_NAME + "/Channel", order = 0)]
    public class ChannelObject : ScriptableObject
    {
        public Action<float> OnEvaluated;
        public string ChannelName;
        public MetricObject DifficultyMetric;
        public MetricObject ProgressionMetric;
        public ChannelSetting Setting;
        [HideInInspector] public string Evaluator;

        private Channel channel = null;

        public void Initialize()
        {
            DifficultyMetric.CreateMetric();
            ProgressionMetric.CreateMetric();
            
            var evalType = Type.GetType(Evaluator);
            if (evalType == null) throw new Exception($"No Evaluator found with type {Evaluator}!");
            IEvaluator eval = Activator.CreateInstance(evalType) as IEvaluator;
            channel = new Channel(ChannelName, eval, DifficultyMetric.Get(), ProgressionMetric.Get(), Setting);
            channel.OnEvaluated = OnEvaluated;
            Debug.Log($"Created Channel for {ChannelName} with Evaluator {eval.GetType()}");
        }

        public float Evaluate(EvaluationParameter parameter) => channel.Evaluate(parameter);

        public float Evaluate()
        {
            var param = new EvaluationParameter();
            return channel.Evaluate(param);
        }

    }
}