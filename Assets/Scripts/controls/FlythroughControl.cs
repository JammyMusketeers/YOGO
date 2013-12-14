using UnityEngine;
using System.Collections;

public class FlythroughControl : MonoBehaviour {

	#region Public Members
	public float movingSpeed = 5.0f;
	public float turnSpeed = 5.0f;
	#endregion

	#region Private Members
	private float moveX;
	private float moveY;
	private float turnX;
	private float turnY;
	#endregion
	
	void Update () 
	{
		moveX = Input.GetAxis("Horizontal") * movingSpeed;
		moveY = Input.GetAxis("Vertical") * movingSpeed;

		turnX += Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime;
		turnY += Input.GetAxis("Mouse Y") * turnSpeed * Time.deltaTime;

		//transform.Translate(moveX, 0, moveY);
		transform.eulerAngles = new Vector3(turnY, turnX, 0);
	}

	void FixedUpdate ()
	{
		rigidbody.AddRelativeForce(moveX, 0, moveY);
	}
}
