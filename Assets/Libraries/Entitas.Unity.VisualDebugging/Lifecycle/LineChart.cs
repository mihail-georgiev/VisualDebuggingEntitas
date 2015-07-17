using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using Eppy;

namespace Entitas.Unity.VisualDebugging {
	public class LineChart {
		EditorWindow window;
		float windowHeight;

		float chartBorderHorizontal = 60;
		float chartBorderVertikal = 20;
		int chartSections = 10;
		float sectionWidth;
		float chartFloor ;
		float chartTop;


		float timeStep;

		Dictionary<String, List<String>> entityEntries;
		List <float>[] entityNodesTimeStamps;
		List<string>[] entityLineNodeLabels;
		float lastRecordedTime;
			
		List<Color> colors = new List<Color>{Color.magenta, Color.cyan * 2.0f, Color.green, Color.yellow, Color.red, Color.grey};	

		Color axisColor = Color.green;
		Color fontColor = Color.yellow;
		
		float pipRadius = 3.0f;

		public string scrollText = "";

		//for the new stuff
		Dictionary<string, List<Tuple<string,string>>>[] sortedEntityData;
		HashSet<string> componentList;
		string lastLogFilePath;

		bool op1 = true;
		bool op2 = true;
		bool[] selectedComponents;
		float timeFrameBeginn,timeFrameEnd; 

		public LineChart(EditorWindow window) {
			readEntriesDataFromFile();
			generateChartData();
			int linesToDraw = entityEntries.Count;
			float height = linesToDraw*20 +100;
			this.window = window;
			this.windowHeight = height;
			selectedComponents = new bool[componentList.Count];
			timeFrameBeginn = 0f;
			timeFrameEnd = lastRecordedTime/3;
		}

		public void drawControls() {
			EditorGUILayout.HelpBox("Read from file: " + lastLogFilePath + 
				"\nLast Entry recorded at: " + lastRecordedTime + " ms" +
			                        "\nRecorded Entities: " + sortedEntityData.Length, MessageType.Info);


			op1 = EditorGUILayout.Toggle("Show all Entity data", !op2,(GUIStyle)"Radio");
			op2 = EditorGUILayout.Toggle("Show events for selected components only", !op1,(GUIStyle)"Radio");
			if(op2){
				EditorGUILayout.BeginVertical();
				for(int i =0; i<selectedComponents.Length;i++){
					selectedComponents[i] = EditorGUILayout.Toggle(componentList.ElementAt(i), selectedComponents[i]);
				}
			
				EditorGUILayout.EndVertical();
			}
			else {
				for(int i =0; i<selectedComponents.Length;i++){
					selectedComponents[i] = true;
				}
			}
			EditorGUILayout.MinMaxSlider(ref timeFrameBeginn, ref timeFrameEnd, 0f, lastRecordedTime);
			EditorGUILayout.LabelField("min " + timeFrameBeginn);
			EditorGUILayout.LabelField("max " + timeFrameEnd);
			EditorGUILayout.LabelField("time frame " + (timeFrameEnd - timeFrameBeginn));
			timeStep = (timeFrameEnd - timeFrameBeginn)/chartSections;
			chartSections = EditorGUILayout.IntField ("Sections", chartSections);
		}

		public void DrawChart() {	
			if(entityNodesTimeStamps.Length > 0) {
				creteChartRect ();
//				drawEntityLines ();
				drawEntityEvents();
		    	drawAxisAndEntityLines();
				drawSectionsAndLabels();
			}
		}
	
		void creteChartRect() {
			Rect rect = GUILayoutUtility.GetRect (Screen.width, windowHeight);
			chartTop = rect.y + chartBorderVertikal;
			sectionWidth = (float)(Screen.width - (chartBorderHorizontal * 2)) / chartSections;
			chartFloor = rect.y + rect.height - chartBorderVertikal;
		}

		void drawEntityLines() {
			for (int i = 0; i < entityNodesTimeStamps.Length; i++) {
				if (entityNodesTimeStamps [i] != null) {
					drawLine (entityNodesTimeStamps [i], entityLineNodeLabels [i], "Entity_" + i, i);
				}
			}
		}

