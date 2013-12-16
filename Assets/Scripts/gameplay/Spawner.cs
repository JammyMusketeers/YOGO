using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public GameObject spawns;
	public float probabilityPercent;
	public bool randRotation;

	void Start () 
	{
		if (Random.value * 100.0 < probabilityPercent)
		{
			// spawn a...
			if (randRotation)
				transform.Rotate(0, Random.value * 360, 0);
			Instantiate (spawns, transform.position, transform.rotation);
		}
	}
}
