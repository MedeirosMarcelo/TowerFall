using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {

    public float rayDistance = 25f;

    void Start() {
        
    }

    bool DetectFront() {
        RaycastHit hit;
        Ray ray = new Ray(this.transform.position, Vector3.forward);
        if (Physics.Raycast(ray, out hit, rayDistance)) {
            if (hit.collider.name == "Wall") {
                return true;
            }
        }
        return false;
    }

    void AdjustPosition() {

        Vector3 origin = Vector3.zero;
        Vector3 destination = Vector3.zero;
        float val = 0f;
        transform.position = Vector3.Lerp(origin, destination, val);
    }

    void OnTriggerEnter(Collider col) {
        
    }
}
