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
                var newPoints = new List<Vector2>(history.Records.Count);
                foreach (var record in history.Records)
                    newPoints.Add(new Vector2(record.currentProgression, record.currentDifficulty));

                Debug.Log("Got new points!");
                graph.SetPoints(newPoints);
            }

            graph.Draw();
            graph.Setting(Channel.Data);
            GUILayout.Space(10f);
            GUILayout.Label("Values");
            DrawUILine(Color.white, 2, 0);
            base.OnInspectorGUI();

            OnDraw();
            GUILayout.EndVertical();
            Repaint();
        }

        public static void DrawUILine(Color color, int thickness = 2, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }

        protected virtual void OnDraw()
        {
            
        }
    }
}