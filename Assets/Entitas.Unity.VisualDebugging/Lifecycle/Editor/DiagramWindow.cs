using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class DiagramWindow : EditorWindow , ClickResponder {

	LineChart entityDiagram;

 	GUIStyle boxStyle;

	Vector2 scrollPos = Vector2.zero;
	string scrollText = "";
	bool showDiagram = false;

	[MenuItem ("Entitas/ShowDiagram")]
    static void Init () {
        // Get existing open window or if none, make a new one:
		DiagramWindow window = (DiagramWindow) EditorWindow.GetWindow (typeof (DiagramWindow));
		
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
		if (entityDiagram == null){
			entityDiagram = new LineChart(this);
		}
		entityDiagram.drawControls();
		entityDiagram.clickResponder = this;

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

	// Use this if you want real time updating
	void EditorUpdate() {
		Repaint();
	}
}