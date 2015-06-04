using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;


public class DiagramWindow : EditorWindow , ClickResponder {
	

	LineChart entityDiagram;
	Dictionary<String, List<String>> entityEntries;

 	GUIStyle boxStyle;

	Vector2 scrollPos = Vector2.zero;
	string scrollText = "";


	float step = 0.001f;
	float max,min;
	float lastTimeStamp;
	int ticks ;

	bool showDiagram = false;
    [MenuItem ("Entitas/ShowDiagram")]
    static void Init () {
        // Get existing open window or if none, make a new one:
		DiagramWindow window = (DiagramWindow) EditorWindow.GetWindow (typeof (DiagramWindow));
		
		// Listen for updates - use this if you want real time graphs
		// if (EditorApplication.update != window.EditorUpdate) EditorApplication.update += window.EditorUpdate;
		
		// Listen for moves
		window.wantsMouseMove = true;
	
		// Create a style which loads a background image
		window.boxStyle = new GUIStyle();
		window.boxStyle.normal.background = (Texture2D) Resources.Load("Border", typeof(Texture2D));
		window.boxStyle.border = new RectOffset(32,32,32,32);
		window.boxStyle.margin = new RectOffset(4,4,4,4);

		window.Focus();
    }
	
   void OnGUI () {
		if(entityEntries == null){
			generateDataDictionary();
		}

		int linesToDraw = entityEntries.Count;
		float height = linesToDraw*20> Screen.height/4 ? Screen.height/4 : 200;
		if (entityDiagram == null) {
				entityDiagram = new LineChart(this, height);

			}
		generateChartValues();	
	
		step = EditorGUILayout.FloatField ("Step", step);
		min = EditorGUILayout.Slider ("Slider", min, 0, lastTimeStamp);
		max = EditorGUILayout.Slider ("Slider1", max, min, lastTimeStamp*2);
		ticks = (int)Math.Ceiling((max-min)/step);

		entityDiagram.ticks = ticks;
		entityDiagram.lastTime = lastTimeStamp;
		entityDiagram.min = min;
		entityDiagram.max = max;
		entityDiagram.clickResponder = this;
		List<string> axisLabels = new List<string>();
		for(int i=0; i<=ticks;i++)
		{
			axisLabels.Add("" + (min + i * step));
		}
		entityDiagram.axisLabels = axisLabels;

		if(GUILayout.Button("Draw Chart"))
		{
			if(showDiagram)
				showDiagram = false;
			else
				showDiagram = true;
		}

		if(showDiagram)
		{
			entityDiagram.DrawChart();
		}
		EditorGUILayout.BeginHorizontal();
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width( Screen.width), GUILayout.Height(Screen.height/3) );
		GUILayout.Label(scrollText);
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndHorizontal();
		if(GUILayout.Button("Clear"))
		{
			scrollText = "Click on line point to view it\n";
		}
	}
	
	public void Click(string label, float value, int index) {
		scrollText += "Clicked on Entity_" + index + " event: " + label + " at: " + value + "\n";
	}

	void generateDataDictionary()
	{
		String[] lines = File.ReadAllLines("Assets/Logs/2015-06-02_TestLog(14).txt");
		entityEntries = new Dictionary<String, List<String>>();
		
		foreach (String line in lines)
		{
			String[] split = line.Split(':');
			
			if (!entityEntries.ContainsKey(split[0]))
			{
				entityEntries.Add(split[0], new List<string>());
			}
			if(split.Length>1)
				entityEntries[split[0]].Add(split[1]);
		}
		//get time of last entry
		string last = lines[(lines.Length) - 1];
		lastTimeStamp = float.Parse(last.Split(new String[]{":"," at "}, StringSplitOptions.None)[2]);
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
		entityDiagram.entityTimeStampsList = nodesTimeStamps;
		entityDiagram.entityLineNodeLabels = nodesData;

	}
	
	// Use this if you want real time updating
	void EditorUpdate() {
		Repaint();
	}
	
}

 