using UnityEngine;
using System.Collections;

public class PlayerCharacter : MonoBehaviour {

	public Texture2D deathmask;		// overlayed on death
	//public AudioClip deathSound;	// played on death
	public float deadWait = 2.0f;	// time to wait before you can return to the title

	private bool dead;
	private float deadDelay;
	private CharacterController charCtrl;
	private GameObject music;		// DIRTY! use game manager to disable music eventually

	void Start () 
	{
		charCtrl = gameObject.GetComponent("CharacterController") as CharacterController;
		music = GameObject.Find ("Music Controller");
	}

	void OnGUI ()
	{
		if (dead)
		{
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), deathmask, ScaleMode.StretchToFill, true);
		}
	}

	void Update ()
	{
		if (dead)
		{
			deadDelay += Time.deltaTime;

			if (deadDelay > deadWait)
			{
				if (Input.anyKey)
				{
					Application.LoadLevel(0);
				}
			}
		}
	}

	public bool IsDead ()
	{
		return dead;
	}

	public void Kill () 
	{
		// use this to disable character controller and play game over screen on player
		charCtrl.enabled = false;
		dead = true;
		Debug.Log("YOU ARE DEAD");

		// disable music
		if (music != null) {
			music.SetActive(false);
		}
	}

}
