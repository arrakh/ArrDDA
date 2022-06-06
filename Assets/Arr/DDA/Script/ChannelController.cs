using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arr.DDA.Script
{
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

        public ChannelData Evaluate(ChannelObject id, float newProgression, bool record)
        {
            if (!evaluators.TryGetValue(id, out var evaluator))
            {
                Debug.LogError($"Trying to get evaluator for id {id} but failed!");
                return ChannelData.Default;
            }
            
            var data = TryGetData(id);
            data.currentProgression = newProgression;
            var newData = evaluator.Evaluate(data);
            lastData[id] = newData;
            
            if (record) TryRecord(id, newData);

            return newData;
        }
        
        public ChannelData Evaluate<T>(ChannelObject channel, float newProgression, T parameter, bool record)
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
            data.currentProgression = newProgression;
            var newData = paramEval.Evaluate(data, parameter);
            lastData[channel] = newData;
            
            if (record) TryRecord(channel, newData);

            return newData;
        }

        public void OverrideHistory(ChannelObject id, ChannelHistory newHistory)
            => histories[id] = newHistory;

        public bool TryGetHistory(ChannelObject channel, out ChannelHistory history)
        {
            Debug.Log("Trying to get history...");
            if (!histories.TryGetValue(channel, out history))
            {
                Debug.Log("Couldn't get history, will get last data...");
                bool findLastData = lastData.TryGetValue(channel, out var data);
                if (findLastData)
                {
                    history = new ChannelHistory(data);
                    return true;
                }
            }
            else return true;

            Debug.Log("Didn't get any?!?!");
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
                history.AddRecord(newData);
                Debug.Log($"Recorded {name} with new data: {newData}");
            }
            else histories[id] = new ChannelHistory(newData);
        }
    }
}