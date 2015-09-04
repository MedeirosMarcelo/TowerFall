using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	public bool shot;
	float speed = 30f;

	void Start () {
	//	Physics.gravity = new Vector3 (0, -300, 0);
	//	rigidbody.useGravity = false;
		if (shot) {
			Move ();
		}
	}

	void FixedUpdate () {
	//	rigidbody.useGravity = true;
		transform.forward = Vector3.Slerp (transform.forward, rigidbody.velocity.normalized, 10 * Time.deltaTime);
	}

	void Move(){
		rigidbody.AddForce (new Vector3 (0, 0.2f, 1f) * speed, ForceMode.Impulse);
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.name == "Floor" || 
		    col.gameObject.name == "Wall") {
			rigidbody.isKinematic = true;
		}
	}
}
