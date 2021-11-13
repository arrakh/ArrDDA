using System;
using UnityEngine;

namespace Arr.DDA.Script
{
    [CreateAssetMenu(fileName = "New Channel", menuName = ProjectConst.PROJECT_NAME + "/Channel", order = 0)]
    public class ChannelObject : ScriptableObject
    {
        public string ChannelName;
        public MetricObject ChallengeMetric;
        public MetricObject SkillMetric;
        public ChannelSetting Setting;
        public Type Evaluator;

        private Channel channel = null;

        public void CreateChannel()
        {
            IEvaluator eval = Activator.CreateInstance(Evaluator) as IEvaluator;
            channel = new Channel(ChannelName, eval, ChallengeMetric.GetMetric(), SkillMetric.GetMetric(), Setting);
        }

    }
}