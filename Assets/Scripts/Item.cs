using UnityEngine;
using System.Collections;

public class Item : Reflectable {

    public bool grabbable = true;

    public void PickUp() {
        Destroy();
    }

    void Destroy() {
        WorldMirror worldMirror = transform.parent.GetComponent<WorldMirror>();
        worldMirror.DestroyAll(this.gameObject);
    }

    void Use(Item item) {

    }
}
