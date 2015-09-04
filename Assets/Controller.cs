using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	public bool canControl = true;
	public bool canMove = true;
	public bool dead = false;
	public bool isGrounded;
	public GameObject arrow;

	GroundCollider groundCollider;
	float runSpeed = 5f;
	float jumpForce = 30f;
	Vector3 direction;
	State state;

	enum State{
		Run,
		Shoot,
		Jump,
		Crouch,
		Dash,
		GrabLedge,
		Climb,
		SlideDown,
		Die
	}

	void Start () {
		groundCollider = GetComponentInChildren<GroundCollider> ();
	}

	void FixedUpdate () {
		isGrounded = groundCollider.isGrounded;
		Move ();
		Shoot ();
	}

	void Move() {
		direction = new Vector3(Input.GetAxis ("Horizontal") * runSpeed, 0, Input.GetAxis("Vertical") * runSpeed);
		if (isGrounded){
			if (Input.GetButtonDown ("Jump")) {
				Debug.Log("JUMP");
				rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpForce, rigidbody.velocity.z);
			}
		}
		Test ();
		rigidbody.MovePosition (this.transform.position + direction * Time.deltaTime);
	}

	void Shoot(){
		if (Input.GetButtonDown("Fire1")){
			SendArrow();
		}
	}

	void SendArrow (){

		Vector3 arrowPosition = transform.Find ("ArrowSpawner").position;
		Quaternion arrowRotation = transform.Find ("ArrowSpawner").rotation;
		Vector3 arrowSpawner = transform.Find ("ArrowSpawner").position;
		GameObject newArrow = (GameObject)Instantiate (arrow, arrowPosition, arrowRotation);
		newArrow.GetComponent<Arrow>().shot = true;
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


