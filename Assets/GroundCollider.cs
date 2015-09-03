using UnityEngine;
using System.Collections;

public class GroundCollider : MonoBehaviour {

	public bool isGrounded { get; private set; }

	void OnTriggerEnter(Collider col){
		if (col.tag == "Walkable") {
			isGrounded = true;
		}
	}

	void OnTriggerExit(Collider col){
		if (col.tag == "Walkable") {
			isGrounded = false;
		}
	}
}
