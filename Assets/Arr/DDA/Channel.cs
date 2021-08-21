using System;

namespace Arr.DDA
{
    [Serializable]
    public class Channel : IDisposable
    {
        public Action<float> OnEvaluated;
        
        private string channelName;
        private IEvaluator evaluator;
        private Metric challengeMetric;
        private Metric skillMetric;
        private ChannelSetting setting;

        public Channel(string channelName, IEvaluator evaluator, Metric challengeMetric, Metric skillMetric, ChannelSetting setting)
        {
            this.channelName = channelName;
            this.evaluator = evaluator;
            this.challengeMetric = challengeMetric;
            this.skillMetric = skillMetric;
            this.setting = setting;
        }

        public void Dispose()
        {
            challengeMetric?.Dispose();
            skillMetric?.Dispose();
        }
    }
}