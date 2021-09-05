using System;
using Arr.DDA;
using Arr.DDA.Script;
using UnityEditor;
using UnityEngine;

public class ChannelManagerEditorWindow : EditorWindow
{
    private UnityEngine.Object[] metrics;
    private UnityEngine.Object[] channels;

    private int selectedMetricIndex = 0;
    private string metricName = String.Empty;

    private bool createMetricFoldout;

    private GUIStyle centerText;
    
    [MenuItem("ArrDDA/Channel Manager")]
    public static void CreateWindow()
    {
        ChannelManagerEditorWindow window = GetWindow<ChannelManagerEditorWindow>();
        
        window.Show();
    }

    private void OnGUI()
    {
        centerText = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter};

        metrics = Resources.LoadAll(ProjectConst.FOLDER_METRICS, typeof(MetricObject));
        channels = Resources.LoadAll(ProjectConst.FOLDER_CHANNELS, typeof(ChannelObject));

        
        selectedMetricIndex = EditorGUILayout.Popup(label: "Metric: ",selectedMetricIndex, Array.ConvertAll(metrics, x => x.name));
        
        EditorGUILayout.Space(20f);
        
        GUILayout.Label("Channel Tools", centerText);
        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            createMetricFoldout = EditorGUILayout.Foldout(createMetricFoldout, "Create a new Metric");

            if (createMetricFoldout)
            {
                metricName = EditorGUILayout.TextField("Metric Name: ", metricName);

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
        
        Repaint();

    }

    private void CreateMetric(string name)
    {
        MetricObject obj = CreateInstance<MetricObject>();
        
        AssetDatabase.CreateAsset(obj, $"{ProjectConst.FOLDER_RESOURCES}/{ProjectConst.FOLDER_METRICS}/{name}.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        EditorGUIUtility.PingObject(obj);
    }
}
