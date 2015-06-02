using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Shows how a chart can be created in an inspector.
/// </summary> 
[CustomEditor(typeof(TestInspector))]
public class TestInspectorEditor : Editor {
	private const int maxNumbers = 50;
	private LineChart lineChart;
	private List<float> numbers;

	override public void OnInspectorGUI() {
//		if (numbers == null) numbers = new List<float>(){0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f};
//
//		// Enable this to check more frequently
//		// if (EditorApplication.update != this.EditorUpdate) EditorApplication.update += this.EditorUpdate;
////		if (EditorApplication.isPlaying && !EditorApplication.isPaused) numbers.Add((float)((TestInspector)target).random);
////		if (numbers.Count > 100) numbers.RemoveAt(0);
//		if (lineChart == null) {
//			lineChart = new LineChart(this, 200.0f);
//		}
//		lineChart.data = new List<float>[]{numbers};
//		lineChart.dataNodesLabels = new List<string>(){"A","q","w","e","r","t","y"};
//		lineChart.pipRadius = 1.0f;
//		lineChart.drawTicks = false;
//		// Make sure you do this or the cahrts wont work
//		Handles.BeginGUI();
//		lineChart.DrawChart();
//		Handles.EndGUI();
	
		
	}
	
	// Use this for more frequent updates (multiple times per frame)
	void EditorUpdate() {
		Repaint();
	}
}
