using Arr.DDA.Script;

namespace Arr.DDA
{
    public interface IEvaluator
    {
        public ChannelData Evaluate(ChannelData data);
    }
    
    public interface IEvaluator<in TParameter> : IEvaluator
    {
        public ChannelData Evaluate(ChannelData data, TParameter param);
    }
}