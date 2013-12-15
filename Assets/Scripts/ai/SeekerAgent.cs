using UnityEngine;
using System.Collections;

public class SeekerAgent : MonoBehaviour {

	#region Public Members
	public float moveSpeed = 50f;
	public float turnSpeed = 50f;
	#endregion

	#region Private Members
	private CharacterController charCtrl;
	private Vector3 moveVector;
	private Vector3 forward;

	private float turnCooldown;
	#endregion

	void Start () 
	{
		charCtrl = gameObject.GetComponent("CharacterController") as CharacterController;
	}

	void Update () 
	{
		forward = transform.TransformDirection(Vector3.forward);
		charCtrl.SimpleMove(forward * moveSpeed);

		if (turnCooldown <= 0)
		{
			if ((charCtrl.collisionFlags & CollisionFlags.Sides) != 0) 
			{
				transform.Rotate (0, Random.Range(-95, 95), 0);
				turnCooldown = 0.1f;
			}
		} 
		else
		{
			turnCooldown -= Time.deltaTime;
		}
	}
}
