using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : Reflectable {

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
    public NetworkView netRigidBody { get; private set; }
    public NetworkView netView { get; private set; }
    public bool isMine { get { return netView.isMine; } }
    public bool isNotMine { get { return !netView.isMine; } }

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
        controller.Update();
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
        netView = GetComponent<NetworkView>();

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
    }

    public void TakeHit(DamageDealer damager) {
        if (Network.isClient) {
            /* Take hit is a server thing */
            return;
        }
        /*
        if (fsm.state == CharacterFsm.State.Dash) {
            PickUpItem(damager);
        }
        else */

        {
            int dmg = damager.GetComponent<DamageDealer>().damage;
            TakeDamage(dmg);
        }
    }

    void MonitorHealth() {
        if (health <= 0) {
            //gameManager.Respawn(playerNumber);
            Destroy();
        }
    }
    /*
    void UseItem(Item item) {
    }

    public void PickUpItem(Item item) {
        if (item.tag == "Arrow") {
            DamageDealer arrow = (DamageDealer)item;
            if (!arrow.alive) {
                if (arrows.arrowList.Count < 7) {
                    Arrow arrowItem = (Arrow)item;
                    arrows.StoreArrow(arrowItem.type);
                }
                item.PickUp();
            }
        }
        else if (item.tag == "Item") {
            UseItem(item);
            item.PickUp();
        }
        else {
            Debug.LogError("PickUpItem - Item has invalid tag.");
        }
    }
    */


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
        if (Network.isServer) {
            Network.RemoveRPCs(netView.viewID);
            Network.Destroy(netView.viewID);
        }
        else {
            netView.RPC("Destroy", RPCMode.Server);
        }
    }

    [RPC]
    void TakeDamage(int damage) {
        Debug.Log(health + " " + damage);
        health -= damage;
        MonitorHealth();

        if (Network.isServer && health > 0) {
            netView.RPC("TakeDamage", RPCMode.Others, damage);
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
