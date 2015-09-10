using UnityEngine;
using System.Collections;

public class ObjectMirror : MonoBehaviour {

	public GameObject target;

	void Update () {
		MirrorActions ();
	}

	void MirrorActions(){
		if (target) {
			this.transform.localPosition = target.transform.localPosition;
			this.transform.localRotation = target.transform.localRotation;
		}
	}
}
