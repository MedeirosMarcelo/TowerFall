using UnityEngine;
using System.Collections;

public class DebugButton : MonoBehaviour {

	public Texture btnTexture;
	public int left = 10;
	public int top = 70;
	public int width = 50;
	public int height = 30;
	public string label = "Console";
	private ScreenDebug screenDebug;

	void Start(){
		screenDebug = GetComponent<ScreenDebug> ();
	}

	void OnGUI() {

		if (GUI.Button (new Rect (left, top, width, height), label)){
			if (screenDebug.buttonPressed)
				screenDebug.buttonPressed = false;
			else
				screenDebug.buttonPressed = true;
		}
	}
}
