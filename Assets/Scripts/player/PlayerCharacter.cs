using UnityEngine;
using System.Collections;

public class PlayerCharacter : MonoBehaviour {

	public GUISkin guiSkin;
	public Texture2D deathmask;		// overlayed on death
	//public AudioClip deathSound;	// played on death
	public float deadWait = 2.0f;	// time to wait before you can return to the title
	public int score;

	private bool dead;
	private float deadDelay;
	private CharacterController charCtrl;
	private float scoreShow;
	private GameObject music;		// DIRTY! use game manager to disable music eventually

	void Start () 
	{
		charCtrl = gameObject.GetComponent("CharacterController") as CharacterController;
		music = GameObject.Find ("Music Controller");


	}

	void OnGUI ()
	{
		GUI.skin = guiSkin;
		if (dead)
		{
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), deathmask, ScaleMode.StretchToFill, true);
		}
		else if (scoreShow > 0)
		{
			GUI.color = new Color(1f, 1f, 0.2f, scoreShow);
			GUI.Label (new Rect(Screen.width-200, Screen.height - 100, 200, 100), "Gold Collected!");
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
		else 
		{
			// hide cursor
			Screen.showCursor = false;
			Screen.lockCursor = true;

			if (scoreShow > 0)
			{
				scoreShow -= Time.deltaTime;
			}
		}
	}

	public void AddScore (int amount)
	{
		score += amount;
		scoreShow += 2.0f;
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
