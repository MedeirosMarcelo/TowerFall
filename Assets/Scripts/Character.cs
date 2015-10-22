using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : Reflectable {

    public int health = 1;
    public IList<GameObject> arrows = new List<GameObject>();
    public string[] arrowListDetail = new string[10];
    public GameObject basicArrow;
    Controller controller;

    void Start() {
        controller = GetComponent<Controller>();
    }

    void Update() {
        int i = 0;
        foreach (GameObject arrow in arrows){
            arrowListDetail[i] = arrow.name;
            i++;
        }
    }

    public void StoreArrow(Arrow arrow) {
        GameObject newArrow = GetArrowByType(arrow.type);
        arrows.Add(newArrow);
    }

    public void RemoveArrow(GameObject arrow) {
        arrows.Remove(arrow);
    }

    void UseItem(Item item) {
        
    }

    public void TakeHit(DamageDealer damager) {
        if (controller.state == PlayerState.Dash) {
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

    public GameObject GetArrow() {
        return arrows[0];
    }

    public void PickUpItem(Item item) {
        if (item.tag == "Arrow") {
            StoreArrow((Arrow)item);
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

    GameObject GetArrowByType(ArrowType type) {
        switch (type) {
            default:
            case ArrowType.basic:
                return basicArrow;
        }
    }
}
