using UnityEngine;
using System.Collections;

public class HandsCollider : MonoBehaviour {

	public bool canGrabLedge { get; private set;}

	void OnTriggerEnter(Collider col){
		if (col.name == "Surface") {
			canGrabLedge = true;
		}
	}

	void OnTriggerExit(Collider col){
		if (col.name == "Surface") {
			canGrabLedge = false;
		}
	}
}
