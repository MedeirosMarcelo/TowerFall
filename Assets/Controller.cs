using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	public bool canControl = true;
	public bool canMove = true;
	public bool dead = false;
	public bool isGrounded;
	public bool canGrabLedge;
	public GameObject arrow;

	GroundCollider groundCollider;
	HandsCollider handsCollider;
	GameObject arrowSpawner;
	float runSpeed = 5f;
	float jumpForce = 30f;
	Vector3 direction;
	State state;

	enum State{
		Idle,
		Run,
		Shoot,
		Jump,
		Crouch,
		Dash,
		GrabLedge,
		Climb,
		SlideDown,
		Fall,
		Die
	}

	void StateMachine(){
		switch (state) {
		case State.Idle:
			break;
		case State.Run:
			break;
		case State.Shoot:
			break;
		case State.Jump:
			break;
		case State.Crouch:
			break;
		case State.Dash:
			break;
		case State.GrabLedge:
			break;
		case State.Climb:
			break;
		case State.SlideDown:
			break;
		case State.Fall:
			break;
		case State.Die:
			break;
		}
	}

	void EnterState(State newState){
		OnStateExit();
		state = newState;

		switch (state) {
		case State.Idle:
			break;
		case State.Run:
			break;
		case State.Shoot:
			break;
		case State.Jump:
			break;
		case State.Crouch:
			break;
		case State.Dash:
			break;
		case State.GrabLedge:
			break;
		case State.Climb:
			break;
		case State.SlideDown:
			break;
		case State.Fall:
			break;
		case State.Die:
			break;
		}
	}

	void OnStateExit(){
		switch (state) {
		case State.Idle:
			break;
		case State.Run:
			break;
		case State.Shoot:
			break;
		case State.Jump:
			break;
		case State.Crouch:
			break;
		case State.Dash:
			break;
		case State.GrabLedge:
			break;
		case State.Climb:
			break;
		case State.SlideDown:
			break;
		case State.Fall:
			break;
		case State.Die:
			break;
		}
	}
	
	void Start () {
		groundCollider = GetComponentInChildren<GroundCollider> ();
		handsCollider =  GetComponentInChildren<HandsCollider> ();
	}

	void FixedUpdate () {
		StateMachine ();
		isGrounded = groundCollider.isGrounded;
		canGrabLedge = handsCollider.canGrabLedge;
		Move ();
		GrabLedge ();

	}

	void Update(){
		Shoot ();
//		arrowSpawner.transform.rotation = this.transform.rotation;
	}

	void Move() {
		direction = new Vector3(Input.GetAxis ("Horizontal") * runSpeed, 0, Input.GetAxis("Vertical") * runSpeed);
		if (isGrounded){
			if (Input.GetButtonDown ("Jump")) {
				Debug.Log("JUMP");
				rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpForce, rigidbody.velocity.z);
			}
		}
		direction = transform.TransformDirection(direction);
		rigidbody.MovePosition (this.transform.position + direction * Time.deltaTime);
	//	Test ();
	}

	void Shoot(){
		if (Input.GetButtonDown("Fire1")){
			SendArrow();
		}
	}

	void SendArrow (){

		Vector3 arrowPosition = transform.Find ("ArrowSpawner").position;
		Quaternion arrowRotation = transform.Find ("ArrowSpawner").rotation;
		GameObject newArrow = (GameObject)Instantiate (arrow, arrowPosition, arrowRotation);
		newArrow.GetComponent<Arrow>().shot = true;
	}

	void GrabLedge(){
		if (canGrabLedge) {
			if (Input.GetAxisRaw("Vertical") > 0){
				if (Input.GetButtonDown ("Jump")) {
					Debug.Log("JUMP");
					rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpForce, rigidbody.velocity.z);
				}
				else {
					rigidbody.useGravity = false;
					rigidbody.velocity = Vector3.zero;
					Debug.Log("HANGING");
				}
			}
			else{
				ReleaseLedge();
		//		Debug.Log("RELEASE");
			}
		}
		else{
			ReleaseLedge();
	//		Debug.Log("RELEASE");
		}
	}

	void ReleaseLedge(){
		rigidbody.useGravity = true;
	}
	
	void Test()
	{
		// Get the velocity
		Vector3 horizontalMove = rigidbody.velocity;
		// Don't use the vertical velocity
		horizontalMove.y = 0;
		// Calculate the approximate distance that will be traversed
		float distance =  horizontalMove.magnitude * Time.fixedDeltaTime;
		// Normalize horizontalMove since it should be used to indicate direction
		horizontalMove.Normalize();
		RaycastHit hit;
		
		// Check if the body's current velocity will result in a collision
		if(rigidbody.SweepTest(horizontalMove, out hit, distance))
		{
			// If so, stop the movement
			rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0);
		}
	}
}


