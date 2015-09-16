using UnityEngine;
using System.Collections;

public class LoopController : MonoBehaviour {

    public bool debug = false;
    public GameObject loopArea;

    Transform transform;
    Rigidbody rigidbody;
    Vector3 boundMin;
    Vector3 boundMax;

    void Start() {
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody>();

        Vector3 offset = loopArea.transform.localPosition;
        Vector3 halfSize = loopArea.transform.localScale / 2.0f;
        boundMin = offset - halfSize;
        boundMax = offset + halfSize;
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

        LoopAxis(boundMin.x, boundMax.x, ref position.x);
        LoopAxis(boundMin.y, boundMax.y, ref position.y);
        LoopAxis(boundMin.z, boundMax.z, ref position.z);

        if (position != transform.localPosition) {
            transform.localPosition = position;
            Debug.Log(name  + "\nBound Min= " + boundMin.ToString("N2") + " Max=" + boundMax.ToString("N2") + "\n"
                + "Position=" + position.ToString("N2"));
        }
    }

    void FixedUpdate() {
        Loop();
    }
}
