using UnityEngine;
using System.Collections;

public interface ClickResponder {
	void Click(string label, float value, int index);
}