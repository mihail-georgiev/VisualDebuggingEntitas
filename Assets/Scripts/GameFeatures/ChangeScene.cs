using UnityEngine;

public class ChangeScene : MonoBehaviour {

	public void StartGame(){
		Application.LoadLevel("Game");
	}
	public void QuitGame(){
		Application.Quit();
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}
}