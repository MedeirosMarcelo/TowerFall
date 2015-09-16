using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	public bool shot;
	float speed = 35f;
	float rotationSpeed = 10f;
	float lifespan = 3f;
	float arc = 0.2f;
	float endTime;

	void Start () {
		endTime = lifespan + Time.time;
	//	Physics.gravity = new Vector3 (0, -300, 0);
		if (shot) {
			Move ();
		}
	}

	void FixedUpdate () {
		transform.forward = Vector3.Slerp (transform.forward, rigidbody.velocity.normalized, rotationSpeed * Time.deltaTime);
	//	DestroyOnTime ();
	}

	void Move(){
		Vector3 direction = transform.forward;
		direction.y += arc; 
		rigidbody.AddForce (direction * speed, ForceMode.Impulse);
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.name == "Floor" || 
		    col.gameObject.name == "Wall") {
            Debug.Log(col.gameObject.name);
			rigidbody.isKinematic = true;
		}
	}

	void DestroyOnTime(){
		if (Time.time > endTime) {
			Destroy (this.gameObject);
		}
	}
}
