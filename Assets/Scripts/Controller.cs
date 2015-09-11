﻿using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

    public bool canControl = true;
    public bool canMove = true;
    public bool dead = false;
    public bool isGrounded;
    public bool canGrabLedge;
    public GameObject arrow;



    public float runSpeed = 5f;
    public float jumpForce = 25f;
    public float dashForce = 30f;

	WorldMirror worldMirror;
    GroundCollider groundCollider;
    HandsCollider handsCollider;
    GameObject arrowSpawner;

    Vector3 direction;



    State state;

    enum State {
        Idle,
        Run,
        Shoot,
        Jump,
        Crouch,
        Dash,
        GrabLedge,
        Climb,
        SlideDown,
        Fall,
        Die
    }

    void StateMachine() {
        switch (state) {
            case State.Idle:
                break;
            case State.Run:
                break;
            case State.Shoot:
                break;
            case State.Jump:
                break;
            case State.Crouch:
                break;
            case State.Dash:
                break;
            case State.GrabLedge:
                break;
            case State.Climb:
                break;
            case State.SlideDown:
                break;
            case State.Fall:
                break;
            case State.Die:
                break;
        }
    }

    void EnterState(State newState) {
        OnStateExit();
        state = newState;

        switch (state) {
            case State.Idle:
                break;
            case State.Run:
                break;
            case State.Shoot:
                break;
            case State.Jump:
                break;
            case State.Crouch:
                break;
            case State.Dash:
                break;
            case State.GrabLedge:
                break;
            case State.Climb:
                break;
            case State.SlideDown:
                break;
            case State.Fall:
                break;
            case State.Die:
                break;
        }
    }

    void OnStateExit() {
        switch (state) {
            case State.Idle:
                break;
            case State.Run:
                break;
            case State.Shoot:
                break;
            case State.Jump:
                break;
            case State.Crouch:
                break;
            case State.Dash:
                break;
            case State.GrabLedge:
                break;
            case State.Climb:
                break;
            case State.SlideDown:
                break;
            case State.Fall:
                break;
            case State.Die:
                break;
        }
    }

    void Start() {
		worldMirror = transform.parent.GetComponent<WorldMirror> ();
        groundCollider = GetComponentInChildren<GroundCollider>();
        handsCollider = GetComponentInChildren<HandsCollider>();
    }

    // Inputs
    float inputHorizontal = 0f;
    float inputVertical = 0f;
    bool inputShoot = false;
    bool inputDash = false;
    bool inputJump = false;

    // Must be called before each Update to accumulate inputs
    void UpdateInput() {
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
        inputShoot |= Input.GetButtonDown("Fire1");
        inputDash |= Input.GetButtonDown("Fire2");
        inputJump |= Input.GetButtonDown("Jump");
    }

    // Must be called after each Fixed Update to  clear accumulated inputs
    void ClearInput() {
        inputShoot = false;
        inputDash = false;
        inputJump = false;
    }

    void FixedUpdate() {
        StateMachine();
        isGrounded = groundCollider.isGrounded;
        canGrabLedge = handsCollider.canGrabLedge;
        Dash();
        Move();
        GrabLedge();
        Shoot();
        //Loop();
        ClearInput();
    }

    void Update() {
        UpdateInput();
    }

    void Move() {
        direction = new Vector3(inputHorizontal * runSpeed, 0, inputVertical * runSpeed);
        if (isGrounded) {
            if (inputJump) {
                Debug.Log("JUMP");
                rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
        direction = transform.TransformDirection(direction);
        rigidbody.MovePosition(this.transform.position + direction * Time.deltaTime);
        //	Test ();
    }

    public GameObject loopArea;
    bool printLoop = true;
    void Loop () {
        Bounds bounds = loopArea.renderer.bounds;
        // get 
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;
        //modify
        min.y = float.NegativeInfinity;
        max.y = float.PositiveInfinity;
        //set
        bounds.min = min;
        bounds.max = max;

        Vector3 position = rigidbody.position;
        Vector3 nextPosition = position + (rigidbody.velocity * Time.fixedDeltaTime);
        bool bounded = bounds.Contains(nextPosition);

        if (bounded == false) {
            Vector3 relativePosition = (bounds.center - position);
            relativePosition.y = 0f;
            rigidbody.position = position + 2 * relativePosition;
            Debug.Log("Offset=" +relativePosition.ToString("N2"));
        }

        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            printLoop = !printLoop;
        }
        if (printLoop) {
            Debug.Log("Bounds Min= " + min.ToString("N2") + " Max=" + max.ToString("N2"));
            Debug.Log("Position=" + rigidbody.position.ToString("N2"));
            Debug.Log("Next Position=" + rigidbody.position.ToString("N2"));
            Debug.Log("Contains=" + bounded.ToString());
        }

    }

    void Shoot() {
        if (inputShoot) {
            BuildArrow();
        }
    }

	void BuildArrow() {
        Vector3 arrowPosition = transform.Find("Main Camera").Find("ArrowSpawner").position;
		Quaternion arrowRotation = transform.Find("Main Camera").Find("ArrowSpawner").rotation;
		GameObject newArrow = worldMirror.InstantiateAll(arrow, arrowPosition, arrowRotation);
		newArrow.GetComponent<Arrow>().shot = true;
    }
	
    void GrabLedge() {
        if (canGrabLedge) {
            if (inputVertical > 0) {
                if (inputJump) {
                    Debug.Log("JUMP");
                    rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }
                else {
                    rigidbody.useGravity = false;
                    rigidbody.velocity = Vector3.zero;
                    Debug.Log("HANGING");
                }
            }
            else {
                ReleaseLedge();
                // Debug.Log("RELEASE");
            }
        }
        else {
            ReleaseLedge();
            // Debug.Log("RELEASE");
        }
    }

    void ReleaseLedge() {
        rigidbody.useGravity = true;
    }

    void Dash() {
        if (inputDash) {
            Debug.Log("DASH");
            float vertical = (inputVertical == 0f) ? 1f : inputVertical;
            Vector3 dash = new Vector3(inputHorizontal, 0, vertical).normalized * dashForce;
            rigidbody.AddForce(Camera.main.transform.TransformDirection(dash), ForceMode.Impulse);
        }
    }

    void Test() {
        // Get the velocity
        Vector3 horizontalMove = rigidbody.velocity;
        // Don't use the vertical velocity
        horizontalMove.y = 0;
        // Calculate the approximate distance that will be traversed
        float distance = horizontalMove.magnitude * Time.fixedDeltaTime;
        // Normalize horizontalMove since it should be used to indicate direction
        horizontalMove.Normalize();
        RaycastHit hit;

        // Check if the body's current velocity will result in a collision
        if (rigidbody.SweepTest(horizontalMove, out hit, distance)) {
            // If so, stop the movement
            rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0);
        }
    }
}


