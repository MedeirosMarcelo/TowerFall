using UnityEngine;
using System.Collections;

public class GroundCollider : MonoBehaviour {

	public bool isGrounded { get; private set; }
	public float rayLength;
	int layerMask = 1 << 8;
	/*
	void OnTriggerEnter(Collider col){
		if (col.tag == "Walkable") {
			isGrounded = true;
		}
	}

	void OnTriggerExit(Collider col){
		if (col.tag == "Walkable") {
			isGrounded = false;
		}
	}*/

	void Update(){
		DetectGround ();
	}

	void DetectGround(){
		RaycastHit hit;
		Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.red);
		if (Physics.Raycast (this.transform.position, Vector3.down, out hit, rayLength, layerMask)) {
			if (hit.collider.tag == "Walkable"){
				isGrounded = true;
			}
		}
		else {
			isGrounded = false;
		}
	}
}
