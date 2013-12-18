using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour {

	public GUISkin guiSkin;
	public string nextLevel;
	public Texture2D instructions;
	public float skipDelay = 3.0f;
	public float textDelay = 4.0f;
	public float instructionsDelay = 2.0f;

	private float alphaText;
	private bool showInstructions;
	private float instructionScreenTimer;

	void OnGUI () 
	{
		GUI.color = new Color(.2f, .2f, .2f, 1);
		GUI.Label (new Rect(Screen.width - 300, Screen.height - 100, 300, 50),"By Alexander Webb, for Ludum Dare Jam 28");
		GUI.Label (new Rect(Screen.width - 150, Screen.height - 50, 150, 50),"Music by Rich Webb");

		GUI.skin = guiSkin;
		if (instructionScreenTimer > 0) 
		{
			GUI.color = new Color(1, 1, 1, 1);
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), instructions, ScaleMode.StretchToFill, true);
		}
		else
		{
			GUILayout.BeginArea(new Rect(Screen.width * 0.425f, 
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

	}

	void Update ()
	{
		// show cursor
		Screen.showCursor = true;
		Screen.lockCursor = false;

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		alphaText = Mathf.Sin (Time.timeSinceLevelLoad);

		if (showInstructions)
		{
			instructionScreenTimer += Time.deltaTime;
			if (instructionScreenTimer > instructionsDelay)
			{
				if (Input.anyKey)
				{
					Application.LoadLevel(nextLevel);
				}
			}
		}
		else
		{
			if (Time.timeSinceLevelLoad > skipDelay)
			{
				if (Input.anyKey)
				{
					// show instructions
					showInstructions = true;
				}
			}
		}
	}
}
