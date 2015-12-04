using UnityEngine;
using System.Collections;

public class ObjectReflection : MonoBehaviour {

	public GameObject original;

	void Update () {
		MirrorActions ();
	}

	void MirrorActions(){
		if (original) {
			this.transform.localPosition = original.transform.position;
			this.transform.localRotation = original.transform.rotation;
		}
	}
}
