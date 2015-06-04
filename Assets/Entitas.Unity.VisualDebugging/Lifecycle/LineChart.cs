using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LineChart {

	public List <float>[] entityTimeStampsList;

	public List<string>[] entityLineNodeLabels;
		
	public List <string> axisLabels;

	public float chartBorder_X = 45;

	public float chartBorder_Y = 20;
	

	/// <summary>
	/// The colors to draw the bar in. Colors will cycle if there aren't enough to draw each stack. The first
	/// color will be used by a single bar chart.
	/// </summary>
	public List<Color> colors = new List<Color>{Color.magenta, Color.cyan * 2.0f, Color.green};	

	public ClickResponder clickResponder;

	public Color axisColor = Color.white;
	

	public Color fontColor = Color.white;

	/// <summary>
	/// Size of the pips that occur at each data point. Use 1 or smaller to draw straight lines.
	/// </summary>
	public float pipRadius = 3.0f;

	public bool drawTicks = true;

	//tickCount
	public int ticks=10;

	public float lastTime= 0.02f;

	public float min,max;

	private float barFloor ;
	private float barTop;
	private	float lineWidth;
	private float dataMax;
	
	private EditorWindow window;
	private Editor editor;
	private float windowHeight;
	
	public LineChart(Editor editor, float windowHeight){
		this.editor = editor;
		this.windowHeight = windowHeight;
		axisLabels = new List<string>();
	}

	public LineChart(EditorWindow window, float windowHeight){
		this.window = window;
		this.windowHeight = windowHeight;
		axisLabels = new List<string>();
	}

	public void DrawChart() {	
		
		if (entityTimeStampsList.Length > 0) {
			
			Rect rect = GUILayoutUtility.GetRect(Screen.width, windowHeight);
			barTop = rect.y + chartBorder_Y;
			lineWidth = (float) (Screen.width - (chartBorder_X * 2)) / ticks;
			barFloor = rect.y + rect.height - chartBorder_Y;

			int c = 0;
			for (int i = 0; i < entityTimeStampsList.Length; i++) {
				if (entityTimeStampsList[i] != null) {
					DrawLine (entityTimeStampsList[i], entityLineNodeLabels[i], colors[c++], "Entity_" + i, i);
					if (c > colors.Count - 1) c = 0;
				}
			}

			// Draw Axis
			Handles.color = axisColor;
	    	Handles.DrawLine(new Vector2(chartBorder_X, barTop), new Vector2(chartBorder_X, barFloor));
			Handles.DrawLine(new Vector2(chartBorder_X, barFloor), new Vector2(Screen.width - chartBorder_X, barFloor));
			
			GUIStyle centeredStyle = new GUIStyle();
			centeredStyle.alignment = TextAnchor.UpperCenter;
			centeredStyle.normal.textColor = fontColor;
			
			// Draw ticks and labels
			for (int i = 0; i <= ticks; i++) {
				if (i > 0 && drawTicks) Handles.DrawLine(new Vector2(chartBorder_X + (lineWidth * i), barFloor - 3), new Vector2(chartBorder_X + (lineWidth * i), barFloor + 3));
				if (i < axisLabels.Count) {
					Rect labelRect = new Rect(chartBorder_X + (lineWidth * i) - lineWidth / 2.0f, barFloor + 5, lineWidth, 16);			
					GUI.Label(labelRect, axisLabels[i], centeredStyle);				
				}
			}

			Handles.color = Color.white;
		}
	}
	
	private void DrawLine(List<float> timeStampList, List<string> dataNodesLabels, Color color, string label, int index) {
		Vector2 previousLine = Vector2.zero;
		Vector2 newLine;
		Handles.color = color;
		
		for (int i = 0; i < timeStampList.Count; i++) {
			float end = chartBorder_X + ((timeStampList[i]-min)/(max-min))*(Screen.width - chartBorder_X);
			newLine = new Vector2( (end), -(index+1)*20+barFloor);
			if(timeStampList[i]<min || timeStampList[i]>max*1.1f){
				previousLine = newLine;
				continue;
			}
	
			if (i > 0) {	
				Handles.DrawAAPolyLine(previousLine, newLine);
			}
			previousLine = newLine;
		
			Rect selectRect = new Rect((previousLine - (Vector2.up * 0.5f)).x - pipRadius * 3, (previousLine - (Vector2.up * 0.5f)).y - pipRadius * 3, pipRadius * 6, pipRadius * 6);
			if (selectRect.Contains(Event.current.mousePosition)) {
				GUIStyle centeredStyle = new GUIStyle();
				centeredStyle.alignment = TextAnchor.UpperCenter;
				centeredStyle.normal.textColor = fontColor;
				Handles.DrawSolidDisc(previousLine - (Vector2.up * 0.5f), Vector3.forward, pipRadius * 1.5f);

				// Listen for click
				if (clickResponder != null &&  Event.current.button == 0 && Event.current.isMouse && Event.current.type == EventType.MouseDown) {
					clickResponder.Click(dataNodesLabels[i], timeStampList[i], index);
				}
				if (window != null) window.Repaint();
				if (editor != null) editor.Repaint();
			} else {
				Handles.DrawSolidDisc(previousLine - (Vector2.up * 0.5f), Vector3.forward, pipRadius);
			}
		}	
		if (label != null) {
			GUIStyle colorStyle = new GUIStyle();
			colorStyle.normal.textColor = color;
			Rect labelRect = new Rect(1, previousLine.y - 8, 100, 16);			
			GUI.Label(labelRect, label, colorStyle);				
		}
	}
}