using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Entitas.Unity.VisualDebugging {
	public class DiagramWindow : EditorWindow {
		LineChart entityDiagram;
	 	GUIStyle boxStyle;
		Vector2 scrollPos = Vector2.zero;
		Vector2 scrollPos2 = Vector2.zero;
		bool showDiagram = false;


		[MenuItem ("Entitas/ShowDiagram")]
	    static void Init () {
	        // Get existing open window or if none, make a new one:
			DiagramWindow window = (DiagramWindow) EditorWindow.GetWindow (typeof (DiagramWindow),false, "Entity Diagram");
			// Listen for moves
			window.wantsMouseMove = true;
			// Create a style which loads a background image
			window.boxStyle = new GUIStyle();
			window.boxStyle.normal.background = (Texture2D) Resources.Load("Black", typeof(Texture2D));
			window.boxStyle.border = new RectOffset(32,32,32,32);
			window.boxStyle.margin = new RectOffset(4,4,4,4);
		
			window.Focus();
	    }
		
	   void OnGUI () {
			if (entityDiagram == null) {
				entityDiagram = new LineChart(this);
			}
			entityDiagram.drawControls();

			if(GUILayout.Button("Draw Chart")) {
				if(showDiagram)
					showDiagram = false;
				else {
					showDiagram = true;
				}
			}

			if(showDiagram)	{
				EditorGUILayout.BeginHorizontal(boxStyle);
				scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2, GUILayout.Width( Screen.width), GUILayout.Height(Screen.height/3) );
				entityDiagram.DrawChart();
				EditorGUILayout.EndScrollView();
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.BeginHorizontal();
				scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width( Screen.width), GUILayout.Height(Screen.height/3) );
				GUILayout.Label(entityDiagram.scrollText);
				EditorGUILayout.EndScrollView();
			EditorGUILayout.EndHorizontal();

			if(GUILayout.Button("Clear")) {
				entityDiagram.scrollText = "Click on line point to view it\n";
			}
		}
	}
}