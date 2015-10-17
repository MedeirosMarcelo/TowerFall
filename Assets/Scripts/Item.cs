using UnityEngine;
using System.Collections;

public class Item : Reflectable {

    public bool grabbable = true;

    public void PickUp() {
        Destroy();
    }

    void Destroy() {
        
        Destroy(this.gameObject);
    }

    void Use(Item item) {

    }
}
