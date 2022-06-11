using System;
using UnityEngine;

namespace Arr.DDA.Script
{
    public abstract class ChannelObject : ScriptableObject
    {
        public ChannelDrawSetting drawSetting = ChannelDrawSetting.Default;

        public ChannelData startingData = ChannelData.Default;

        public void Initialize()
        {
            DynamicDifficulty.Initialize(this, startingData, GetEvaluator());
        }

        public void Initialize(ChannelHistory history)
        {
            DynamicDifficulty.Initialize(this, history, GetEvaluator());
        }

        public void InitializeFromJson(string historyJson)
        {
            if (historyJson.Equals(String.Empty)) Initialize();
            else
            {
                var history = JsonUtility.FromJson<ChannelHistory>(historyJson);
                Initialize(history);
            }
        }

        public float GetDifficulty()
        {
            if (DynamicDifficulty.TryGetData(this, out var data)) return data.currentDifficulty;
            return startingData.currentDifficulty;
        }

        protected virtual IEvaluator GetEvaluator() => null;

        public virtual float Evaluate(float newProgression, bool record = true, bool isDelta = true)
        {
            return DynamicDifficulty.Evaluate(this, newProgression, record, isDelta).currentDifficulty;
        }

        protected virtual float Evaluate<T>(float newProgression, T parameter, bool record = true, bool isDelta = true)
        {
            return DynamicDifficulty.Evaluate(this, newProgression, parameter, record, isDelta).currentDifficulty;
        }

        public string GetHistoryAsJson()
        {
            if (!DynamicDifficulty.TryGetHistory(this, out var history))
            {
                throw new Exception("Cannot Get History. Are you calling this outside of Play Mode?");
            }

            return JsonUtility.ToJson(history);
        }
    }
    
    [Serializable]
    public struct ChannelDrawSetting
    {
        public Vector2 zoom;
        public float graphSize, cellIncrements, padding, height;
        public bool drawPoints;
        public bool drawLineGradient;
        public float lineGradientDistance;
        public int lineGradientAmount;

        public static ChannelDrawSetting Default => new ()
        {
            graphSize = 10f,
            height = 240,
            padding = 0.5f,
            lineGradientDistance = 0.1f,
            lineGradientAmount = 20,
            zoom = Vector2.one,
            cellIncrements = 0.5f
        };
    }
}