using UnityEngine;

namespace Arr.DDA.Script
{
    [CreateAssetMenu(fileName = "New Channel", menuName = ProjectConst.PROJECT_NAME + "/Channel", order = 0)]
    public class ChannelObject : ScriptableObject
    {
        public string ChannelName;
        public EvaluateType EvaluateType;
        public MetricObject ChallengeMetric;
        public MetricObject SkillMetric;
        public ChannelSetting Setting;

        private Channel channel = null;

        private void OnEnable()
        {
            if(channel == null) CreateChannel();
        }

        private void CreateChannel()
        {
            IEvaluator evaluator = null;
            switch (EvaluateType)
            {
                case EvaluateType.Adapt:
                    evaluator = new AdaptValue();
                    break;
                case EvaluateType.Restrain:
                    evaluator = new RestrainValue();
                    break;
            }

            channel = new Channel(ChannelName, evaluator, ChallengeMetric.GetMetric(), SkillMetric.GetMetric(), Setting);
        }
    }

    public enum EvaluateType
    {
        Adapt,
        Restrain
    }
}