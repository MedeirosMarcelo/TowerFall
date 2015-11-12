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
    }

    public void Create(int playerNumber) {
        Start();
        this.playerNumber = playerNumber;
    }

    void Start() {
        Debug.Log("Start");

        this.playerNumber = 0;
        charCamera = GetComponentInChildren<Camera>();
        net = GetComponent<NetworkView>();

        if (!net.isMine) {
            charCamera.gameObject.SetActive(false);
        }
        //worldMirror = GetComponentInParent<WorldMirror>();
        handsCollider = GetComponentInChildren<HandsCollider>();
        groundCollider = GetComponentInChildren<GroundCollider>();

        input = new CharacterInput(this);
        controller = new CharacterController(this, input);
        arrows = new CharacterArrows(this, input);
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
                    arrows.StoreArrow((Arrow)item);
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

    void OnTriggerEnter(Collider col) {
        if (col.tag == "Item" || col.tag == "Arrow") {
            Item item = col.gameObject.GetComponent<Item>();
            if (item.grabbable) {
                PickUpItem(item);
            }
        }
    }
}
