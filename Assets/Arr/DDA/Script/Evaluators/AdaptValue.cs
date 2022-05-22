using UnityEngine;

namespace Arr.DDA.Script.Evaluators
{
    public class AdaptValue : Evaluator<AdaptParameter>
    {
        private const float REGULATION_VALUE = 1f;
        private const float SMALL_MOD = 0.5f;
        private const float LARGE_MOD = 2f;

        public override float OnEvaluate(Metric difficulty, Metric progression, ChannelSetting channel, AdaptParameter parameter)
        {
            var diff = difficulty.Value;

            bool isAnxious = diff > channel.GetAnxietyThreshold(progression.Value);
            bool isBored = diff < channel.GetBoredomThreshold(progression.Value);

            if (isAnxious)
            {
                if (parameter.isSuccess)
                {
                    diff += REGULATION_VALUE * SMALL_MOD;
                    channel.AnxietyThreshold = Mathf.Clamp(channel.AnxietyThreshold += REGULATION_VALUE, -channel.BoredomThreshold, float.MaxValue);
                }
                else diff -= REGULATION_VALUE * LARGE_MOD;
            }

            else if (isBored)
            {
                if (parameter.isSuccess) diff += REGULATION_VALUE * LARGE_MOD;
                else
                {
                    diff -= REGULATION_VALUE * SMALL_MOD;
                    channel.BoredomThreshold = Mathf.Clamp(channel.BoredomThreshold - REGULATION_VALUE, -channel.AnxietyThreshold, float.MaxValue);
                    Debug.Log($"{diff} -= {REGULATION_VALUE} * {SMALL_MOD} = {diff -= REGULATION_VALUE * SMALL_MOD}");
                }
            }

            else diff += parameter.isSuccess ? REGULATION_VALUE : -REGULATION_VALUE;

            return diff;
        }
    }
    
    public class AdaptParameter : EvaluationParameter
    {
        public bool isSuccess;

        public AdaptParameter(bool isSuccess)
        {
            this.isSuccess = isSuccess;
        }

        public AdaptParameter()
        {
        }
    }
}