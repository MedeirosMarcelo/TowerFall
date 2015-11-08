using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {

    public GameObject owner;
    float speed = 7f;
    public float cameraDistance = 5f;
    public float maxDistance = -5.5f;
    public float rayDistance = 5f;
    public Vector3 cameraPosition;

    void LateUpdate() {
        AdjustPosition(cameraPosition);
    }

    void AdjustPosition(Vector3 defaultPosition) {

        float distance;
        SightBlocked(out distance);

        if (distance > cameraDistance) {
            distance = cameraDistance;
        }

        cameraPosition = new Vector3(1.71f, 1.62f, -distance);
        //Debug.Log("Distance " + distance + " " + cameraPosition);
        transform.localPosition = cameraPosition;
    }

    void SightBlocked(out float hitPoint) {
        Vector3 origin = owner.transform.position;
        Vector3 direction = transform.position - origin;
        RaycastHit hit;
        Ray ray = new Ray(origin, direction);
        Debug.DrawRay(origin, direction, Color.blue);
        if (Physics.Raycast(ray, out hit, rayDistance)) {
            if (hit.collider.tag == "Walkable") {
                hitPoint = Vector3.Distance(hit.point, origin);
            }
            else {
                hitPoint = cameraDistance;
            }
        }
        else {
            hitPoint = cameraDistance;
        }
    }
}
