using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

	public int points = 1000;
	public AudioClip collectSound;
	public float randomiseSize;
	public float randomisePoints;

	private bool pickedUp;
	private PlayerCharacter playerScript;
	
	void Start ()
	{
		float rand = (Random.value * 2f) - 1f;
		if (randomisePoints > 0) 
		{
			points += Mathf.CeilToInt(randomisePoints * rand);
		}

		if (randomiseSize > 0) 
		{
			transform.localScale = new Vector3(1 + (rand * randomiseSize), 
			                                   1 + (rand * randomiseSize), 
			                                   1 + (rand * randomiseSize));
		}
	}

	void OnTriggerEnter (Collider other) 
	{
		if (!pickedUp)
		{
			if (other.gameObject.tag == "Player")
			{
				//Debug.Log("holy crap you guys the player picked me up");
				playerScript = other.gameObject.GetComponent("PlayerCharacter") as PlayerCharacter;
				Collect(playerScript);
			}
		}
	}

	void Collect (PlayerCharacter target)
	{
		if (collectSound)
			audio.PlayOneShot(collectSound);

		target.score += points;

		pickedUp = true;
		renderer.enabled = false;
	}
}
