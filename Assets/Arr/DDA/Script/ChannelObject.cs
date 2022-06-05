using System;
using UnityEngine;

namespace Arr.DDA.Script
{
    public abstract class ChannelObject : ScriptableObject
    {
        [SerializeField] protected ChannelData startingData = ChannelData.Default;

        private bool initialized = false;
        private Guid channelId = Guid.Empty;
        private ChannelData currentData;

        public Guid Id => channelId;
        public ChannelData Data => initialized ? currentData : startingData;

        public void Initialize()
        {
            if (initialized)
                throw new Exception($"You are trying to Initialize {name} while it is already initialized!");

            channelId = Guid.NewGuid();
            currentData = startingData;
            
            DynamicDifficulty.Initialize(channelId, currentData, GetEvaluator());
        }

        protected virtual IEvaluator GetEvaluator() => null;

        public virtual float Evaluate(float newProgression, bool record = true)
        {
            currentData = DynamicDifficulty.Evaluate(channelId, newProgression, record);
            return currentData.currentDifficulty;
        }

        protected virtual float Evaluate<T>(float newProgression, T parameter, bool record = true)
        {
            currentData = DynamicDifficulty.Evaluate(channelId, newProgression, parameter, record);
            return currentData.currentDifficulty;
        }

        public float Difficulty => currentData.currentDifficulty;
    }
}