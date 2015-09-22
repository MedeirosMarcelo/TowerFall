using UnityEngine;
using System.Collections;

public class LoopController : MonoBehaviour {

    public GameObject Reflection;

    Transform transform;
    Rigidbody rigidbody;
    WorldMirror worldMirror;
    Bounds bounds;

    void Start() {
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody>();
        worldMirror = GetComponentInParent<WorldMirror>();
        bounds = worldMirror.bounds;
        Debug.Log(name + bounds);
    }

    void LoopAxis(float min, float max, ref float position) {
        float size = max - min;
        if (position > max) {
            position -= size;
        }
        else if (position < min) {
            position += size;
        }
    }

    void Loop() {
        Vector3 position = transform.localPosition;

        LoopAxis(bounds.min.x, bounds.max.x, ref position.x);
        LoopAxis(bounds.min.y, bounds.max.y, ref position.y);
        LoopAxis(bounds.min.z, bounds.max.z, ref position.z);

        if (position != transform.localPosition) {
            transform.localPosition = position;
            Debug.Log(name + "\n"
                + "Bound Min= " + bounds.min.ToString("N2") + " Max=" + bounds.max.ToString("N2") + "\n"
                + "Position=" + position.ToString("N2"));
        }
    }

    void FixedUpdate() {
        if (rigidbody.isKinematic == false) {
            Loop();
        }
    }
}
