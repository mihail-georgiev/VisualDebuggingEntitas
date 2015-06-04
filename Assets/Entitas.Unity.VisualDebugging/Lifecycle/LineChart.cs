using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

public class LineChart {

	public List <float>[] entityTimeStampsList;

	public List<string>[] entityLineNodeLabels;
		
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
	private int ticks=10;

	private float lastTime= 0.02f;

	private float min = 0f;
	private float max = 2f;
	private float step = 0.005f;

	private float barFloor ;
	private float chartTop;
	private	float lineWidth;
	private float dataMax;
	
	private EditorWindow window;
	private Editor editor;
	private float windowHeight;

	private Dictionary<String, List<String>> entityEntries;
	
	public LineChart(Editor editor, float windowHeight){
		this.editor = editor;
		this.windowHeight = windowHeight;
	}

	public LineChart(EditorWindow window){
		generateDataDictionary();
		generateChartValues();	
		int linesToDraw = entityEntries.Count;
		float height = linesToDraw*20> Screen.height/4 ? Screen.height/4 : 200;
		this.window = window;
		this.windowHeight = height;
	}

	// napravi go da pokazva polsednoto zapisano vreme, sled tova v dve poleta da se zapishe ot koga do koga da pokaje grafikata (default ot nachaloto do kraq)
	//posle vzemi ramkata ot-do i q razdeli na 10-15 ticka i chartai liniite koito popadat v taq ramka


	public void DrawChart() {	
		
		if (entityTimeStampsList.Length > 0) {
			
			Rect rect = GUILayoutUtility.GetRect(Screen.width, windowHeight);
			chartTop = rect.y + chartBorder_Y;
			lineWidth = (float) (Screen.width - (chartBorder_X * 2)) / ticks;
			barFloor = rect.y + rect.height - chartBorder_Y;

			int c = 0;
			for (int i = 0; i < entityTimeStampsList.Length; i++) {
				if (entityTimeStampsList[i] != null) {
					DrawLine (entityTimeStampsList[i], entityLineNodeLabels[i], colors[c++], "Entity_" + i, i);
					if (c > colors.Count - 1) c = 0;
				}
			}

			Handles.color = axisColor;
	    	drawAxis();
			
			GUIStyle centeredStyle = new GUIStyle();
			centeredStyle.alignment = TextAnchor.UpperCenter;
			centeredStyle.normal.textColor = fontColor;
			
			drawTicksAndLabels (centeredStyle);

			Handles.color = Color.white;
		}
	}

	public void drawControls()
	{
		step = EditorGUILayout.FloatField ("Time Step", step);
		min = EditorGUILayout.Slider ("Start", min, 0, lastTime);
		max = EditorGUILayout.Slider ("End", max, min, lastTime*2);
		ticks = (int)Math.Ceiling((max-min)/step);
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

	void generateDataDictionary()
	{
		String[] lines = File.ReadAllLines("Assets/Logs/2015-06-02_TestLog(14).txt");
		entityEntries = new Dictionary<String, List<String>>();
		
		foreach (string line in lines)
		{
			string[] split = line.Split(':');
			
			if (!entityEntries.ContainsKey(split[0]))
			{
				entityEntries.Add(split[0], new List<string>());
			}
			if(split.Length>1)
				entityEntries[split[0]].Add(split[1]);
		}
		//get time of last entry
		string last = lines[(lines.Length) - 1];
		lastTime = float.Parse(last.Split(new string[]{":"," at "}, StringSplitOptions.None)[2]);
	}
	
	void generateChartValues()
	{	
		List<string>[] nodesData = new List<string>[entityEntries.Count];
		List<float>[] nodesTimeStamps = new List<float>[entityEntries.Count];
		string[] separators = new string[]{" at "};
		int index = 0;
		foreach(KeyValuePair<String, List<String>> pair in entityEntries)
		{
			nodesData[index] = new List<string>();
			nodesTimeStamps[index] = new List<float>();
			foreach(string dataNode in pair.Value)
			{	
				string[] split = dataNode.Split(separators,StringSplitOptions.None);
				nodesData[index].Add(split[0]);
				nodesTimeStamps[index].Add(float.Parse(split[1]));
			}
			index++;
		}
		entityTimeStampsList = nodesTimeStamps;
		entityLineNodeLabels = nodesData;
		
	}

	void drawAxis ()
	{
		Handles.DrawLine (new Vector2 (chartBorder_X, chartTop), new Vector2 (chartBorder_X, barFloor));
		Handles.DrawLine (new Vector2 (chartBorder_X, barFloor), new Vector2 (Screen.width - chartBorder_X, barFloor));
	}

	void drawTicksAndLabels (GUIStyle centeredStyle)
	{
		for (int i = 0; i <= ticks; i++) {
			if (i > 0 && drawTicks)
				Handles.DrawLine (new Vector2 (chartBorder_X + (lineWidth * i), barFloor - 3), new Vector2 (chartBorder_X + (lineWidth * i), barFloor + 3));

			Rect labelRect = new Rect (chartBorder_X + (lineWidth * i) - lineWidth / 2.0f, barFloor + 5, lineWidth, 16);
			GUI.Label (labelRect, "" + (min + i * step), centeredStyle);
		}
	}
}