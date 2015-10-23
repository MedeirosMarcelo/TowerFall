using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class CharacterInput {

    Character character;

    public float horizontal { get; private set; }
    public float vertical { get; private set; }
    public float lookHorizontal { get; private set; }
    public float lookVertical { get; private set; }

    public bool shoot { get; private set; }
    public bool dash { get; private set; }
    public bool jump { get; private set; }

    float sensitivityX = 15F;
    float sensitivityY = 15F;
    float minimumX = -360F;
    float maximumX = 360F;
    float minimumY = -60F;
    float maximumY = 60F;
    float rotationY = 0F;
    bool mouseLook;

    public Vector3 vector {
        get { return new Vector3(horizontal, 0, vertical); }
    }

    public CharacterInput(Character character) {
        this.character = character;
        horizontal = 0f;
        vertical = 0f;
        shoot = false;
        dash = false;
        jump = false;
        HideCursor();

        // Make the rigid body not change rotation
        character.rigidbody.freezeRotation = true;
    }

    public void Update() {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        lookHorizontal = Input.GetAxis("Look Horizontal");
        lookVertical = Input.GetAxis("Look Vertical");

        // AccumulateButtons
        shoot |= Input.GetButtonDown("Fire1");
        dash |= Input.GetButtonDown("Fire2");
        jump |= Input.GetButtonDown("Jump");

        UpdateCursor();
        Look();
    }

    public void FixedUpdate() {
        shoot = false;
        dash = false;
        jump = false;
    }

    void ShowCursor() {
        mouseLook = false;
        Screen.showCursor = true;
        Screen.lockCursor = false;
    }

    void HideCursor() {
        mouseLook = true;
        Screen.showCursor = false;
        Screen.lockCursor = true;

    }

    void UpdateCursor() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (Screen.showCursor) { HideCursor(); }
            else { ShowCursor(); }
        }
    }

    void Look() {
        if (mouseLook) {
            rotationY += lookVertical * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            //Camera
            var camTransform = character.charCamera.transform;
            camTransform.localEulerAngles = new Vector3(-rotationY, camTransform.localEulerAngles.y, 0);

            //player
            var charTransform = character.transform;
            float rotationX = charTransform.localEulerAngles.y + lookHorizontal * sensitivityX;
            charTransform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
    }
}


