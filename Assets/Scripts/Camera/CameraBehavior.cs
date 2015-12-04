using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {

    public GameObject target;
    private float lerpFraction = 0.5f;
    private float maxDistance = 5.5f;
    private int layerMask;


    private Vector3 position {
        get { return transform.position; }
        set { transform.position = value; }
    }
    private Vector3 targetPosition {
        get { return target.transform.position + (Vector3.up * 0.9f); }
    }
    private float near {
        get { return camera.nearClipPlane + 1f; }
    }


    void Start() {
        layerMask = LayerMask.GetMask("Wall");
        if (target == null) {
            target = transform.parent.gameObject;
        }
    }

    private float sideAngle = 30;
    private float minDownAngle = 60;

    void LateUpdate() {
        Vector3 hit;

        Vector3 direction = -transform.forward;
        direction = Quaternion.AngleAxis(-sideAngle, transform.up) * direction;

        /* We limit camera when lloging up so it doesnt go to characters feet */
        float angle = Vector3.Angle(Vector3.up, transform.forward);
        if (angle < minDownAngle) {
            direction = Quaternion.AngleAxis(minDownAngle - angle, transform.right) * direction;
        }

        NextPosition(targetPosition, direction, out hit);

        // hit now contains the global position we should put the camera, yet we must compensate camera near 
        hit -= (direction * near);

        // if hit position is closer to target just jump to it otherwise lerp to it
        if ((hit - targetPosition).magnitude < (position - targetPosition).magnitude) {
            position = hit;
        }
        else {
            position = Vector3.Lerp(position, hit, lerpFraction);
        }
    }


    void NextPosition(Vector3 origin, Vector3 direction, out Vector3 hitPosition) {
        Vector3 maxPosition = direction * maxDistance;
        RaycastHit hit;
        Ray ray = new Ray(origin, direction);
        Debug.DrawRay(origin, maxPosition, Color.blue);
        if (Physics.Raycast(ray, out hit, maxDistance, layerMask)) {
            if (hit.collider.tag == "Walkable") {
                hitPosition = hit.point;
                return;
            }
        }
        hitPosition = origin + maxPosition;
    }
}
