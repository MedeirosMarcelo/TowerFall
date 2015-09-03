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
	float jumpForce = 18f;
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

		direction = new Vector3(Input.GetAxis ("Horizontal"), 0, Input.GetAxis("Vertical"));

		if (isGrounded){
			if (Input.GetButtonDown ("Jump")) {
				Debug.Log("JUMP");
				rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpForce, rigidbody.velocity.z);
			}
		}
		Vector3 velocity = new Vector3 (direction.x * runSpeed, direction.y * jumpForce, direction.z * runSpeed);
		rigidbody.MovePosition (this.transform.position + velocity * Time.deltaTime);
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
}


