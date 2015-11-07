using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {

    public GameObject owner;
    float speed = 7f;
    public float maxDistance = -5.5f;
    public float rayDistance = 5f;
    Vector3 cameraPosition = new Vector3(1.71f, 1.62f, -5.51f);

    void LateUpdate() {
        AdjustPosition(cameraPosition);
    }

    void AdjustPosition(Vector3 defaultPosition) {

        Vector3 hitPoint;
        Vector3 targetPosition;

        if (SightBlocked(out hitPoint)) {
            Debug.Log("SightBlocked" + " " + hitPoint);
            targetPosition = hitPoint;
        }
        else {
            targetPosition = defaultPosition;
            Debug.Log("NotBlocked" + " " + defaultPosition);
        }

        if (Vector3.Distance(transform.position, targetPosition) > 2f) {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, speed * Time.deltaTime);
        }
    }

    bool SightBlocked(out Vector3 hitPoint) {
        Vector3 origin = owner.transform.position;
        Vector3 direction = transform.position - origin;
        RaycastHit hit;
        Ray ray = new Ray(origin, direction);
        Debug.DrawRay(origin, direction, Color.blue);
        if (Physics.Raycast(ray, out hit, rayDistance)) {
            Debug.Log("RaycastHit: " + hit.collider.name);
            if (hit.collider.tag == "Walkable") {
                hitPoint = transform.InverseTransformPoint(hit.point);
                hitPoint = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z -1);
                return true;
            }
        }
        hitPoint = Vector3.zero;
        return false;
    }

    void OnTriggerEnter(Collider col) {
        if (true){
            
        }
    }
}
