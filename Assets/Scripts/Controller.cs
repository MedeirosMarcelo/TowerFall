using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

    public bool canControl = true;
    public bool canMove = true;
    public bool dead = false;
    public bool isGrounded;
    public bool canGrabLedge;
    public GameObject arrow;


    public float runSpeed = 12f;
    public float jumpForce = 25f;
    public float dashForce = 30f;

    WorldMirror worldMirror;

    GroundCollider groundCollider;
    MouseLook charMouseLook;
    MouseLook cameraMouseLook;
    HandsCollider handsCollider;
    GameObject arrowSpawner;

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
        charMouseLook = this.GetComponent<MouseLook>();
        cameraMouseLook = Camera.main.GetComponent<MouseLook>();
        worldMirror = transform.parent.GetComponent<WorldMirror>();
        groundCollider = GetComponentInChildren<GroundCollider>();
        handsCollider = GetComponentInChildren<HandsCollider>();
        Screen.showCursor = false;
        Screen.lockCursor = true;
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
        Move();
        GrabLedge();
        Shoot();
        ClearInput();
    }

    void Update() {
        UpdateInput();
        ShowHideMouseCursor();
    }

    float velocity = 14f;
    float maxAcceleration = 8f;
    Vector3 CalculateVelocityChange(Vector3 inputVector) {
        // Calculate how fast we should be moving
        var relativeVelocity = transform.TransformDirection(inputVector) * velocity;
        // Calcualte the delta velocity
        var velocityChange = relativeVelocity - rigidbody.velocity;
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxAcceleration, maxAcceleration);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxAcceleration, maxAcceleration);
        velocityChange.y = 0;
        return velocityChange;
    }

    void Dash(Vector3 inputVector) {
        Debug.Log("DASH");

        if(inputVector == Vector3.zero) {
            inputVector = Vector3.forward;
        }

        Vector3 dash = inputVector.normalized * dashForce;
        rigidbody.AddForce(Camera.main.transform.TransformDirection(dash), ForceMode.Impulse);
    }

    float maxVelocity = 40f;
    void Move() {
        // Plane movemente (x,z)
        Vector3 planeVelocity = rigidbody.velocity;
        planeVelocity.y = 0f;
        if (planeVelocity.magnitude < maxVelocity) {
            var inputVector = new Vector3(inputHorizontal, 0, inputVertical);
            if (inputDash) {
                Dash(inputVector);
            }
            else {
                if (inputVector.magnitude > 0f) {
                    inputVector = new Vector3(inputHorizontal, 0, inputVertical);
                    rigidbody.AddForce(CalculateVelocityChange(inputVector), ForceMode.VelocityChange);
                }
            }
        } else {
            Debug.Log("Too fast too furious!");
        }
        // Vertical movement (y)
        if (isGrounded) {
            if (inputJump) {
                Debug.Log("JUMP");
                rigidbody.AddForce(Vector3.up* jumpForce, ForceMode.Impulse);
            }
        }
        //	Test ();
    }

    void Shoot() {
    if (inputShoot) {
        //BuildArrow();
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

void ShowHideMouseCursor() {
    if (Input.GetKeyDown(KeyCode.Tab)) {
        if (Screen.showCursor) {
            charMouseLook.enabled = true;
            cameraMouseLook.enabled = true;
            Screen.showCursor = false;
            Screen.lockCursor = true;
        }
        else {
            charMouseLook.enabled = false;
            cameraMouseLook.enabled = false;
            Screen.showCursor = true;
            Screen.lockCursor = false;
        }
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


