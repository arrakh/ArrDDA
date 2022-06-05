/*
using System;
using Arr.DDA.Script;
using Syrus.Plugins.ChartEditor;
using UnityEditor;
using UnityEngine;

namespace Arr.DDA.Editor
{
    public class ChannelManagerEditorWindow : EditorWindow
    {
        private MetricObject[] metrics;
        private UnityEngine.Object[] channels;

        private string metricName = String.Empty;
        private string channelName = String.Empty;

        private bool createMetricFoldout;
        private bool createChannelFoldout;

        private GUIStyle centerText;

        private EvaluatorEditorHandler evalHandler;
        private DDAGraph graph;
    
        [MenuItem("ArrDDA/Channel Manager")]
        public static void CreateWindow()
        {
            ChannelManagerEditorWindow window = GetWindow<ChannelManagerEditorWindow>();
        
            window.Show();
        }

        private int activeChannelIndex;
        private ChannelObject activeChannel = null;
        private Vector2 scrollPosition = Vector2.zero;
        private void OnGUI()
        {
            centerText = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter, 
                fontStyle = FontStyle.Bold,
                fontSize = 24
            };
            
            if (evalHandler == null) evalHandler = new EvaluatorEditorHandler();
            if (graph == null) graph = new DDAGraph("Testing");

            metrics = Resources.LoadAll<MetricObject>(ProjectConst.FOLDER_METRICS);
            channels = Resources.LoadAll(ProjectConst.FOLDER_CHANNELS, typeof(ChannelObject));

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(10f);
            GUILayout.Label("DDA Manager", centerText);
            GUILayout.Space(10f);

            if (channels.Length > 0)
            {
                activeChannelIndex = EditorGUILayout.Popup(label: "Challenge Metric", activeChannelIndex, 
                    Array.ConvertAll(channels, x => x.name));

                activeChannel = channels[activeChannelIndex] as ChannelObject;
                
                if(activeChannel != null) 
                    OnActiveChannelChanged(activeChannel);
            }
            else
            {
                EditorGUILayout.HelpBox("No Channel detected. Create a new channel in the Channel Tools section below", MessageType.Warning);
            }

            GUILayout.Space(20f);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.EndVertical();
        
            GUILayout.Label("Channel Tools", centerText);
            GUILayout.Space(5f);
            CreateMetricFoldout();
            CreateChannelFoldout();
            EditorGUILayout.EndScrollView();

            Repaint();

        }

        private void OnActiveChannelChanged(ChannelObject channel)
        {
            graph.Draw();
            graph.Setting(channel.Setting);
        }

        private void CreateMetricFoldout()
        {
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                createMetricFoldout = EditorGUILayout.Foldout(createMetricFoldout, "Create New Metric");

                if (createMetricFoldout)
                {
                    metricName = EditorGUILayout.TextField("Metric Name", metricName);

                    using (new EditorGUI.DisabledScope(String.IsNullOrEmpty(metricName)))
                    {
                        if (GUILayout.Button("Create Metric"))
                        {
                            CreateMetric(metricName);
                            metricName = String.Empty;
                            GUIUtility.keyboardControl = 0;
                        }
                    }
                }
            }

        }

        private void CreateMetric(string name)
        {
            MetricObject obj = CreateInstance<MetricObject>();
        
            AssetDatabase.CreateAsset(obj, $"{ProjectConst.FOLDER_RESOURCES}/{ProjectConst.FOLDER_METRICS}/{name}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();    
        
            EditorGUIUtility.PingObject(obj);
        }

    


        private int DifficultyIndex, progressionIndex, evalIndex;
        private ChannelSetting channelSetting = new ChannelSetting();
        private bool channelSettingFoldout;
        private DDAGraph createChannelGraph;
    
        private void CreateChannelFoldout()
        {
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                createChannelFoldout = EditorGUILayout.Foldout(createChannelFoldout, "Create New Channel");

                if (createChannelFoldout)
                {
                    if (createChannelGraph == null) createChannelGraph = new DDAGraph("New Graph");
                    createChannelGraph.Setting(channelSetting);
                    createChannelGraph.Draw();
                    
                    var eval = evalHandler.FindEvaluator();
                
                    channelName = EditorGUILayout.TextField("Channel Name", channelName);

                    if (metrics.Length > 0)
                    {
                        DifficultyIndex = EditorGUILayout.Popup(label: "Difficulty Metric", DifficultyIndex, 
                            Array.ConvertAll(metrics, x => x.name));
                        progressionIndex = EditorGUILayout.Popup(label: "Progression Metric", progressionIndex, 
                            Array.ConvertAll(metrics, x => x.name)); 
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("No Metric detected. Create a new metric first!", MessageType.Warning);
                    }

                    evalIndex = EditorGUILayout.Popup(label: "Evaluator", evalIndex, 
                        Array.ConvertAll(eval, x => x.Name));

                    using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                    {
                        channelSettingFoldout = EditorGUILayout.Foldout(channelSettingFoldout, "Channel Setting");

                        if (channelSettingFoldout)
                        {
                            channelSetting.AnxietyThreshold = EditorGUILayout.FloatField("Anxiety Threshold", channelSetting.AnxietyThreshold);
                            channelSetting.BoredomThreshold = EditorGUILayout.FloatField("Boredom Threshold", channelSetting.BoredomThreshold);
                            channelSetting.FlowOffset = EditorGUILayout.FloatField("Flow Offset", channelSetting.FlowOffset);
                            channelSetting.Width = EditorGUILayout.FloatField("Width", channelSetting.Width);
                            channelSetting.Slant = EditorGUILayout.FloatField("Slant", channelSetting.Slant);
                        }
                    }

                    using (new EditorGUI.DisabledScope(String.IsNullOrEmpty(channelName) && metrics.Length > 0))
                    {
                        if (GUILayout.Button("Create Channel"))
                        {
                            Type evalType = evalHandler.GetEvaluatorType(evalIndex);
                        
                            CreateChannel(channelName, evalType, metrics[DifficultyIndex], metrics[progressionIndex], channelSetting);
                            channelName = String.Empty;
                            GUIUtility.keyboardControl = 0;
                        }
                    }
                }
            }

        }
    
        private void CreateChannel(string name, Type evaluator, MetricObject difficulty, MetricObject progression, ChannelSetting setting)
        {
            ChannelObject obj = CreateInstance<ChannelObject>();

            obj.ChannelName = name;
            obj.Setting = setting;
            obj.DifficultyMetric = difficulty;
            obj.ProgressionMetric = progression;
            obj.Evaluator = evaluator.AssemblyQualifiedName;
        
            AssetDatabase.CreateAsset(obj, $"{ProjectConst.FOLDER_RESOURCES}/{ProjectConst.FOLDER_CHANNELS}/{name}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();    
        
            EditorGUIUtility.PingObject(obj);
        }


    }
}
*/