		void drawAxisAndEntityLines() {		
			Handles.color = axisColor;
			Handles.DrawLine (new Vector2 (chartBorderHorizontal, chartTop), new Vector2 (chartBorderHorizontal, chartFloor));
			Handles.DrawLine (new Vector2 (chartBorderHorizontal, chartFloor), new Vector2 (Screen.width, chartFloor));
			Handles.color = Color.gray;
			for(int i =0; i<sortedEntityData.Length;i++)
			{
				Handles.DrawLine (new Vector2 (chartBorderHorizontal, -(i+1)*20+chartFloor), new Vector2 (Screen.width, -(i+1)*20+chartFloor));
				//colect components
				string compList = "";
				foreach(var comp in sortedEntityData[i].Keys){
					compList+= comp + "\n";
				}
				//draw enitity line labels in front
				GUIStyle colorStyle = new GUIStyle ();
				colorStyle.normal.textColor = Color.white;
				Rect labelRect = new Rect (1, -(i+1)*20+chartFloor - 8, 100, 16);
//				sortedEntityData[i].Keys.ToString();
				GUI.Label (labelRect, new GUIContent("Entity_" + i,compList), colorStyle);
			}
		}
		//
		void drawSectionsAndLabels() {
			GUIStyle centeredStyle = new GUIStyle();
			centeredStyle.alignment = TextAnchor.UpperCenter;
			centeredStyle.normal.textColor = fontColor;
			
			for (int i = 0; i <= chartSections; i++) {
				if (i > 0)
					Handles.DrawLine (new Vector2 (chartBorderHorizontal + (sectionWidth * i), chartFloor - 3), new Vector2 (chartBorderHorizontal + (sectionWidth * i), chartFloor + 3));
				
				Rect labelRect = new Rect (chartBorderHorizontal + (sectionWidth * i) - sectionWidth / 2.0f, chartFloor + 5, sectionWidth, 16);
				GUI.Label(labelRect, "" + (timeFrameBeginn + i * timeStep) + "ms", centeredStyle);
			}
		}

		void drawEntityEvents()
		{
			if(op1)// all components selected
			{	

				for(int i = 0; i<sortedEntityData.Length; i++)
				{	
					int colr = 0;
					foreach(string componentKey in sortedEntityData[i].Keys){
						Handles.color = colors[colr++];
						if(colr> colors.Count-1)
							colr = 0;
						List<Tuple<string,string>> componentEventList = sortedEntityData[i][componentKey];
						foreach(Tuple<string,string> eventEntry in componentEventList)
						{
							float timeStamp = float.Parse(eventEntry.Item2);
							if(timeStamp>= timeFrameBeginn && timeStamp<=timeFrameEnd){
								float centerX = chartBorderHorizontal + ((timeStamp-timeFrameBeginn)/(timeFrameEnd-timeFrameBeginn))*(Screen.width - chartBorderHorizontal);
								float centerY = -(i+1)*20+chartFloor;
								Handles.DrawSolidDisc(new Vector2(centerX,centerY), Vector3.forward, pipRadius);
							} 
						}
					} 
				}
			}

			else{
				List<string> comp = new List<string>();
				for(int i =0; i < selectedComponents.Length; i++){ //get selected components
					if(selectedComponents[i])
						comp.Add(componentList.ElementAt(i));
				}
				//check if entity has this components
				List<int> entitiesToDisplay = new List<int>();

				for(int i = 0; i<sortedEntityData.Length; i++){
					int checker = comp.Count;
					foreach(string c in comp){
						if(sortedEntityData[i].Keys.Contains(c))
							checker--;
					}
					if(checker == 0)
						entitiesToDisplay.Add(i);
				}
				if(entitiesToDisplay.Count>0)
				{
					for(int i=0; i<entitiesToDisplay.Count; i++){
						int entIndex = entitiesToDisplay[i];
						var temp = sortedEntityData[entIndex];
						int colr = 0;
						foreach(var c in comp){
							Handles.color = colors[colr++];
							if(colr> colors.Count-1)
								colr = 0;
							var list = temp[c];
							foreach(var l in list){
								float timeStamp = float.Parse(l.Item2);
								if(timeStamp>= timeFrameBeginn && timeStamp<=timeFrameEnd){
									float centerX = chartBorderHorizontal + ((timeStamp-timeFrameBeginn)/(timeFrameEnd-timeFrameBeginn))*(Screen.width - chartBorderHorizontal);
									float centerY = -(entIndex+1)*20+chartFloor;
									Handles.DrawSolidDisc(new Vector2(centerX,centerY), Vector3.forward, pipRadius);
								}
							}
						}
					}
				}
			}
		}



