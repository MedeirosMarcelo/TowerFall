using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    PlayerInput input { get; set; }

    [SerializeField]
    float runSpeed = 14f;
    [SerializeField]
    float dashForce = 30f;
    [SerializeField]
    float maxAcceleration = 8f;
    [SerializeField]
    float jumpForce = 25f;

    void UpdateVelocity() {
        // Calculate how fast we should be moving
        var relativeVelocity = transform.TransformDirection(input.vector) * runSpeed;
        // Calcualte the delta velocity
        var velocityChange = relativeVelocity - rigidbody.velocity;
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxAcceleration, maxAcceleration);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxAcceleration, maxAcceleration);
        velocityChange.y = 0;
        rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void UpdateDash() {
        if (input.dash) {
            Debug.Log("dash");
            Vector3 dash = input.vector.normalized * dashForce;
            rigidbody.AddForce(Camera.main.transform.TransformDirection(dash), ForceMode.Impulse);
        }
    }

    void UpdateJump() {
        if (input.jump) {
            Debug.Log("jump");
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    bool SweepDirection(Vector3 direction, out Vector3 directionHit) {
        direction.Normalize();
        float distance = collider.bounds.size.z * 0.5f;
        RaycastHit hit;
        if (rigidbody.SweepTest(direction, out hit, distance)) {
            Debug.Log(hit.distance + "mts distance to obstacle");
            directionHit = direction;
            return true;
        }
        else {
            directionHit = Vector3.one;
            return false;
        }
    }

    void Start() {
        input = GetComponent<PlayerInput>();
    }

    void Update() {
        input.PreUpdate();
    }

    void FixedUpdate() {
        UpdateVelocity();
        UpdateDash();
        UpdateJump();
        input.PostFixedUpdate();
    }
}
