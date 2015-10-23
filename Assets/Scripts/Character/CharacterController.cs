using UnityEngine;
using System;
using System.Collections;


[Serializable]
public class CharacterController {

    Character character;

    float runSpeed = 14f;
    float dashForce = 30f;
    float maxAcceleration = 8f;
    float jumpForce = 25f;

    public CharacterController(Character character) {
        this.character = character;
    }

    public void FixedUpdate() {
        Move();
        Dash();
        Jump();
    }
 

    void Move() {
        // Calculate how fast we should be moving
        var relativeVelocity = character.transform.TransformDirection(character.input.vector) * runSpeed;
        // Calcualte the delta velocity
        var velocityChange = relativeVelocity - character.rigidbody.velocity;
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxAcceleration, maxAcceleration);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxAcceleration, maxAcceleration);
        velocityChange.y = 0;
        character.rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void Dash() {
        if (character.input.dash) {
            Debug.Log("dash");
            Vector3 dash = character.input.vector.normalized * dashForce;
            character.rigidbody.AddForce(Camera.main.transform.TransformDirection(dash), ForceMode.Impulse);
        }
    }

    void Jump() {
        if (character.input.jump) {
            Debug.Log("jump");
            character.rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    bool SweepDirection(Vector3 direction, out Vector3 directionHit) {
        direction.Normalize();
        float distance = character.collider.bounds.size.z * 0.5f;
        RaycastHit hit;
        if (character.rigidbody.SweepTest(direction, out hit, distance)) {
            Debug.Log(hit.distance + "mts distance to obstacle");
            directionHit = direction;
            return true;
        }
        else {
            directionHit = Vector3.one;
            return false;
        }
    }
}
