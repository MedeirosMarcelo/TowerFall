using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : Reflectable {

    PlayerInput input { get; set; }
    PlayerArrows arrows { get; set; }
    PlayerController controller { get; set; }
    PlayerFsm fsm { get; set; }

    public int health = 1;

    void Start() {
        input = GetComponent<PlayerInput>();
        arrows = GetComponent<PlayerArrows>();
        controller = GetComponent<PlayerController>();
        fsm = GetComponent<PlayerFsm>();
    }

    void Update() {
        input.InputUpdate();

    }

    void FixedUpdate() {
        controller.Move();
        controller.Dash();
        controller.Jump();
        arrows.Shoot();
        input.PostFixedUpdate();
    }


    void UseItem(Item item) {
        
    }

    public void TakeHit(DamageDealer damager) {
        if (fsm.state == PlayerFsm.State.Dash) {
            PickUpItem(damager);
        }
        else {
            int dmg = damager.GetComponent<DamageDealer>().damage;
            TakeDamage(dmg);
        }
    }

    void TakeDamage(int damage) {
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
            arrows.Store((Arrow)item);
            item.PickUp();
        }
        else if (item.tag == "Item") {
            UseItem(item);
            item.PickUp();
        }
        else{
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
