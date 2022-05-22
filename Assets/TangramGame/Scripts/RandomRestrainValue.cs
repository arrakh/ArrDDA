using Arr.DDA;
using Arr.DDA.Script;
using UnityEngine;

namespace TangramGame.Scripts
{
    public class RandomRestrainValue : Evaluator<EvaluationParameter>
    {
        private const float REGULATION_VALUE_MIN = 0.5f;
        private const float REGULATION_VALUE_MAX = 1.5f;
        public override float OnEvaluate(Metric difficulty, Metric progression, ChannelSetting channel, EvaluationParameter parameter)
        {
            float currentDiff = difficulty.Value;
            float anxiety = channel.GetAnxietyThreshold(progression.Value);
            float boredom = channel.GetBoredomThreshold(progression.Value);
            var rand = Random.Range(REGULATION_VALUE_MIN, REGULATION_VALUE_MAX);
            if (currentDiff > anxiety) currentDiff -= rand;
            else if (currentDiff < boredom ) currentDiff += rand;
            Debug.Log($"Evaluating Restrain Value, Diff: {currentDiff}, Anx: {anxiety}, Bor: {boredom}, Cha: {difficulty.Value}, Ski: {progression.Value}");
            return currentDiff;
        }
    }
}
