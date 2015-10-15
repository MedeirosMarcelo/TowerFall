using UnityEngine;
using System.Collections;

public class DetectWall : MonoBehaviour {

    public bool hitWall;

    void OnTriggerEnter(Collider col) {
        if (col.name == "Wall") {
            hitWall = true;
        }
    }

    void OnTriggerExit(Collider col) {
        if (col.name == "Wall") {
            hitWall = false;
        }
    }
}
