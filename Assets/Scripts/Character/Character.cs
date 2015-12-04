using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : Reflectable {

    public static readonly int group = (int)NetworkGroup.Character;
    // Internals
    // exposing getters for internals 
    public CharacterController controller { get; private set; }
    public CharacterInput input { get; private set; }
    public CharacterArrows arrows { get; private set; }
    public CharacterFsm fsm { get; private set; }

    // Children
    public Camera charCamera { get; private set; }
    public GroundCollider groundCollider { get; private set; }
    public HandsCollider handsCollider { get; private set; }
    public GameObject head { get; private set; }
    public GameObject feet { get; private set; }
    public GameObject arrowSpawner { get; private set; }

    // World
    //Network
    public bool isMine { get { return networkView.isMine; } }
    public bool isNotMine { get { return !networkView.isMine; } }

    //Input lock flags 
    public bool mouseLookEnabled { get; set; }
    public bool keyboardMovementEnabled { get; set; }


    public int playerNumber;
    public int health = 1;
    public GameObject basicArrow;
    public CharacterInput.Type inputType;



    void OnValidate() {
        if (input != null) { input.type = inputType; }
    }

    void Update() {
        if (isNotMine) {
            return;
        }
        input.Update();
    }


    void FixedUpdate() {
        if (Network.isServer && !waitingDestruction) {
            DetectJumpKill();
            return;
        }
        if (isNotMine) {
            return;
        }
        controller.Look();
        fsm.FixedUpdate(Time.deltaTime);
        arrows.FixedUpdate();

        // Input must be last here
        input.FixedUpdate();
    }

    void Start() {
        Debug.Log("Char Start");
        this.playerNumber = 0;
        charCamera = GetComponentInChildren<Camera>();

        if (isNotMine) {
            charCamera.gameObject.SetActive(false);
        }
        handsCollider = GetComponentInChildren<HandsCollider>();
        groundCollider = GetComponentInChildren<GroundCollider>();
        head = transform.FindChild("Head").gameObject;
        feet = transform.FindChild("Feet").gameObject;
        arrowSpawner = transform.FindChild("ArrowSpawner").gameObject;

        input = new CharacterInput(this);
        controller = new CharacterController(this);
        arrows = new CharacterArrows(this);
        fsm = new CharacterFsm(this);

        mouseLookEnabled = true;
        keyboardMovementEnabled = true;
    }

    public void TakeHit(DamageDealer damager) {
        Debug.Log("Take hit");

        if (Network.isClient) {
            return;
        }
        int dmg = damager.GetComponent<DamageDealer>().damage;
        TakeDamage(dmg);
   }

    void MonitorHealth() {
        if (health <= 0) {
            //gameManager.Respawn(playerNumber);
            Destroy();
        }
    }



    private int headMask;
    private float headMaxDistance = 0.2f;
    private float headRadius = 0.25f;
    private float headKillForce = 25f;

    void DetectJumpKill() {
        headMask = LayerMask.GetMask("Feet");
        Vector3 position = head.transform.position;
        Vector3 direction = Vector3.up;
        RaycastHit hit;
        Ray ray = new Ray(position, direction);

        Debug.DrawRay(position, direction * headMaxDistance, Color.yellow);
        Debug.DrawRay(position + Vector3.forward * headRadius, direction * headMaxDistance, Color.yellow);
        Debug.DrawRay(position + Vector3.right * headRadius, direction * headMaxDistance, Color.yellow);
        Debug.DrawRay(position + Vector3.back * headRadius, direction * headMaxDistance, Color.yellow);
        Debug.DrawRay(position + Vector3.left * headRadius, direction * headMaxDistance, Color.yellow);

        if (Physics.SphereCast(ray, headRadius, out hit, headMaxDistance, headMask)) {
            Debug.Log("Head Hit" + hit.collider.name);
            var parent = hit.collider.transform.parent;
            if (parent.tag == "Player") {
                Debug.Log("Jump kill!");
                Destroy();
                parent.rigidbody.AddForce(Vector3.up * headKillForce, ForceMode.Impulse);

            }
        }
    }


    private bool waitingDestruction = false;
    [RPC]
    void Destroy() {
        if(Network.isServer) {
            waitingDestruction = true;
            Debug.Log("Destroy Character " + GetInstanceID());
            Network.RemoveRPCs(networkView.owner, group);
            networkView.RPC("Destroy", RPCMode.Others);
            gameObject.SetActive(false);
            return;
        }
        if (networkView.isMine) {
            Debug.Log("Destroy Character " + GetInstanceID());
            Network.Destroy(gameObject);
            return;
        }
    }

    [RPC]
    void TakeDamage(int damage) {
        Debug.Log(health + " " + damage);
        health -= damage;
        MonitorHealth();

        if (Network.isServer && health > 0) {
            networkView.RPC("TakeDamage", RPCMode.Others, damage);
        }
    }
    [RPC]
    void StoreArrow(int type) {
        if (isMine && (arrows.stack.Count < arrows.maxArrows)) {
            Debug.Log("Got Arrow type = " + (ArrowType)type);
            arrows.stack.Push((ArrowType)type);
        }
    }
}
