using UnityEngine;
using System.Collections;

public class ExitPoint : MonoBehaviour {

	public GUISkin guiSkin;
	public Texture2D winScreen;
	public float restartDelay = 2.0f;

	private bool depart;
	private PlayerCharacter playerScript;
	private float restartTimer;

	void OnTriggerEnter (Collider other) 
	{
		if (other.gameObject.tag == "Player")
		{
			playerScript = other.gameObject.GetComponent("PlayerCharacter") as PlayerCharacter;
			// player leaving
			depart = true;
			other.gameObject.transform.position = new Vector3(0,-10, 0);	// horrible hacky way of getting rid of player for now
			playerScript.enabled = false;
		}
	}

	void Update ()
	{
		if (depart)
		{
			restartTimer += Time.deltaTime;
			if (restartTimer > restartDelay)
			{
				if (Input.anyKey) 
				{
					Application.LoadLevel(0);
				}
			}
		}
	}

	void OnGUI ()
	{
		if (depart)
		{
			// show cursor
			Screen.showCursor = true;
			Screen.lockCursor = false;

			// show stats
			GUI.skin = guiSkin;
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), winScreen, ScaleMode.StretchToFill, true);
			GUILayout.BeginArea(new Rect(Screen.width * 0.45f, 
			                             Screen.height * 0.7f, 
			                             Screen.width * 0.2f, 
			                             Screen.height * 0.3f));
			GUI.color = Color.yellow;
			GUILayout.Label (playerScript.score + " G");
			GUILayout.EndArea();
		}
	}

}
