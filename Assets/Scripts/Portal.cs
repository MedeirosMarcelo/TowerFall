using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

	public Portal otherPortal;
	public bool teleport = false;

	void OnTriggerEnter (Collider col) {
		if (!teleport) {
			col.transform.localPosition = new Vector3((col.transform.localPosition.x * -1f), col.transform.localPosition.y, col.transform.localPosition.z);
			otherPortal.teleport = true;
		}
	}

	void OnTriggerExit(Collider col){
		teleport = false;
	}
}
