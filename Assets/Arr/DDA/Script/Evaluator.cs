using System;

namespace Arr.DDA.Script
{
    public abstract class Evaluator<T> : IEvaluator
    where T : EvaluationParameter
    {
        public float OnInternalEvaluate(Metric difficulty, Metric progression, ChannelSetting channel, EvaluationParameter parameter)
        {
            if (parameter is T param) return OnEvaluate(difficulty, progression, channel, param);
            else throw new Exception($"This evaluator param is NOT type of {typeof(T)}");
        }

        public virtual float OnEvaluate(Metric difficulty, Metric progression, ChannelSetting channel, T parameter) => difficulty.Value;
    }
}