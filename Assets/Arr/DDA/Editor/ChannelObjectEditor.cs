using System;
using System.Collections.Generic;
using Arr.DDA.Script;
using UnityEditor;
using UnityEngine;

namespace Arr.DDA.Editor
{
    [CustomEditor(typeof(ChannelObject), true)]
    [CanEditMultipleObjects]
    public class ChannelObjectEditor : UnityEditor.Editor
    {
        private DDAGraph graph;
        private ChannelObject Channel => target as ChannelObject;

        /*private void OnEnable()
        {
            Channel.OnEvaluated += OnEvaluated;
        }

        private void OnDisable()
        {
            Channel.OnEvaluated -= OnEvaluated;
        }

        private void OnEvaluated(float obj)
        {
            if (graph == null) return;
            Debug.Log($"Evaluated to {obj}");
            float progression = Channel.ProgressionMetric.Get().Value;
            float difficulty = Channel.DifficultyMetric.Get().Value;
            var vec = new Vector2(progression, difficulty);
            graph.AddPoint(vec, true);
            points.Add(vec);
        }*/

        public override void OnInspectorGUI()
        {
            GUILayout.BeginVertical();

            if (graph == null) graph = new DDAGraph(Channel.name);
            
            if(DynamicDifficulty.TryGetHistory(Channel, out var history))
            {
                var newPoints = new List<Vector2>(history.records.Count);
                foreach (var record in history.records)
                    newPoints.Add(new Vector2(record.currentProgression, record.currentDifficulty));

                //Debug.Log("Got new points!");
                graph.SetPoints(newPoints);
            }

            graph.Draw();
            var hasData = DynamicDifficulty.TryGetData(Channel, out var data);
            graph.Setting(hasData ? data : Channel.startingData);
            GUILayout.Space(10f);
            base.OnInspectorGUI();
            graph.SetDrawSetting(Channel.drawSetting);

            OnDraw();
            GUILayout.EndVertical();
            Repaint();
        }

        protected virtual void OnDraw()
        {
            
        }
    }
}