using UnityEngine;

namespace Arr.DDA.Script.Evaluators
{
    public class RestrainValue : Evaluator<EvaluationParameter>
    {
        private const float REGULATION_VALUE = 0.4f;
        public override float OnEvaluate(Metric difficulty, Metric progression, ChannelSetting channel, EvaluationParameter parameter)
        {
            float currentDiff = difficulty.Value;
            float anxiety = channel.GetAnxietyThreshold(progression.Value);
            float boredom = channel.GetBoredomThreshold(progression.Value);
            if (currentDiff > anxiety) currentDiff -= REGULATION_VALUE;
            else if (currentDiff < boredom ) currentDiff += REGULATION_VALUE;
            Debug.Log($"Evaluating Restrain Value, Diff: {currentDiff}, Anx: {anxiety}, Bor: {boredom}, Cha: {difficulty.Value}, Ski: {progression.Value}");
            return currentDiff;
        }
    }
}