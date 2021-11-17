namespace Arr.DDA.Script.Evaluators
{
    public class AdaptValue : Evaluator<AdaptParameter>
    {
        private const float REGULATION_VALUE = 1f;
        private const float SMALL_MOD = 0.5f;
        private const float LARGE_MOD = 3f;

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
                    channel.AnxietyThreshold += REGULATION_VALUE;
                }
                else diff -= REGULATION_VALUE * LARGE_MOD;
            }

            if (isBored)
            {
                if (parameter.isSuccess) diff += REGULATION_VALUE * LARGE_MOD;
                else
                {
                    diff -= REGULATION_VALUE * SMALL_MOD;
                    channel.BoredomThreshold += REGULATION_VALUE;
                }
            }

            return diff;
        }
    }
    
    public class AdaptParameter : EvaluationParameter
    {
        public bool isSuccess;
    }
}