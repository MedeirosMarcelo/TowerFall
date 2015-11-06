using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {

    public GameObject owner;
    float speed = 0.25f;
    public float maxDistance = -5.5f;
    public float rayDistance = 5f;
    Vector3 cameraPosition = new Vector3(1.71f, 1.62f, -5.51f);

    void Update() {
        AdjustPosition(cameraPosition);
    }

    void AdjustPosition(Vector3 defaultPosition) {

        Vector3 hitPoint;
        Vector3 targetPosition;

        if (SightBlocked(out hitPoint)) {
            Debug.Log("SightBlocked");
            targetPosition = hitPoint;
        }
        else {
            targetPosition = defaultPosition;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, speed * Time.deltaTime);
        if (Vector3.Distance(this.transform.position, targetPosition) > 2f) {
            Debug.Log("Vector3.Distance");
            
        }
        //transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(25f, 25f, 25f), speed * Time.deltaTime);
    }

    bool SightBlocked(out Vector3 hitPoint) {
        Vector3 direction = transform.position - transform.parent.position;
        RaycastHit hit;
        Ray ray = new Ray(transform.parent.position, direction);
        Debug.DrawRay(transform.parent.position, direction, Color.blue);
        if (Physics.Raycast(ray, out hit, rayDistance)) {
            Debug.Log("RaycastHit: " + hit.collider.name);
            if (hit.collider.tag == "Walkable") {
                hitPoint = hit.point;
                return true;
            }
        }
        hitPoint = Vector3.zero;
        return false;
    }
}
