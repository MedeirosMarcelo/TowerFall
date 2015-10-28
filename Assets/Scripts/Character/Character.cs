using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : Reflectable {

    // exposing getters for internals 
    public CharacterController controller { get; private set; }
    public CharacterInput input { get; private set; }
    public CharacterArrows arrows { get; private set; }
    public CharacterFsm fsm { get; private set; }

    public Camera charCamera { get; private set; }
    public WorldMirror worldMirror { get; private set; }
    public GroundCollider groundCollider { get; private set; }
    public HandsCollider handsCollider { get; private set; }

    public int playerNumber;
    public int health = 1;
    public GameObject basicArrow;

    public CharacterInput.Type inputType;

    void OnValidate() {
        if (input != null) {
            input.type = inputType;
        }
    }

    void Start() {
        charCamera = GetComponentInChildren<Camera>();
        worldMirror = GetComponentInParent<WorldMirror>();
        handsCollider = GetComponentInChildren<HandsCollider>();
        groundCollider = GetComponentInChildren<GroundCollider>();
 
        controller = new CharacterController(this);
        arrows = new CharacterArrows(this);
        input = new CharacterInput(this);
        input.type = inputType;
        fsm = new CharacterFsm(this);
   }

    void Update() {
        // Input must be first here
        input.Update();
    }

    void FixedUpdate() {
        fsm.Update(Time.deltaTime);
        arrows.FixedUpdate();
        // Input must be last here
        input.FixedUpdate();
    }

    //Constructor
    public void Create(int playerNumber) {
        this.playerNumber = playerNumber;
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
