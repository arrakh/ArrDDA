using UnityEngine;

namespace Arr.DDA.Script.Evaluators
{
    [CreateAssetMenu(menuName = "ArrDDA/Adapt Value Channel")]
    public class AdaptValueChannel : ChannelObject
    {
        public float regulationValue = 1f;
        public float smallModifier = 0.5f;
        public float bigModifier = 2f;
        
        protected override IEvaluator GetEvaluator() => new AdaptValue(regulationValue, smallModifier, bigModifier);

        public float Evaluate(float newProgression, AdaptParameter parameter, bool record = true)
            => Evaluate<AdaptParameter>(newProgression, parameter, record);
    }
}