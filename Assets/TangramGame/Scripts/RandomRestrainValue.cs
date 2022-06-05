using Arr.DDA;
using Arr.DDA.Script;
using UnityEngine;

namespace TangramGame.Scripts
{
    public class RandomRestrainValue : IEvaluator
    {
        private float minRegulationValue = 0.5f;
        private float maxRegulationValue = 1.5f;
        
        public RandomRestrainValue(float minRegulationValue, float maxRegulationValue)
        {
            this.minRegulationValue = minRegulationValue;
            this.maxRegulationValue = maxRegulationValue;
        }

        public ChannelData Evaluate(ChannelData data)
        {
            float diff = data.currentDifficulty;
            float anxiety = data.GetAnxietyThreshold();
            float boredom = data.GetBoredomThreshold();
            var rand = Random.Range(minRegulationValue, maxRegulationValue);
            if (diff > anxiety) diff -= rand;
            else if (diff < boredom ) diff += rand;
            //Debug.Log($"Evaluating Restrain Value, Diff: {diff}, Anx: {anxiety}, Bor: {boredom}");
            data.currentDifficulty = diff;
            return data;
        }
    }
}
