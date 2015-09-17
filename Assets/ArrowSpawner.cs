using UnityEngine;
using System.Collections;

public class ArrowSpawner : MonoBehaviour {

    public GameObject camera;

	void Update () {
        RotateWithCamera();
	}

    void RotateWithCamera() {
        if (camera != null) {
            this.transform.localRotation = camera.transform.localRotation;
        }
    }
}
