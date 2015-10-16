using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour {

    IList<Arrow> arrows = new List<Arrow>();

    public void StoreArrow(Arrow arrow) {
        arrows.Add(arrow);
    }

    void UseItem(Item item) {
        
    }

    public void PickUpItem(Item item) {
        if (item.tag == "Arrow") {
            arrows.Add((Arrow)item);
        }
        else if (item.tag == "Item") {
            UseItem(item);
        }
        else{
            Debug.LogError("PickUpItem - Item has invalid tag.");
        }
    }

    void OnTriggerEnter(Collider col) {
        if (col.tag == "Item" || col.tag == "Arrow") {
            Item item = col.gameObject.GetComponent<Item>();
            PickUpItem(item);
        }
    }
}
