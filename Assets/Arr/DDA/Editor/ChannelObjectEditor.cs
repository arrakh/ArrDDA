using System;
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
        private ChannelObject Channel => target as ChannelObject;

        private int evalIndex;
        private int lastIndex;

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
            graph.AddPoint(new Vector2(progression, difficulty));
        }

        public override void OnInspectorGUI()
        {
            GUILayout.BeginVertical();
            
            if (graph == null) graph = new DDAGraph(Channel.ChannelName);
            if (evalHandler == null) evalHandler = new EvaluatorEditorHandler();
            
            graph.Draw();
            graph.Setting(Channel.Setting, 0.5f);
            base.OnInspectorGUI();

            EditorGUILayout.Separator();
            EditorGUILayout.LabelField($"Evaluator: {Type.GetType(Channel.Evaluator)?.Name}");
            
            var eval = evalHandler.FindEvaluator();
            evalIndex = EditorGUILayout.Popup(label: "Change Evaluator", evalIndex, 
                Array.ConvertAll(eval, x => x.Name));

            if (lastIndex != evalIndex)
            {
                lastIndex = evalIndex;
                Channel.Evaluator = eval[evalIndex].AssemblyQualifiedName;
            }
            
            
            GUILayout.EndVertical();
        }
        
        
    }
}