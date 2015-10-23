using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : Reflectable {

    PlayerInput input { get; set; }
    PlayerArrows arrows { get; set; }
    PlayerController controller { get; set; }

    void Start() {
        input = GetComponent<PlayerInput>();
        arrows = GetComponent<PlayerArrows>();
        controller = GetComponent<PlayerController>();
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
            Debug.Log("ARROW");
            Item item = col.gameObject.GetComponent<Item>();
            if (item.grabbable) {
                PickUpItem(item);
            }
        }
    }


}
