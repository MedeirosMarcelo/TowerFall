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
    //GameManager gameManager;

    //Network
    public NetworkView net { get; private set; }
    public bool isMine { get { return net.isMine; } }
    public bool isNotMine { get { return !net.isMine; } }

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
        this.playerNumber = 0;
        charCamera = GetComponentInChildren<Camera>();
        net = GetComponent<NetworkView>();

        if (!net.isMine) {
            charCamera.gameObject.SetActive(false);
        }
        //worldMirror = GetComponentInParent<WorldMirror>();
        handsCollider = GetComponentInChildren<HandsCollider>();
        groundCollider = GetComponentInChildren<GroundCollider>();
        feet = transform.FindChild("Feet").gameObject;
        arrowSpawner = transform.FindChild("ArrowSpawner").gameObject;

        input = new CharacterInput(this);
        controller = new CharacterController(this, input);
        arrows = new CharacterArrows(this, input, arrowSpawner);
        fsm = new CharacterFsm(this, input, controller);
    }

    void UseItem(Item item) {

    }

    public void TakeHit(DamageDealer damager) {
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

    void TakeDamage(int damage) {
        Debug.Log(health + " " + damage);
        health -= damage;
        MonitorHealth();
    }

    void MonitorHealth() {
        if (health <= 0) {
            //gameManager.Respawn(playerNumber);
            Destroy(this.gameObject);
        }
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

    void DetectJumpKill() {
        int layerMask = 1 << 9;
        Vector3 point1 = feet.transform.position;
        Vector3 point2 = point1 + Vector3.down * 0.2f;
        float radius = 0.25f;
        Vector3 direction = Vector3.down;
        float maxDistance = 0.2f;
        //Debug.DrawLine(point1, point2, Color.yellow);
        Debug.DrawRay(point1, direction * maxDistance, Color.yellow);
        RaycastHit hit;
        Ray ray = new Ray(point1, direction);
        if (Physics.Raycast(ray, out hit, maxDistance, layerMask)) {
       // if (Physics.CapsuleCast(point1, point2, radius, direction, out hit, maxDistance, layerMask)) {
            Debug.Log("JUMP HIT " + hit.collider.name);
            if (hit.collider.transform.parent.tag == "Player") {
                Debug.Log("JUMP KILL!");
                rigidbody.AddForce(Vector3.up * 20f, ForceMode.Impulse);
                Destroy(hit.collider.transform.parent.gameObject);

            }
        }
    }

    void OnTriggerEnter(Collider col) {
        if (col.tag == "Item" || col.tag == "Arrow") {
            Item item = col.gameObject.GetComponent<Item>();
            if (item.grabbable) {
                PickUpItem(item);
            }
        }
    }
}
