using UnityEngine;
using System.Collections;

public class LoopController : MonoBehaviour {

    public bool debug = false;
    public GameObject loopArea;

    Transform transform;
    Rigidbody rigidbody;
    Mover mover;

    void Start() {
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody>();
        mover = GetComponent<Mover>();
    }
    void LoopAxis(float min, float max, ref float position, ref float velocity) {
        float nextPosition = position + velocity * Time.fixedDeltaTime;
        if (nextPosition > max) {
            position = min;
            velocity = (nextPosition - max) / Time.fixedDeltaTime;
            Debug.Log("Loop Max");
        }
        else if (nextPosition < min) {
            position = max;
            velocity = (nextPosition - min) / Time.fixedDeltaTime;
            Debug.Log("Loop Min");
        }
    }

    void Loop() {
        Bounds bounds = loopArea.renderer.bounds;
        Vector3 boundMin = bounds.min;
        Vector3 boundMax = bounds.max;
        Vector3 newPosition = transform.position;
        Vector3 newVelocity = rigidbody.velocity;

        LoopAxis(boundMin.x, boundMax.x, ref newPosition.x, ref newVelocity.x);
        LoopAxis(boundMin.y, boundMax.y, ref newPosition.y, ref newVelocity.y);
        LoopAxis(boundMin.z, boundMax.z, ref newPosition.z, ref newVelocity.z);

        if (newPosition != transform.position) {
            transform.position = newPosition;
            rigidbody.velocity = newVelocity;

            Debug.Log("Bound Min= " + boundMin.ToString("N2") + "\n"
                + "bound Max=" + boundMax.ToString("N2") + "\n"
                + "Position=" + newPosition.ToString("N2") + "\n"
                + "Velocity=" + newVelocity.ToString("N2"));
        }
    }

    void FixedUpdate() {
        Loop();
    }
}
