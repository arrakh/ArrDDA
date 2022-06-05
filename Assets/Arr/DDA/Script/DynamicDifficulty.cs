using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arr.DDA.Script
{
    public static class DynamicDifficulty
    {
        /*private static ChannelController controller;

        [RuntimeInitializeOnLoadMethod]
        private static void Start()
        {
            controller = new GameObject("DynamicDifficultyAdjustment").AddComponent<ChannelController>();
            controller.gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
        }*/
        
        private static Dictionary<Guid, IEvaluator> evaluators = new Dictionary<Guid, IEvaluator>();
        private static Dictionary<Guid, ChannelHistory> histories = new Dictionary<Guid, ChannelHistory>();
        private static Dictionary<Guid, ChannelData> lastData = new Dictionary<Guid, ChannelData>();

        public static void Initialize(Guid id, ChannelData startingData, IEvaluator evaluator)
        {
            if (id == Guid.Empty) throw new Exception("Trying to register but id is empty!");
            
            evaluators[id] = evaluator;
            histories[id] = new ChannelHistory(startingData);
        }

        public static ChannelData Evaluate(Guid id, float newProgression, bool record = false)
        {
            if (id == Guid.Empty) throw new Exception("Trying to get an uninitialized channel!");

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
        
        public static ChannelData Evaluate<T>(Guid id, float newProgression, T parameter, bool record = false)
        {
            if (!evaluators.TryGetValue(id, out var evaluator))
            {
                Debug.LogError($"Trying to get evaluator for id {id} but failed!");
                return ChannelData.Default;
            }

            if (!(evaluator is IEvaluator<T> paramEval))
            {
                Debug.LogError($"Trying to evaluate but evaluator with id {id} is NOT of type {typeof(T)}");
                return ChannelData.Default;
            }

            var data = TryGetData(id);
            data.currentProgression = newProgression;
            var newData = paramEval.Evaluate(data, parameter);
            lastData[id] = newData;
            
            if (record) TryRecord(id, newData);

            return newData;
        }

        public static void OverrideHistory(Guid id, ChannelHistory newHistory)
            => histories[id] = newHistory;

        public static bool TryGetHistory(Guid id, out ChannelHistory history) => histories.TryGetValue(id, out history); 

        private static ChannelData TryGetData(Guid id)
        {
            bool hasHistory = histories.TryGetValue(id, out var history);

            var data = ChannelData.Default;

            if (!lastData.TryGetValue(id, out data) && hasHistory) data = history.LatestData;

            return data;
        }

        private static void TryRecord(Guid id, ChannelData newData)
        {
            if (histories.TryGetValue(id, out var history)) history.AddRecord(newData);
            else histories[id] = new ChannelHistory(newData);
        }
    }
}