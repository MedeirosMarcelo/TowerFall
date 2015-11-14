using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {

    public GameObject owner;
    float speed = 7f;
    private float maxDistance = 5.5f;
    int layerMask = 1 << 8;

    void LateUpdate() {
        Vector3 hit = Vector3.zero;
        if (SightBlocked(ref hit)) {
            transform.position = hit;
        }
        else if(transform.localPosition.magnitude < maxDistance) {
            transform.localPosition *= 1.1f;
            if (SightBlocked(ref hit)) {
                transform.position = hit;
            }
        }
        /* TODO: Corrigir camera NEAR
        var dist = transform.localPosition.magnitude - camera.nearClipPlane;
        transform.localPosition = transform.localPosition.normalized * dist;
        */
    }


    bool SightBlocked(ref Vector3 hitPosition) {
        Vector3 origin = owner.transform.position;
        Vector3 delta = transform.position - origin;
        Debug.Log("origin=" + origin + " cam= " + transform.position + " delta=" + delta);

        RaycastHit hit;
        Ray ray = new Ray(origin, delta.normalized);
        Debug.DrawRay(origin, delta, Color.blue);

        if (Physics.Raycast(ray, out hit, maxDistance, layerMask)) {
            Debug.Log("Hitted = " + hit.collider.name);
            if (hit.collider.tag == "Walkable") {
                Debug.Log("Hit = " + hit.point);
                 hitPosition = hit.point;
                 return true;
            }
        }
        return false;
    }



}
