using System;
using UnityEngine;

namespace Arr.DDA.Script
{
    [Serializable]
    public class Channel : IDisposable
    {
        public Action<float> OnEvaluated;
        
        private string channelName;
        private IEvaluator evaluator;
        private Metric difficulty;
        private Metric progression;
        private ChannelSetting setting;

        public Channel(string channelName, IEvaluator evaluator, Metric difficulty, Metric progression, ChannelSetting setting)
        {
            this.channelName = channelName;
            this.evaluator = evaluator;
            this.difficulty = difficulty;
            this.progression = progression;
            this.setting = setting;
            Debug.Log($"Constructor for {channelName} is called! Evaluator is null {evaluator == null}");
        }

        public float Evaluate(EvaluationParameter parameter)
        {
            var result = evaluator.OnInternalEvaluate(difficulty, progression, setting, parameter);
            difficulty.SetValue(result);
            OnEvaluated?.Invoke(result);
            return result;
        }

        public void Dispose()
        {
            difficulty?.Dispose();
            progression?.Dispose();
        }
    }
}