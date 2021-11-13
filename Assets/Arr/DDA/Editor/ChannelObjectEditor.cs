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
        
        public override void OnInspectorGUI()
        {
            GUILayout.BeginVertical();
            
            if (graph == null) graph = new DDAGraph(Channel.ChannelName);
            if (evalHandler == null) evalHandler = new EvaluatorEditorHandler();
            
            graph.Draw();
            graph.Setting(Channel.Setting, 0.5f);
            base.OnInspectorGUI();

            var eval = evalHandler.FindEvaluator();
            evalIndex = EditorGUILayout.Popup(label: "Evaluator", evalIndex, 
                Array.ConvertAll(eval, x => x.Name));

            Channel.Evaluator = eval[evalIndex];
            
            
            GUILayout.EndVertical();
        }
        
        
    }
}