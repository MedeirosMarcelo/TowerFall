using UnityEngine;
using System.Collections;

public class ObjectMirror : MonoBehaviour {

	public GameObject original;

	void Update () {
		MirrorActions ();
	}

	void MirrorActions(){
		if (original) {
			this.transform.localPosition = original.transform.localPosition;
			this.transform.localRotation = original.transform.localRotation;
		}
	}
}
