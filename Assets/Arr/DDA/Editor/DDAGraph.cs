using System;
using System.Collections.Generic;
using System.Linq;
using Arr.DDA.Script;
using Syrus.Plugins.ChartEditor;
using UnityEditor;
using UnityEngine;

namespace Arr.DDA.Editor
{
    public class DDAGraph
    {
        private string name;
        private float upper, lower, width, offset, slant;
        private Vector2 highest, lowest, relativeScale;
        private List<Vector2> points = new List<Vector2>();

        private GUIStyle centerText;

        private GUIChartEditor.ChartFunction upperboundFunction;
        private GUIChartEditor.ChartFunction lowerboundFunction;
        private ChannelDrawSetting drawSetting = ChannelDrawSetting.Default;

        public DDAGraph(string name, List<Vector2> graphPoints = null)
        {
            this.name = name;

            if (graphPoints == null) points = new List<Vector2>();
            else foreach (var point in graphPoints)
                    AddPoint(point);
            
            centerText = new GUIStyle(GUI.skin.label)
                {alignment = TextAnchor.MiddleCenter, fontSize = 18, fontStyle = FontStyle.Bold};

            upperboundFunction = x => (x * slant) + (upper * width) + offset;
            lowerboundFunction = x => (x * slant) - (lower * width) + offset;

            upper = lower = slant = 1f;
            width = 0.5f;
        }

        public void SetName(string newName) => name = newName;

        public void Draw()
        {

            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.Space(5f);
            GUILayout.Label(name, centerText);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            //Substitute bound func if higher/lower so bound is always visible
            var funcHigh = upperboundFunction.Invoke(highest.x + drawSetting.graphSize);
            var funcLow = lowerboundFunction.Invoke(lowest.x);
            var highestY = funcHigh > highest.y ? funcHigh : highest.y;
            var lowestY = funcLow < lowest.y ? funcLow : lowest.y;

            //Find padded min and max based on lowest or highest recorded position
            var minX = Mathf.Clamp(lowest.x - drawSetting.padding, float.MinValue, 0f) - drawSetting.zoom.x * drawSetting.cellIncrements;
            var maxX = Mathf.Clamp(highest.x + drawSetting.padding, drawSetting.graphSize, float.MaxValue) * drawSetting.zoom.x + drawSetting.zoom.x * drawSetting.cellIncrements;
            var minY = Mathf.Clamp(lowestY - drawSetting.padding, float.MinValue, 0f) - drawSetting.zoom.y * drawSetting.cellIncrements;
            var maxY = Mathf.Clamp(highestY + drawSetting.padding, drawSetting.graphSize, float.MaxValue) * drawSetting.zoom.y + drawSetting.zoom.y * drawSetting.cellIncrements;

            relativeScale.x = (maxX - minX) / drawSetting.graphSize;
            relativeScale.y = (maxY - minY) / drawSetting.graphSize;
 
            var cellIncrement = 1 / drawSetting.cellIncrements;

            var cellXIncrement = Mathf.Clamp(Mathf.Ceil(relativeScale.x - 1) / cellIncrement, 0.01f, float.MaxValue);
            var cellYIncrement = Mathf.Clamp(Mathf.Ceil(relativeScale.y - 1) / cellIncrement, 0.01f, float.MaxValue);
            
            GUIChartEditor.BeginChart(10, 10, 10, drawSetting.height, Color.black,
                GUIChartEditorOptions.ChartBounds(minX, maxX, minY, maxY),
                GUIChartEditorOptions.SetOrigin(ChartOrigins.BottomLeft),
                GUIChartEditorOptions.ShowAxes(Color.white),
                GUIChartEditorOptions.ShowGrid(cellXIncrement, cellYIncrement, new Color(0.2f, 0.2f, 0.2f, 1f), true)
            );

            GUIChartEditor.PushFunction(upperboundFunction, float.MinValue, float.MaxValue, Color.green);
            GUIChartEditor.PushFunction(lowerboundFunction, float.MinValue, float.MaxValue, Color.red);

            DrawGradient();

            DrawLines();
            
            DebugPointer();
            GUIChartEditor.EndChart();

            GUILayout.EndVertical();
        }

        private void DrawGradient()
        {
            if (!drawSetting.drawLineGradient) return;

            for (int i = 1; i < drawSetting.lineGradientAmount; i++)
            {
                var mult = i;
                var color = new Color(0f, 1f, 0f, 1f - ((float)i / drawSetting.lineGradientAmount));
                GUIChartEditor.PushFunction(x => upperboundFunction(x) + (drawSetting.lineGradientDistance * mult * relativeScale.y), float.MinValue, float.MaxValue, color);
            }

            for (int i = 1; i < drawSetting.lineGradientAmount; i++)
            {
                var mult = i;
                var color = new Color(1f, 0f, 0f, 1f - ((float)i / drawSetting.lineGradientAmount));
                GUIChartEditor.PushFunction(x => lowerboundFunction(x) - (drawSetting.lineGradientDistance * mult * relativeScale.y), float.MinValue, float.MaxValue, color);
            }
        }
        
        private void DrawLines()
        {
            if (points.Count <= 0) return;
            
            GUIChartEditor.PushLineChart(points.ToArray(), Color.cyan);


            if (drawSetting.drawPoints)
            {
                foreach (var p in points)
                {
                    GUIChartEditor.PushPoint(p, Color.grey);
                }
            }
            
            var point = points[points.Count - 1];

            GUIChartEditor.PushPoint(point,
                point.y > upperboundFunction(point.x) ? Color.green :
                point.y < lowerboundFunction(point.x) ? Color.red : Color.yellow);

            GUIChartEditor.PushValueLabel(point.y, point.x, point.y - (0.1f * relativeScale.y));
        }
        
        private Vector2 pointer;
        private void DebugPointer()
        {
            if (pointer.magnitude > 0) GUIChartEditor.PushPoint(pointer, Color.white);
            if (pointer.x > highest.x) highest.x = pointer.x;
            if (pointer.y > highest.y) highest.y = pointer.y;
            if (pointer.x < lowest.x) lowest.x = pointer.x;
            if (pointer.y < lowest.y) lowest.y = pointer.y;
        }

        public void AddPoint(Vector2 point, bool redraw = false)
        {
            if (point.x > highest.x) highest.x = point.x;
            if (point.y > highest.y) highest.y = point.y;
            if (point.x < lowest.x) lowest.x = point.x;
            if (point.y < lowest.y) lowest.y = point.y;

            points.Add(point);
            if (redraw) DrawLines();
        }

        public void SetPoints(List<Vector2> points)
        {
            this.points = points;

            foreach (var point in points)
            {
                if (point.x > highest.x) highest.x = point.x;
                if (point.y > highest.y) highest.y = point.y;
                if (point.x < lowest.x) lowest.x = point.x;
                if (point.y < lowest.y) lowest.y = point.y;
            }
        }

        public void Setting(float upper, float lower, float width, float offset, float slant)
        {
            this.upper = upper;
            this.lower = lower;
            this.width = width;
            this.offset = offset;
            this.slant = slant;
        }

        public void Setting(ChannelData setting)
        {
            upper = setting.anxietyThreshold;
            lower = setting.boredomThreshold;
            width = setting.width;
            offset = setting.flowOffset;
            slant = setting.slant;
            pointer.x = setting.currentProgression;
            pointer.y = setting.currentDifficulty;
        }

        public void SetDrawSetting(ChannelDrawSetting setting) => drawSetting = setting;
    }
}
