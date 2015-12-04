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
    public GameObject feet { get; private set; }
    public GameObject arrowSpawner { get; private set; }

    // World
    //public WorldMirror worldMirror { get; private set; }

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
        if (isNotMine) {
            return;
        }

        controller.Look();
        fsm.FixedUpdate(Time.deltaTime);
        arrows.FixedUpdate();

        // Input must be last here
        input.FixedUpdate();
        DetectJumpKill();
    }

    public void Create(int playerNumber) {
        Start();
        this.playerNumber = playerNumber;
    }

    void Start() {
        Debug.Log("Char Start");
        this.playerNumber = 0;
        charCamera = GetComponentInChildren<Camera>();

        if (isNotMine) {
            charCamera.gameObject.SetActive(false);
        }
        //worldMirror = GetComponentInParent<WorldMirror>();
        handsCollider = GetComponentInChildren<HandsCollider>();
        groundCollider = GetComponentInChildren<GroundCollider>();
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

    void DetectJumpKill() {
        int layerMask = 1 << 9;
        Vector3 point1 = feet.transform.position;
        Vector3 point2 = point1 + Vector3.down * 0.2f;
        //float radius = 0.25f;
        Vector3 direction = Vector3.down;
        float maxDistance = 0.2f;
        //Debug.DrawLine(point1, point2, Color.yellow);
        Debug.DrawRay(point1, direction * maxDistance, Color.yellow);
        RaycastHit hit;
        Ray ray = new Ray(point1, direction);
        if (Physics.Raycast(ray, out hit, maxDistance, layerMask)) {
            //if (Physics.CapsuleCast(point1, point2, radius, direction, out hit, maxDistance, layerMask)) {
            Debug.Log("JUMP HIT " + hit.collider.name);
            if (hit.collider.transform.parent.tag == "Player") {
                Debug.Log("JUMP KILL!");
                rigidbody.AddForce(Vector3.up * 20f, ForceMode.Impulse);
                Destroy(hit.collider.transform.parent.gameObject);

            }
        }
    }

    [RPC]
    void Destroy() {
        if(Network.isServer) {
            Debug.Log("Destroy Character " + GetInstanceID());
            Network.RemoveRPCs(base.networkView.owner, group);
            base.networkView.RPC("Destroy", RPCMode.Others);
            return;
        }
        if (base.networkView.isMine) {
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
