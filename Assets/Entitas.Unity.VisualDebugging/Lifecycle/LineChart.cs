using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LineChart {

	public List <float>[] entryTimeStampsList;

	public List <string> dataLabels;

	public List<string>[] dataNodesLabels;
		
	public List <string> axisLabels;

	public float xBorder = 45;

	public float yBorder = 20;

	public string formatString = "{0:0}";

	public string axisFormatString = "{0:0}";
	
	/// <summary>
	/// The colors to draw the bar in. Colors will cycle if there aren't enough to draw each stack. The first
	/// color will be used by a single bar chart.
	/// </summary>
	public List<Color> colors = new List<Color>{Color.magenta, Color.cyan * 2.0f, Color.green};	
	
	/// <summary>
	/// The color of the selected bar.
	/// </summary>
	public Color selectedColor = Colors.PastelOrange;

	
	/// <summary>
	/// The value view mode. Do we see the values, NEVER, ALWAYS or only ON_SELECT.
	/// </summary>
	public ViewMode valueViewMode = ViewMode.ON_SELECT;
	
	
	/// <summary>
	/// The label view mode. Do we see the labels, NEVER, ALWAYS or only ON_SELECT.
	/// </summary>
	public ViewMode labelViewMode = ViewMode.ALWAYS;
	
	/// <summary>
	/// How many horizontal grid lines to draw.
	/// </summary>
	public int gridLines = 0;
	
	/// <summary>
	/// What level should values on the axis be rounded too?
	/// </summary>
	public float axisRounding = 10f;

	public ClickResponder clickResponder;

	public Color axisColor = Color.white;
	

	public Color fontColor = Color.white;
	
	/// <summary>
	/// Additional GUI style drawn in the background. Use this to add a backgroudn image or background box.
	/// </summary>
	public GUIStyle boxStyle;

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
		dataLabels = new List<string>();
		axisLabels = new List<string>();
	}

	public LineChart(EditorWindow window, float windowHeight){
		this.window = window;
		this.windowHeight = windowHeight;
		dataLabels = new List<string>();
		axisLabels = new List<string>();
	}

	public void DrawChart() {	
		
		if (entryTimeStampsList.Length > 0) {
			
			Rect rect = GUILayoutUtility.GetRect(Screen.width, windowHeight);
			barTop = rect.y + yBorder;
			lineWidth = (float) (Screen.width - (xBorder * 2)) / ticks;
			barFloor = rect.y + rect.height - yBorder;
//			dataMax = 0.0f;
//			foreach (List<float> row in entryTimeStampsList) {
//				if (row != null && row.Count > 0) {
//					if (row.Max() > dataMax) {
//						dataMax = row.Max();
//					}
//				}
//			}
//			
//			// Clean up variables
//			if (dataMax % axisRounding != 0){
//				dataMax = dataMax + axisRounding - (dataMax % axisRounding);
//			}
//			
//			// Text to Left
//			GUIStyle labelTextStyle = new GUIStyle();
//	    	labelTextStyle.alignment = TextAnchor.UpperRight;
//			labelTextStyle.normal.textColor = fontColor;
//			
//			// Draw grid lines
//			if (gridLines > 0) { 
//				Handles.color = Color.red;
//				float lineSpacing = (barFloor - barTop) / (gridLines + 1);
//				for (int i = 0; i <= gridLines; i++) {
//					if (i > 0) Handles.DrawLine(new Vector2(xBorder, barTop + (lineSpacing * i)), new Vector2(Screen.width - xBorder, barTop + (lineSpacing * i)));	 
//					if ((dataMax * (1 - ((lineSpacing * i) / (barFloor - barTop)))) > 0)
//						GUI.Label(new Rect(0, barTop + (lineSpacing * i) - 8, xBorder - 2, 50), ""+i , labelTextStyle); //string.Format(axisFormatString, (dataMax * (1 - ((lineSpacing * i) / (barFloor - barTop)))))
//				}
//				Handles.color = Color.green;
//			}
			
			int c = 0;
			for (int i = 0; i < entryTimeStampsList.Length; i++) {
				if (entryTimeStampsList[i] != null) {
					DrawLine (entryTimeStampsList[i], dataNodesLabels[i], colors[c++], i < dataLabels.Count ? dataLabels[i] : "Entity_" + i, i);
					if (c > colors.Count - 1) c = 0;
				}
			}

			// Draw Axis
			Handles.color = axisColor;
	    	Handles.DrawLine(new Vector2(xBorder, barTop), new Vector2(xBorder, barFloor));
			Handles.DrawLine(new Vector2(xBorder, barFloor), new Vector2(Screen.width - xBorder, barFloor));
			
			GUIStyle centeredStyle = new GUIStyle();
			centeredStyle.alignment = TextAnchor.UpperCenter;
			centeredStyle.normal.textColor = fontColor;
			
			// Draw ticks and labels
			for (int i = 0; i <= ticks; i++) {
				if (i > 0 && drawTicks) Handles.DrawLine(new Vector2(xBorder + (lineWidth * i), barFloor - 3), new Vector2(xBorder + (lineWidth * i), barFloor + 3));
				if (i < axisLabels.Count) {
					Rect labelRect = new Rect(xBorder + (lineWidth * i) - lineWidth / 2.0f, barFloor + 5, lineWidth, 16);			
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
			float end = xBorder + ((timeStampList[i]-min)/(max-min))*(Screen.width - xBorder);
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
				if (valueViewMode == ViewMode.ON_SELECT) {
					selectRect.y -= 16; selectRect.width += 50; selectRect.x -= 25;
					GUI.Label(selectRect, string.Format(formatString, dataNodesLabels[i]), centeredStyle);				
				}
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