		void drawLine(List<float> timeStampList, List<string> dataNodesLabels, string label, int index) {
//			Vector2 previousLine = Vector2.zero;
//			Vector2 newLine;
//			Handles.color = Color.blue;
//			
//			for (int i = 0; i < timeStampList.Count; i++) {	
//				if(timeStampList[i] > internTimeTo+2)
//					break;
//
//				float lineX = chartBorderHorizontal + ((timeStampList[i]-internTimeFrom)/(internTimeTo-internTimeFrom))*(Screen.width - chartBorderHorizontal);
//				float lineY = -(index+1)*20+chartFloor;
//				newLine = new Vector2( lineX, lineY);
//				if(timeStampList[i] < internTimeFrom) {
//					previousLine = newLine;
//					continue;
//				}
//		
//				if (i > 0) {
//					previousLine.x = previousLine.x < chartBorderHorizontal ? chartBorderHorizontal: previousLine.x;
//					Handles.DrawAAPolyLine(previousLine, newLine);
//				}
//				previousLine = newLine;
//			
//				Rect selectRect = new Rect((previousLine - (Vector2.up * 0.5f)).x - pipRadius * 3, (previousLine - (Vector2.up * 0.5f)).y - pipRadius * 3, pipRadius * 6, pipRadius * 6);
//				if (selectRect.Contains(Event.current.mousePosition)) {
//					GUIStyle centeredStyle = new GUIStyle();
//					centeredStyle.alignment = TextAnchor.UpperCenter;
//					centeredStyle.normal.textColor = fontColor;
//					Handles.DrawSolidDisc(previousLine - (Vector2.up * 0.5f), Vector3.forward, pipRadius * 1.5f);
//
//					// Listen for click
//					if (Event.current.button == 0 && Event.current.isMouse && Event.current.type == EventType.MouseDown) {
//						scrollText += "Clicked on Entity_" + index + " event: " + dataNodesLabels[i] + " at: " + timeStampList[i] + "\n";
//					}
//					if (window != null)
//						window.Repaint();
//				} else {
//					Handles.DrawSolidDisc(previousLine - (Vector2.up * 0.5f), Vector3.forward, pipRadius);
//				}
//			}
//
//			if (label != null) {
//				drawEntityLineLabel(color, label, previousLine.y);
//			}
		}

		static void drawEntityLineLabel (Color color, string label, float lineHeight) {
			GUIStyle colorStyle = new GUIStyle ();
			colorStyle.normal.textColor = color;
			Rect labelRect = new Rect (1, lineHeight - 8, 100, 16);
			GUI.Label (labelRect, label, colorStyle);
		}

		void readEntriesDataFromFile() {	
			string[] allLogFiles = Directory.GetFiles("Assets/Logs/", "*.txt");
			lastLogFilePath = allLogFiles[allLogFiles.Length-1];
			String[] lines = File.ReadAllLines(lastLogFilePath);
			entityEntries = new Dictionary<String, List<String>>();
			
			foreach (string line in lines) {
				string[] split = line.Split(':');
				
				if (!entityEntries.ContainsKey(split[0])) {
					entityEntries.Add(split[0], new List<string>());
				}
				if(split.Length>1)
					entityEntries[split[0]].Add(split[1]);
			}

			string lastEntry = lines[(lines.Length) - 1];
			lastRecordedTime = float.Parse(lastEntry.Split(new string[]{":"," at "}, StringSplitOptions.None)[2]);
		}

		void newStuff(){
			string[] separators = new string[]{" -> ",","," at "};
			sortedEntityData = new Dictionary<string, List<Tuple<string,string>>>[entityEntries.Keys.Count];
			int index = 0;
			foreach(KeyValuePair<String, List<String>> entry in entityEntries){
				sortedEntityData[index] = new Dictionary<string, List<Tuple<string, string>>>();
				foreach(string eventEntry in entry.Value){
					string[] split = eventEntry.Split(separators,StringSplitOptions.None);
					if(!sortedEntityData[index].ContainsKey(split[1])){
						sortedEntityData[index].Add(split[1], new List<Tuple<string, string>>());
					}
						sortedEntityData[index][split[1]].Add(Tuple.Create(split[0] + " " + split[1] + ", " + split[2], split[3]));
				}
				index++;
			}
			componentList = new HashSet<string>();
			foreach(var dict in sortedEntityData){
				foreach(string key in dict.Keys){
					componentList.Add(key);
				}
			}
		}
		
		void generateChartData() {	
			newStuff();
			List<string>[] nodesData = new List<string>[entityEntries.Count];
			List<float>[] nodesTimeStamps = new List<float>[entityEntries.Count];
			string[] separators = new string[]{" at "};
			int index = 0;
			foreach(KeyValuePair<String, List<String>> pair in entityEntries) {
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
}