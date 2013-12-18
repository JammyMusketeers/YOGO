using UnityEngine;
using System.Collections;

public class SeekerAgent : MonoBehaviour {

	#region Public Members
	public float moveSpeed = 50f;			// how fast it travels
	public float chaseSpeed = 100f;			// how fast it goes when alerted
	public float turnSpeed = 50f;			// how fast it turns
	public float thinkInterval = 0.2f;		// time between AI rethinks
	public float directionInterval = 10f;	// time between directional changes
	public float navTimeout = 5.0f;			// times out getting to a waypoint after this time
	public float searchTime = 20f;			// time to keep searching after losing sight
	public float viewDistance = 35f;		// distance it can see
	public float viewAngle = 45f;			// degrees from forward sight extends to
	public float killRange = 0.5f;			// player is dead within this range

	public AudioClip killSound;				// played when it kills player
	public AudioClip alertSound;			// sound played when it sees you!
	public AudioClip[] chatterSounds;		// sounds used for audio feedback
	#endregion

	#region Private Members
	private CharacterController charCtrl;
	private Vector3 moveVector;
	private float currentSpeed;				// speed chosen
	private Vector3 forward;
	private float currentVelocity;
	private Vector3 lastPosition;

	private bool alerted;					// seen player? if so, we will search or chase
	public float lostSightFor;				// time since player last seen
	private Vector3 pursuitTarget;
	private Vector3 navTarget;				// tranform to follow out of alert
	private float navTimer;					// time taken to reach next nav
	private GameObject[] navPoints;
	private float targetDistance;
	private float thinkTimer;
	private GameObject player;				// ref to player
	private PlayerCharacter playerScript;
	private float directionTimer;			// counts how long we've been going one way

	private int audioChoice;				// sound to play from chatter

	#endregion

	void Start () 
	{
		charCtrl = gameObject.GetComponent("CharacterController") as CharacterController;
		navPoints = GameObject.FindGameObjectsWithTag("nav") as GameObject[];
	}

	void FixedUpdate ()
	{
		currentVelocity = Vector3.Distance(transform.position, lastPosition) * (1/Time.deltaTime);
		lastPosition = transform.position;
	}

	void Update () 
	{
		// detect collision
		if ((charCtrl.collisionFlags & CollisionFlags.Sides) != 0) 
		{
			transform.Rotate (0, turnSpeed * Time.deltaTime, 0);
		}

		// do rethink?
		if (thinkTimer >= thinkInterval)
		{
			// rethink AI
			CheckForPlayer();
			thinkTimer = 0;
		} else {
			thinkTimer += Time.deltaTime;
		}

		if (alerted) {
			// fast!
			currentSpeed = chaseSpeed;
			// Chase target
			transform.LookAt(pursuitTarget);
			transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
			if (lostSightFor == 0)
			{
				if (targetDistance < killRange && !playerScript.IsDead()) 
				{
					audio.PlayOneShot(killSound);
					playerScript.Kill();
				}
			}
		} else {
			// normal speed
			currentSpeed = moveSpeed;

			transform.LookAt(navTarget);
			transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

			directionTimer += Time.deltaTime;

			if (directionTimer > directionInterval) 
			{
				// change direction
				turnSpeed = -turnSpeed;
				directionTimer = 0;
			}

			navTimer += Time.deltaTime;
			if (navTimer > navTimeout) 
			{
				// change direction
				navTarget = GetNearestNavTo(transform.position, 20f);
				navTimer = 0;
			}

			if (Vector3.Distance(navTarget, transform.position) < 1) 
			{
				navTimer = 0;

				// go to new nav target
				navTarget = GetNearestNavTo(navTarget, 20f);
			}
		}

		// move
		forward = transform.TransformDirection(Vector3.forward);
		charCtrl.SimpleMove(forward * currentSpeed);
	}

	Vector3 GetNearestNavTo (Vector3 nearPoint, float longDistance = 20.0f)
	{
		Vector3 newPoint = Vector3.zero;
		foreach (GameObject navPoint in navPoints) 
		{
			float dist = Vector3.Distance(navPoint.transform.position, nearPoint);
			if (dist > 1)
			{
				dist += Random.value * 2.0f;
				if (dist < longDistance) 
				{
					longDistance = dist;
					newPoint = navPoint.transform.position;
				}
			}
		}
		return newPoint;
	}


	bool CheckForPlayer ()
	{
		bool success = false;

		if (player == null)
		{
			player = GameObject.FindGameObjectWithTag("Player");
			playerScript = player.GetComponent("PlayerCharacter") as PlayerCharacter;
		}

		Vector3 targetRelative = player.transform.position - transform.position;
		Quaternion newRotation = new Quaternion();
		newRotation.SetLookRotation(targetRelative);

		if (Quaternion.Angle(transform.rotation, newRotation) < viewAngle)
		{
			// potentially sees you
			//Debug.Log ("Should see you.");

			RaycastHit hit = new RaycastHit();
			Vector3 lookingPoint = player.transform.position - transform.position;
			if (Physics.Raycast (transform.position + Vector3.up, lookingPoint, out hit, viewDistance))
			{
				Debug.DrawLine (transform.position + Vector3.up, hit.point, Color.red);
				Collider seesCol = hit.collider;
				
				if (seesCol.gameObject == player) {
					// I SEE YOU
					targetDistance = hit.distance;
					Debug.Log (targetDistance);
					pursuitTarget = seesCol.transform.position;
					Alarm();
					success = true;
				} else if (alerted) {
					// player evading...
					lostSightFor += thinkInterval;
					PursuitTechniques();
				}
				if (lostSightFor > searchTime) {
					alerted = false;
					lostSightFor = 0;
				}
			}
		}

		return success;
	}

	void Alarm () 
	{
		if (!alerted){
			alerted = true;
			audio.PlayOneShot(alertSound);
		} 
		else if (lostSightFor > 0)
		{
			audioChoice = Random.Range(0, chatterSounds.Length);
			audio.PlayOneShot(chatterSounds[audioChoice]);
		}
		lostSightFor = 0f;
	}

	void PursuitTechniques () 
	{
		if (Vector3.Distance(transform.position, pursuitTarget) < 0.5) {
			// reached last known position
			pursuitTarget = player.transform.position;
			// follow the "sound" of where he went (gasp, cheating!)
		}
		if (currentVelocity < 0.2) {
			// some code to try and prevent stickage
			pursuitTarget = transform.position;
			int action = Random.Range(0, 99);
			if (action > 75) 
			{
				pursuitTarget.x += 8;
			} 
			else if (action > 50) 
			{
				pursuitTarget.x -= 8;
			} 
			else if(action > 25) 
			{
				pursuitTarget.z += 8;
			} 
			else 
			{
				pursuitTarget.z -= 8;
			}
		}
		
	}
}
