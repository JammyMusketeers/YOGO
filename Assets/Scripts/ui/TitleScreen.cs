using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour {

	public GUISkin guiSkin;
	public string nextLevel;
	public float skipDelay = 3.0f;
	public float textDelay = 4.0f;

	private float alphaText;

	void OnGUI () 
	{
		GUI.skin = guiSkin;

		GUILayout.BeginArea(new Rect(Screen.width * 0.45f, 
		                             Screen.height * 0.7f, 
		                             Screen.width * 0.2f, 
		                             Screen.height * 0.3f));
		if (Time.timeSinceLevelLoad > textDelay)
		{
			GUI.color = new Color(1, 1, 1, alphaText);
			GUILayout.Label ("PRESS ANY KEY");
		}
		GUILayout.EndArea();
	}

	void Update ()
	{
		alphaText = Mathf.Sin (Time.timeSinceLevelLoad);

		if (Time.timeSinceLevelLoad > skipDelay)
		{
			if (Input.anyKey)
			{
				// any key pressed
				Application.LoadLevel(nextLevel);
			}
		}
	}
}
