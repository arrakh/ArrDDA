using UnityEngine;

namespace Arr.DDA.Script.Evaluators
{
    [CreateAssetMenu(menuName = "ArrDDA/Restrain Value Channel")]
    public class RestrainValueChannel : ChannelObject
    {
        public float regulationValue = 1f;

        protected override IEvaluator GetEvaluator()
        {
            return new RestrainValue(regulationValue);
        }
    }
}