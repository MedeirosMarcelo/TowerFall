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
        get { return target.transform.position; }
        set { target.transform.position = value; }
    }

    void Start() {
        layerMask = LayerMask.GetMask("Wall");
        if (target == null) {
            target = transform.parent.gameObject;
        }
    }

    void LateUpdate() {
        Vector3 hit;

        // this is the current camera direction relative to target we are following
        Vector3 direction = (position - targetPosition).normalized;
        NextPosition(targetPosition, direction, out hit);

        // hit now contains the global position we should put the camera, yet we must compensate camera near 
        hit -= (direction * camera.nearClipPlane);

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
