using Arr.DDA.Script;

namespace Arr.DDA
{
    public interface IEvaluator
    {
        float OnInternalEvaluate(Metric difficulty, Metric progression, ChannelSetting channel, EvaluationParameter parameter);
    }
    
}