using UnityEngine;

namespace Arr.DDA.Script.Evaluators
{
    public class AdaptValue : IEvaluator<AdaptParameter>
    {
        private float regulationValue = 1f;
        private float smallModifier = 0.5f;
        private float largeModifier = 2f;

        public AdaptValue(float regulationValue, float smallModifier, float largeModifier)
        {
            this.regulationValue = regulationValue;
            this.smallModifier = smallModifier;
            this.largeModifier = largeModifier;
        }

        public AdaptValue() { }

        public ChannelData Evaluate(ChannelData data, AdaptParameter parameter)
        {
            //Debug.Log($"[ADAPT] Old Data - {data}");
            
            var diff = data.currentDifficulty;

            bool isAnxious = diff > data.GetAnxietyThreshold();
            bool isBored = diff < data.GetBoredomThreshold();

            if (isAnxious)
            {
                if (parameter.isSuccess)
                {
                    diff += regulationValue * smallModifier;
                    data.anxietyThreshold = Mathf.Clamp(data.anxietyThreshold += regulationValue, -data.boredomThreshold, float.MaxValue);
                }
                else diff -= regulationValue * largeModifier;
            }

            else if (isBored)
            {
                if (parameter.isSuccess) diff += regulationValue * largeModifier;
                else
                {
                    diff -= regulationValue * smallModifier;
                    data.boredomThreshold = Mathf.Clamp(data.boredomThreshold - regulationValue, -data.anxietyThreshold, float.MaxValue);
                }
            }

            else diff += parameter.isSuccess ? regulationValue : -regulationValue;

            data.currentDifficulty = diff;
            
            //Debug.Log($"[ADAPT] New Data - {data}");
            return data;
        }

        public ChannelData Evaluate(ChannelData data)
        {
            Debug.LogError("Trying to evaluate data without ADAPT VALUE parameter!");
            return data;
        }
    }
    
    public class AdaptParameter
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