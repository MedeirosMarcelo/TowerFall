using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {

    public GameObject owner;
    public float speed = 2f;
    public float maxDistance = -5.5f;
    public float rayDistance = 5f;
    bool adjustPosition;

    void Start() {

    }

    void Update() {
        AdjustPosition();
    }

    bool SightBlocked() {
        Vector3 direction = transform.parent.position - transform.position;
        RaycastHit hit;
        Ray ray = new Ray(this.transform.position, direction);
        Debug.DrawRay(this.transform.position, direction, Color.blue);
        if (Physics.Raycast(ray, out hit, rayDistance)) {
            Debug.Log(hit.collider.name);
            if (hit.collider.name == "Wall") {
                Debug.Log("SightBlocked");
                return true;
            }
        }
        return false;
    }

    void AdjustPosition() {

        Vector3 origin = this.transform.localPosition;
        Vector3 positionNear = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        Vector3 positionFar = new Vector3(transform.localPosition.x, transform.localPosition.y, maxDistance);

        if (SightBlocked()) {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Lerp(transform.localPosition.z, 0, speed * Time.deltaTime));
        //    transform.localPosition = Vector3.Lerp(origin, positionNear, speed);
        }
        else {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Lerp(transform.localPosition.z, maxDistance, speed * Time.deltaTime));
        //    transform.localPosition = Vector3.Lerp(origin, positionFar, speed);
        }
    }

    void OnTriggerExit(Collider col) {

    }
}
