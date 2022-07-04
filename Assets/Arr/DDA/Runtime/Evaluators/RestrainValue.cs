using UnityEngine;

namespace Arr.DDA.Script.Evaluators
{
    public class RestrainValue : IEvaluator
    {
        private float regulationValue = 1f;

        public RestrainValue(float regulationValue)
        {
            this.regulationValue = regulationValue;
        }

        public ChannelData Evaluate(ChannelData data)
        {
            float diff = data.currentDifficulty;
            float anxiety = data.GetAnxietyThreshold();
            float boredom = data.GetBoredomThreshold();
            if (diff > anxiety) diff -= regulationValue;
            else if (diff < boredom ) diff += regulationValue;
            //Debug.Log($"Evaluating Restrain Value, Diff: {diff}, Anx: {anxiety}, Bor: {boredom}");
            data.currentDifficulty = diff;
            return data;
        }
    }
}