using Arr.DDA;
using Arr.DDA.Script;
using UnityEngine;

namespace TangramGame.Scripts
{
    public class RandomRestrainValueChannel : ChannelObject
    {
        public Vector2 regulationValueRange = new Vector2(0.5f, 1.5f);
        
        protected override IEvaluator GetEvaluator() 
            => new RandomRestrainValue(regulationValueRange.x, regulationValueRange.y);
    }
}