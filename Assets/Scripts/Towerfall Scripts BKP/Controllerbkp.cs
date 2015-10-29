using UnityEngine;
using System.Collections;

public class Controllerbkp : MonoBehaviour {

    public bool canControl = true;
    public bool canMove = true;
    public bool dead = false;
    public bool isGrounded;
    public bool canGrabLedge;
    public GameObject arrow;
    public GameObject empty;

    public float runSpeed = 14f;
    public float jumpForce = 25f;
    public float dashForce = 30f;

    WorldMirror worldMirror;

    Camera playerCamera;
    GroundCollider groundCollider;
    MouseLook charMouseLook;
    MouseLook cameraMouseLook;
    HandsCollider handsCollider;
    GameObject arrowSpawner;
    int layermask = 1 << 8;

    public State state;

    public enum State {
        Idle,
        Move,
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
            case State.Move:
                Move();
                GrabLedge();
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
            case State.Move:
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
            case State.Move:
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
        playerCamera = transform.Find("Main Camera").GetComponent<Camera>();
        charMouseLook = this.GetComponent<MouseLook>();
        cameraMouseLook = playerCamera.GetComponent<MouseLook>();
        worldMirror = transform.parent.GetComponent<WorldMirror>();

        groundCollider = GetComponentInChildren<GroundCollider>();
        handsCollider = GetComponentInChildren<HandsCollider>();
        Screen.showCursor = false;
        Screen.lockCursor = true;
        state = State.Move;
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
        Shoot();
        Jump();
        ClearInput();
    }

    void Update() {
        UpdateInput();
        ShowHideMouseCursor();
    }

    void Dash(Vector3 inputVector) {
        Debug.Log("DASH");

        if (inputVector == Vector3.zero) {
            inputVector = Vector3.forward;
        }

        Vector3 dash = inputVector.normalized * dashForce;
        rigidbody.AddForce(Camera.main.transform.TransformDirection(dash), ForceMode.Impulse);
    }

    float maxVelocity = 40f;
    void Move() {
        // Plane movement (x,z)
        Vector3 planeVelocity = rigidbody.velocity;
        planeVelocity.y = 0f;

        var inputVector = new Vector3(inputHorizontal, 0, inputVertical);
        if (inputDash) {
            Dash(inputVector);
        }
        else {
            Vector3 direction = transform.TransformDirection(transform.forward);
            inputVector = new Vector3(inputHorizontal * direction.x, 0, inputVertical * direction.y);
            
           // direction = new Vector3(direction.x * inputHorizontal, 0, direction.z * inputVertical);

            /*Vector3 newVelocity = inputVector;
            Vector3 directionHit;
            if (SweepDirection(newVelocity, out directionHit)) {
                Vector3 tempDirection = directionHit;
                directionHit = InvertVector3(directionHit);
                directionHit.Normalize();
                //      newVelocity.Scale(directionHit);
                Debug.Log("Old " + tempDirection + " New " + directionHit + " Result " + newVelocity);
                Debug.Log("CLAMP");
                newVelocity = new Vector3(0, rigidbody.velocity.y, 0);
            }*/
            /*
            RaycastHit hit;
            Ray ray = new Ray(this.transform.position, newVelocity.normalized);
            Debug.DrawRay(this.transform.position, newVelocity.normalized * 5f, Color.red);
            if (Physics.Raycast(ray, 5f, layermask)) {
                Debug.Log("HIT");
                newVelocity = Vector3.zero;
            }
            */
            inputVector *= runSpeed;
            rigidbody.MovePosition(transform.position + inputVector * Time.deltaTime);
        }

        // Vertical movement (y)
    }

    bool SweepDirection(Vector3 direction, out Vector3 directionHit) {
        direction.Normalize();
        float distance = collider.bounds.size.z * 0.6f;
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

    Vector3 InvertVector3(Vector3 v) {
        float[] array = new float[3];
        array[0] = v.x;
        array[1] = v.y;
        array[2] = v.z;

        for (int i = 0; i < 3; i++) {
            if (array[i] != 0) array[i] = 0;
            else array[i] = 1;
        }
        Vector3 result = new Vector3(array[0], array[1], array[2]);
        return result;
    }

    void Jump() {
        if ((state == State.Move && isGrounded) || 
             state == State.GrabLedge) {
            if (inputJump) {
                rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    void GrabLedge() {
        if (canGrabLedge) {
            rigidbody.useGravity = false;
            rigidbody.velocity = Vector3.zero;
            EnterState(State.GrabLedge);
            Debug.Log("HANGING");
        }
        else {
            ReleaseLedge();
            Debug.Log("RELEASE");
        }
    }

    void ReleaseLedge() {
        rigidbody.useGravity = true;
        EnterState(State.Move);
    }


    void Shoot() {
        if ((state == State.Move ||
            state == State.Jump) && isGrounded) {
            if (inputShoot) {
                BuildArrow();
            }
        }
    }

    void BuildArrow() {
        Vector3 arrowPosition = transform.Find("ArrowSpawner").position;
        Quaternion arrowRotation = transform.Find("ArrowSpawner").rotation;
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));
        RaycastHit hit;
        GameObject newArrow = worldMirror.InstantiateAll(arrow, arrowPosition, arrowRotation);
        if (Physics.Raycast(ray, out hit)) {
            newArrow.transform.LookAt(hit.point);
        }
        else {
            newArrow.transform.LookAt(ray.GetPoint(15));
        }
        newArrow.GetComponent<Arrow>().Shoot(this.gameObject.GetComponent<Character>());
    }

    /*
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
                rigidbody.useGravity = false;
                rigidbody.velocity = Vector3.zero;
                Debug.Log("HANGING");
            }
        }
        else {
            ReleaseLedge();
            // Debug.Log("RELEASE");
        }
    }*/

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


