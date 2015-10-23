using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

    Transform cameraTransform { get; set; }

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

    void Start() {
        cameraTransform = GetComponentInChildren<Camera>().transform;
        horizontal = 0f;
        vertical = 0f;
        shoot = false;
        dash = false;
        jump = false;
        HideCursor();
        // Look around
        // Make the rigid body not change rotation
        var rigidBody = GetComponent<Rigidbody>();
        if (rigidbody) {
            rigidbody.freezeRotation = true;
        }
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

    public void InputUpdate() {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        lookHorizontal = Input.GetAxis("Look Horizontal");
        lookVertical = Input.GetAxis("Look Vertical");
        Debug.Log("lookVertical =" + lookVertical);



        // AccumulateButtons
        shoot |= Input.GetButtonDown("Fire1");
        dash |= Input.GetButtonDown("Fire2");
        jump |= Input.GetButtonDown("Jump");

        UpdateCursor();
        Look();
    }

    public void PostFixedUpdate() {
        shoot = false;
        dash = false;
        jump = false;
    }

    void Look() {
        if (mouseLook) {
            rotationY += lookVertical * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            //Camera
            cameraTransform.localEulerAngles = new Vector3(-rotationY, cameraTransform.localEulerAngles.y, 0);

            //player
            float rotationX = transform.localEulerAngles.y + lookHorizontal * sensitivityX;
            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);

            //Debug.Log("Look X=" + rotationX + " Y=" + rotationY);
        }
    }
}


