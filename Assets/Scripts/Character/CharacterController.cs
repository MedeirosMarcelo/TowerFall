using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class CharacterController {

    Character character;
    GroundCollider groundCollider;
    HandsCollider handsCollider;

    float runSpeed = 14f;
    float dashForce = 30f;
    float maxAcceleration = 8f;
    float jumpForce = 25f;

    public bool isGrounded { get { return groundCollider.isGrounded; } }
    public bool canGrabLedge { get { return handsCollider.canGrabLedge; } }


    public CharacterController(Character character) {
        this.character = character;
        groundCollider = character.groundCollider;
        handsCollider = character.handsCollider;

        // Make the rigid body not change rotation
        character.rigidbody.freezeRotation = true;
    }

    public void AirMove() {
        Move();
    }

    public void Move() {
        // Calculate how fast we should be moving
        var relativeVelocity = character.transform.TransformDirection(character.input.vector) * runSpeed;
        // Calcualte the delta velocity
        var velocityChange = relativeVelocity - character.rigidbody.velocity;
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxAcceleration, maxAcceleration);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxAcceleration, maxAcceleration);
        velocityChange.y = 0;
        character.rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    public void Dodge() {
        Debug.Log("Dodge");
        Vector3 dash = character.input.vector.normalized;
        if (dash == Vector3.zero) { dash = Vector3.forward; }
        character.rigidbody.AddForce(character.charCamera.transform.TransformDirection(dash * dashForce), ForceMode.Impulse);
    }

    public void Jump() {
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

    float sensitivityX = 15F;
    float sensitivityY = 15F;
    float minimumX = -360F;
    float maximumX = 360F;
    float minimumY = -60F;
    float maximumY = 60F;
    float rotationY = 0F;


    public void Look() {
        rotationY += character.input.lookVertical * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        //Camera
        var camTransform = character.charCamera.transform;
        camTransform.localEulerAngles = new Vector3(-rotationY, camTransform.localEulerAngles.y, 0);

        //player
        var charTransform = character.transform;
        float rotationX = charTransform.localEulerAngles.y + character.input.lookHorizontal * sensitivityX;
        charTransform.localEulerAngles = new Vector3(0, rotationX, 0);
    }
}
