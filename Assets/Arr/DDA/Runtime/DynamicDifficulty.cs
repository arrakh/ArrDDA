using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arr.DDA.Script
{
    public static class DynamicDifficulty
    {
        private static ChannelController controller;
        private static bool initialized = false;

        [RuntimeInitializeOnLoadMethod]
        private static void Start()
        {
            controller = new GameObject("DynamicDifficultyAdjustment").AddComponent<ChannelController>();
            controller.gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;

            controller.OnDestroyed += OnControllerDestroyed;
            initialized = true;
        }

        private static void OnControllerDestroyed()
        {
            controller.OnDestroyed -= OnControllerDestroyed;
            initialized = false;
        }

        public static void Initialize(ChannelObject channel, ChannelData startingData, IEvaluator evaluator)
        {
            if (initialized) controller.Initialize(channel, startingData, evaluator);
        }

        public static void Initialize(ChannelObject channel, ChannelHistory history, IEvaluator evaluator)
        {
            if (initialized) controller.Initialize(channel, history, evaluator);
        }

        public static ChannelData Evaluate(ChannelObject channel, float newProgression, bool record, bool isDelta)
        {
            if (initialized) return controller.Evaluate(channel, newProgression, record, isDelta);
            
            Debug.LogError("Trying to evaluate before DynamicDifficulty is Initialized!");
            return ChannelData.Default;
        }

        public static ChannelData Evaluate<T>(ChannelObject channel, float newProgression, T parameter, bool record, bool isDelta)
        {
            if (initialized) return controller.Evaluate(channel, newProgression, parameter, record, isDelta);
            
            Debug.LogError("Trying to evaluate before DynamicDifficulty is Initialized!");
            return ChannelData.Default;
        }

        public static bool TryGetHistory(ChannelObject id, out ChannelHistory history)
        {
            history = new ChannelHistory();
            if (initialized) return controller.TryGetHistory(id, out history);
            return false;
        }

        public static bool TryGetData(ChannelObject channel, out ChannelData data)
        {
            if (initialized) return controller.TryGetData(channel, out data);
            data = ChannelData.Default;
            return false;
        }
    }
}