using System;
using System.Collections.Generic;
using Arr.DDA.Script;
using UnityEditor;
using UnityEngine;

namespace Arr.DDA.Editor
{
    [CustomEditor(typeof(ChannelObject), true)]
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

        private void OnSceneGUI()
        {
            OnInspectorGUI();
        }
        
        public override void OnInspectorGUI()
        {
            GUILayout.BeginVertical();

            var points = new List<Vector2>();

            if(DynamicDifficulty.TryGetHistory(Channel.Id, out var history))
            {
                var newPoints = new List<Vector2>(history.Records.Count);
                foreach (var record in history.Records)
                    newPoints.Add(new Vector2(record.currentDifficulty, record.currentProgression));

                points = newPoints;
            }
            
            if (graph == null) graph = new DDAGraph(Channel.name, points);

            graph.Draw();
            graph.Setting(Channel.Data);
            GUILayout.Label($"DEBUG ID: {Channel.Id}");
            base.OnInspectorGUI();

            OnDraw();
            GUILayout.EndVertical();
            Repaint();
        }

        protected virtual void OnDraw()
        {
            
        }
    }
}