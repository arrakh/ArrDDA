using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arr.DDA.Script
{
    [AddComponentMenu("")]
    public class ChannelController : MonoBehaviour
    {
        public Action OnDestroyed;
        
        private Dictionary<ChannelObject, IEvaluator> evaluators = new Dictionary<ChannelObject, IEvaluator>();
        private Dictionary<ChannelObject, ChannelHistory> histories = new Dictionary<ChannelObject, ChannelHistory>();
        private Dictionary<ChannelObject, ChannelData> lastData = new Dictionary<ChannelObject, ChannelData>();

        private void OnDestroy()
        {
            OnDestroyed?.Invoke();
        }

        public void Initialize(ChannelObject id, ChannelData startingData, IEvaluator evaluator)
        {
            evaluators[id] = evaluator;
            histories[id] = new ChannelHistory(startingData);
        }
        
        public void Initialize(ChannelObject id, ChannelHistory history, IEvaluator evaluator)
        {
            evaluators[id] = evaluator;
            histories[id] = history;
        }

        public ChannelData Evaluate(ChannelObject id, float newProgression, bool record, bool isDelta)
        {
            if (!evaluators.TryGetValue(id, out var evaluator))
            {
                Debug.LogError($"Trying to get evaluator for id {id} but failed!");
                return ChannelData.Default;
            }
            
            var data = TryGetData(id);
            if (isDelta) data.currentProgression += newProgression;
            else data.currentProgression = newProgression;
            var newData = evaluator.Evaluate(data);
            lastData[id] = newData;
            
            if (record) TryRecord(id, newData);

            return newData;
        }
        
        public ChannelData Evaluate<T>(ChannelObject channel, float newProgression, T parameter, bool record, bool isDelta)
        {
            if (!evaluators.TryGetValue(channel, out var evaluator))
            {
                Debug.LogError($"Trying to get evaluator for channel {channel} but failed!");
                return ChannelData.Default;
            }

            if (!(evaluator is IEvaluator<T> paramEval))
            {
                Debug.LogError($"Trying to evaluate but evaluator with channel {channel} is NOT of type {typeof(T)}");
                return ChannelData.Default;
            }

            var data = TryGetData(channel);
            if (isDelta) data.currentProgression += newProgression;
            else data.currentProgression = newProgression;
            var newData = paramEval.Evaluate(data, parameter);
            lastData[channel] = newData;
            
            if (record) TryRecord(channel, newData);

            return newData;
        }

        public bool TryGetHistory(ChannelObject channel, out ChannelHistory history)
        {
            if (!histories.TryGetValue(channel, out history))
            {
                bool findLastData = lastData.TryGetValue(channel, out var data);
                if (findLastData)
                {
                    history = new ChannelHistory(data);
                    return true;
                }
            }
            else return true;

            return false;
        }

        public bool TryGetData(ChannelObject channel, out ChannelData data)
        {
            if (lastData.TryGetValue(channel, out data)) return true;
            return false;
        }

        private ChannelData TryGetData(ChannelObject id)
        {
            bool hasHistory = histories.TryGetValue(id, out var history);

            var data = ChannelData.Default;

            if (!lastData.TryGetValue(id, out data) && hasHistory) data = history.LatestData;

            return data;
        }

        private void TryRecord(ChannelObject id, ChannelData newData)
        {
            if (histories.TryGetValue(id, out var history))
            {
                history.Add(newData, 1000);
                Debug.Log($"Recorded {name} with new data. History is now: \n{histories[id]}");
            }
            else histories[id] = new ChannelHistory(newData);
        }
    }
}