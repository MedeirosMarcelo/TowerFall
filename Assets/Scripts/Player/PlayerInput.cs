using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

    Camera playerCamera { get; set; }
    MouseLook charMouseLook { get; set; }
    MouseLook cameraMouseLook { get; set; }

    public float horizontal { get; private set; }
    public float vertical { get; private set; }
    public bool shoot { get; private set; }
    public bool dash { get; private set; }
    public bool jump { get; private set; }

    public Vector3 vector {
        get { return new Vector3(horizontal, 0, vertical); }
    }

    void ShowCursor() {
        charMouseLook.enabled = false;
        cameraMouseLook.enabled = false;
        Screen.showCursor = true;
        Screen.lockCursor = false;
    }

    void HideCursor() {
        charMouseLook.enabled = true;
        cameraMouseLook.enabled = true;
        Screen.showCursor = false;
        Screen.lockCursor = true;
    }

    void UpdateCursor() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (Screen.showCursor) {
                HideCursor();
            }
            else {
                ShowCursor();
            }
        }
    }


    void AccumulateButtons() {
            shoot |= Input.GetButtonDown("Fire1");
            dash |= Input.GetButtonDown("Fire2");
            jump |= Input.GetButtonDown("Jump");
    }

    void Start() {
        playerCamera = transform.Find("Main Camera").GetComponent<Camera>();
        charMouseLook = GetComponent<MouseLook>();
        cameraMouseLook = playerCamera.GetComponent<MouseLook>();
        horizontal = 0f;
        vertical = 0f;
        shoot = false;
        dash = false;
        jump = false;
        HideCursor();
    }

    public void PreUpdate() {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        AccumulateButtons();
        UpdateCursor();
    }

    public void PostFixedUpdate() {
        shoot = false;
        dash = false;
        jump = false;
    }
}


