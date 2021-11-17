using System;
using System.Collections.Generic;
using System.Linq;
using Syrus.Plugins.ChartEditor;
using UnityEditor;
using UnityEngine;

namespace Arr.DDA.Editor
{
    public class DDAGraph
    {
        private string name;
        private float padding, upper, lower, width, offset, slant;
        private Vector2 highest, lowest;
        private List<Vector2> points = new List<Vector2>();
        private bool drawGradient = true;

        private GUIStyle centerText;

        private GUIChartEditor.ChartFunction upperbound;
        private GUIChartEditor.ChartFunction lowerbound;

        public DDAGraph(string name)
        {
            this.name = name;
            centerText = new GUIStyle(GUI.skin.label)
                {alignment = TextAnchor.MiddleCenter, fontSize = 18, fontStyle = FontStyle.Bold};
            AddPoint(new Vector2(0f, 0f));

            upperbound = x => (x * slant) + (upper * width) + offset;
            lowerbound = x => (x * slant) - (lower * width) + offset;

            upper = lower = slant = 1f;
            padding = width = 0.5f;
        }

        public void Draw()
        {

            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.Space(5f);
            GUILayout.Label(name, centerText);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
            var minBound = -0.5f;
            var maxBound = 1.5f;

            var minX = Mathf.Clamp(lowest.x - padding, float.MinValue, minBound);
            var maxX = Mathf.Clamp(highest.x + padding, maxBound, float.MaxValue);
            var minY = Mathf.Clamp(lowest.y - padding, float.MinValue, minBound);
            var maxY = Mathf.Clamp(highest.y + padding, maxBound, float.MaxValue);

            var relativeY = (maxY - minY) / (maxBound - minBound);
            var relativeX = (maxX - minX) / (maxBound - minBound);

            var cellWidth = Mathf.Clamp(Mathf.Ceil(relativeX - 1) / 2f, 0.25f, float.MaxValue);
            var cellHeight = Mathf.Clamp(Mathf.Ceil(relativeY - 1) / 2f, 0.25f, float.MaxValue);

            GUIChartEditor.BeginChart(10, 200, 10, 200, Color.black,
                GUIChartEditorOptions.ChartBounds(minX, maxX, minY, maxY),
                GUIChartEditorOptions.SetOrigin(ChartOrigins.BottomLeft),
                GUIChartEditorOptions.ShowAxes(Color.white),
                GUIChartEditorOptions.ShowGrid(cellWidth, cellHeight, Color.gray, true)
            );

            GUIChartEditor.PushFunction(upperbound, float.MinValue, float.MaxValue, Color.green);
            GUIChartEditor.PushFunction(lowerbound, float.MinValue, float.MaxValue, Color.red);

            DrawGradient();

            GUIChartEditor.PushLineChart(points.ToArray(), Color.cyan);

            if (points.Count > 0)
            {
                var point = points[points.Count - 1];

                GUIChartEditor.PushPoint(point,
                    point.y > upperbound(point.x) ? Color.green :
                    point.y < lowerbound(point.x) ? Color.red : Color.yellow);

                GUIChartEditor.PushValueLabel(point.y, point.x, point.y - (0.1f * relativeY));
            }

            DebugPointer();

            GUIChartEditor.EndChart();

            EditorGUILayout.Space(10f);

            DebugDrawer();

            GUILayout.EndVertical();
        }

        private void DrawGradient()
        {
            if (!drawGradient) return;

            for (int i = 1; i < 11; i++)
            {
                var mult = i;
                var color = new Color(0f, 1f, 0f, 1f - (i / 11f));
                GUIChartEditor.PushFunction(x => upperbound(x) + (0.05f * mult), float.MinValue, float.MaxValue, color);
            }

            for (int i = 1; i < 11; i++)
            {
                var mult = i;
                var color = new Color(1f, 0f, 0f, 1f - (i / 11f));
                GUIChartEditor.PushFunction(x => lowerbound(x) - (0.05f * mult), float.MinValue, float.MaxValue, color);
            }
        }

        private Vector2 pointer;

        private void DebugPointer()
        {
            if (pointer.magnitude > 0) GUIChartEditor.PushPoint(pointer, Color.white);
        }

        private bool drawFoldout;

        private void DebugDrawer()
        {
            drawFoldout = EditorGUILayout.Foldout(drawFoldout, "Debug Drawer");

            if (drawFoldout)
            {
                pointer = EditorGUILayout.Vector2Field("Debug Draw", pointer);

                if (GUILayout.Button("Draw Point")) AddPoint(pointer);

                padding = Mathf.Abs(EditorGUILayout.FloatField("Padding", padding));
                slant = Mathf.Abs(EditorGUILayout.FloatField("Slant", slant));
                upper = Mathf.Abs(EditorGUILayout.FloatField("Upper", upper));
                lower = Mathf.Abs(EditorGUILayout.FloatField("Lower", lower));
                width = Mathf.Abs(EditorGUILayout.FloatField("Width", width));
                offset = Mathf.Abs(EditorGUILayout.FloatField("Offset", offset));
            }
        }

        public void AddPoint(Vector2 point)
        {
            if (point.x > highest.x) highest.x = point.x;
            if (point.y > highest.y) highest.y = point.y;
            if (point.x < lowest.x) lowest.x = point.x;
            if (point.y < lowest.y) lowest.y = point.y;

            points.Add(point);
        }

        public void Setting(float padding, float upper, float lower, float width, float offset, float slant)
        {
            this.padding = padding;
            this.upper = upper;
            this.lower = lower;
            this.width = width;
            this.offset = offset;
            this.slant = slant;
        }

        public void Setting(ChannelSetting setting, float padding)
        {
            this.padding = padding;
            upper = setting.AnxietyThreshold;
            lower = setting.BoredomThreshold;
            width = setting.Width;
            offset = setting.FlowOffset;
            slant = setting.Slant;
        }
    }
}
