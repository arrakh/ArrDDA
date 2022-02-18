using System;
using UnityEngine;

namespace Arr.DDA.Script
{
    [CreateAssetMenu(fileName = "New Channel", menuName = ProjectConst.PROJECT_NAME + "/Channel", order = 0)]
    public class ChannelObject : ScriptableObject
    {
        public Action<float> OnEvaluated;
        public string ChannelName = "";
        public MetricObject DifficultyMetric;
        public MetricObject ProgressionMetric;
        public ChannelSetting Setting;
        [HideInInspector] public string Evaluator;

        private Channel channel = null;

        public Channel Channel
        {
            get
            {
                if(channel == null) Initialize();
                return channel;
            }
            set => channel = value;
        }

        public void Initialize()
        {
            DifficultyMetric.CreateMetric();
            ProgressionMetric.CreateMetric();
            
            var evalType = Type.GetType(Evaluator);
            if (evalType == null) throw new Exception($"No Evaluator found with type {Evaluator}!");
            IEvaluator eval = Activator.CreateInstance(evalType) as IEvaluator;
            Channel = new Channel(ChannelName, eval, DifficultyMetric.Get(), ProgressionMetric.Get(), Setting);
            Channel.OnEvaluated = OnEvaluated;
            Debug.Log($"Created Channel for {ChannelName} with Evaluator {eval.GetType()}");
        }

        public float Evaluate(float newProgression)
        { 
            ProgressionMetric.Set(newProgression);
            return Channel.Evaluate(null);
        }
        
        public float EvaluateDelta(float deltaProgression)
        { 
            ProgressionMetric.Add(deltaProgression);
            return Channel.Evaluate(null);
        }
        
        public float EvaluateWithParameter(float deltaProgression, EvaluationParameter parameter)
        { 
            ProgressionMetric.Set(deltaProgression);
            return Channel.Evaluate(parameter);
        }
        
        public float EvaluateDeltaWithParameter(float newProgression, EvaluationParameter parameter)
        { 
            ProgressionMetric.Set(newProgression);
            return Channel.Evaluate(parameter);
        }

        public float GetDifficulty() => DifficultyMetric.Value;

        public int GetDifficultyRounded() => Mathf.RoundToInt(DifficultyMetric.Value);
        public int GetDifficultyFloored() => Mathf.FloorToInt(DifficultyMetric.Value);
        public int GetDifficultyCeiled() => Mathf.CeilToInt(DifficultyMetric.Value);

        public float Evaluate()
        {
            var param = new EvaluationParameter();
            return Channel.Evaluate(param);
        }

    }
}