using System;
using System.Collections.Generic;
using Arr.DDA.Script;
using UnityEditor;
using UnityEngine;

namespace Arr.DDA.Editor
{
    [CustomEditor(typeof(ChannelObject))]
    public class ChannelObjectEditor : UnityEditor.Editor
    {
        private DDAGraph graph;
        private EvaluatorEditorHandler evalHandler;
        private List<Vector2> points = new List<Vector2>();
        private ChannelObject Channel => target as ChannelObject;

        private int evalIndex;
        private int lastIndex;

        private string lastName = String.Empty;

        private void OnEnable()
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
            float progression = Channel.ProgressionMetric.Get().Value;
            float difficulty = Channel.DifficultyMetric.Get().Value;
            var vec = new Vector2(progression, difficulty);
            graph.AddPoint(vec, true);
            points.Add(vec);
        }

        public override void OnInspectorGUI()
        {
            GUILayout.BeginVertical();
            
            if (graph == null) graph = new DDAGraph(Channel.ChannelName, points);
            if (evalHandler == null) evalHandler = new EvaluatorEditorHandler();

            if (!lastName.Equals(Channel.ChannelName))
            {
                lastName = Channel.ChannelName;
                graph.SetName(lastName);
            }
            
            graph.Draw();
            graph.Setting(Channel.Setting);
            base.OnInspectorGUI();

            EditorGUILayout.Separator();
            if(Channel.Evaluator != null)EditorGUILayout.LabelField($"Evaluator: {Type.GetType(Channel.Evaluator)?.Name}");
            
            var eval = evalHandler.FindEvaluator();
            evalIndex = EditorGUILayout.Popup(label: "Change Evaluator", evalIndex, 
                Array.ConvertAll(eval, x => x.Name));

            if (lastIndex != evalIndex)
            {
                lastIndex = evalIndex;
                Channel.Evaluator = eval[evalIndex].AssemblyQualifiedName;
            }
            
            
            GUILayout.EndVertical();
            Repaint();
        }
        
        
    }
}