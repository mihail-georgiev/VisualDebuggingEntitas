using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

public class LineChart {
	EditorWindow window;
	float windowHeight;

	float chartBorderHorizontal = 60;
	float chartBorderVertikal = 20;
	int chartSections = 10;
	float sectionWidth;
	float chartFloor ;
	float chartTop;

	float timeFrameFrom = 0f;
	float timeFrameTo = 100f;
	float timeStep;

	Dictionary<String, List<String>> entityEntries;
	List <float>[] entityNodesTimeStamps;
	List<string>[] entityLineNodeLabels;
	float lastRecordedTime;
		
	List<Color> colors = new List<Color>{Color.magenta, Color.cyan * 2.0f, Color.green};	

	Color axisColor = Color.green;
	Color fontColor = Color.yellow;
	
	float pipRadius = 3.0f;

	public ClickResponder clickResponder;

	public LineChart(EditorWindow window)
	{
		readEntriesDataFromFile();
		generateChartData();	
		int linesToDraw = entityEntries.Count;
		float height = linesToDraw*20 +100;
		this.window = window;
		this.windowHeight = height;
	}

	public void drawControls()
	{
		EditorGUILayout.HelpBox("Last Entry recorded at: " + lastRecordedTime + " ms", MessageType.None);
		timeFrameFrom = EditorGUILayout.Slider ("Time Frame Begin (ms):", timeFrameFrom, 0, lastRecordedTime);
		timeFrameTo = EditorGUILayout.Slider ("Time Frame End (ms):", timeFrameTo, timeFrameFrom, lastRecordedTime+2);
		chartSections = EditorGUILayout.IntField ("Sections", chartSections);
		timeStep = (timeFrameTo-timeFrameFrom)/chartSections;
	}

	public void DrawChart()
	{	
		if(entityNodesTimeStamps.Length > 0)
		{
			creteChartRect ();
			drawEntityLines ();
	    	drawAxis();
			drawSectionsAndLabels();
		}
	}

	void creteChartRect()
	{
		Rect rect = GUILayoutUtility.GetRect (Screen.width, windowHeight);
		chartTop = rect.y + chartBorderVertikal;
		sectionWidth = (float)(Screen.width - (chartBorderHorizontal * 2)) / chartSections;
		chartFloor = rect.y + rect.height - chartBorderVertikal;
	}

	void drawEntityLines()
	{
		int c = 0;
		for (int i = 0; i < entityNodesTimeStamps.Length; i++)
		{
			if (entityNodesTimeStamps [i] != null)
			{
				drawLine (entityNodesTimeStamps [i], entityLineNodeLabels [i], colors [c++], "Entity_" + i, i);
				if (c > colors.Count - 1)
					c = 0;
			}
		}
	}

	void drawAxis ()
	{		
		Handles.color = axisColor;
		Handles.DrawLine (new Vector2 (chartBorderHorizontal, chartTop), new Vector2 (chartBorderHorizontal, chartFloor));
		Handles.DrawLine (new Vector2 (chartBorderHorizontal, chartFloor), new Vector2 (Screen.width - chartBorderHorizontal, chartFloor));
	}

	void drawSectionsAndLabels()
	{
		GUIStyle centeredStyle = new GUIStyle();
		centeredStyle.alignment = TextAnchor.UpperCenter;
		centeredStyle.normal.textColor = fontColor;
		
		for (int i = 0; i <= chartSections; i++)
		{
			if (i > 0)
				Handles.DrawLine (new Vector2 (chartBorderHorizontal + (sectionWidth * i), chartFloor - 3), new Vector2 (chartBorderHorizontal + (sectionWidth * i), chartFloor + 3));
			
			Rect labelRect = new Rect (chartBorderHorizontal + (sectionWidth * i) - sectionWidth / 2.0f, chartFloor + 5, sectionWidth, 16);
			GUI.Label(labelRect, "" + (timeFrameFrom + i * timeStep) + "ms", centeredStyle);
		}
	}

	void drawLine(List<float> timeStampList, List<string> dataNodesLabels, Color color, string label, int index)
	{
		Vector2 previousLine = Vector2.zero;
		Vector2 newLine;
		Handles.color = color;
		
		for (int i = 0; i < timeStampList.Count; i++)
		{
			float lineX = chartBorderHorizontal + ((timeStampList[i]-timeFrameFrom)/(timeFrameTo-timeFrameFrom))*(Screen.width - chartBorderHorizontal);
			float lineY = -(index+1)*20+chartFloor;
			newLine = new Vector2( lineX, lineY);
			if(timeStampList[i] < timeFrameFrom)
			{
				previousLine = newLine;
				continue;
			}
	
			if (i > 0) 
			{
				previousLine.x = previousLine.x < chartBorderHorizontal ? chartBorderHorizontal: previousLine.x;
				Handles.DrawAAPolyLine(previousLine, newLine);
			}
			previousLine = newLine;
		
			Rect selectRect = new Rect((previousLine - (Vector2.up * 0.5f)).x - pipRadius * 3, (previousLine - (Vector2.up * 0.5f)).y - pipRadius * 3, pipRadius * 6, pipRadius * 6);
			if (selectRect.Contains(Event.current.mousePosition))
			{
				GUIStyle centeredStyle = new GUIStyle();
				centeredStyle.alignment = TextAnchor.UpperCenter;
				centeredStyle.normal.textColor = fontColor;
				Handles.DrawSolidDisc(previousLine - (Vector2.up * 0.5f), Vector3.forward, pipRadius * 1.5f);

				// Listen for click
				if (clickResponder != null &&  Event.current.button == 0 && Event.current.isMouse && Event.current.type == EventType.MouseDown) {
					clickResponder.Click(dataNodesLabels[i], timeStampList[i], index);
				}
				if (window != null) window.Repaint();
			} else {
				Handles.DrawSolidDisc(previousLine - (Vector2.up * 0.5f), Vector3.forward, pipRadius);
			}
		}	
		if (label != null)
		{
			drawEntityLineLabel(color, label, previousLine.y);
		}
	}

	static void drawEntityLineLabel (Color color, string label, float lineHeight)
	{
		GUIStyle colorStyle = new GUIStyle ();
		colorStyle.normal.textColor = color;
		Rect labelRect = new Rect (1, lineHeight - 8, 100, 16);
		GUI.Label (labelRect, label, colorStyle);
	}

	void readEntriesDataFromFile()
	{	
		string[] allLogFiles = Directory.GetFiles("Assets/Logs/", "*.txt");
		string lastLogFilePath = allLogFiles[allLogFiles.Length-1];
		String[] lines = File.ReadAllLines(lastLogFilePath);
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

		string lastEntry = lines[(lines.Length) - 1];
		lastRecordedTime = float.Parse(lastEntry.Split(new string[]{":"," at "}, StringSplitOptions.None)[2]);
	}
	
	void generateChartData()
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
		entityNodesTimeStamps = nodesTimeStamps;
		entityLineNodeLabels = nodesData;
	}
}