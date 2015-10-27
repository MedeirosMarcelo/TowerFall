using UnityEngine;
using System.Collections;

public class DamageDealer : Item {

    public int damage = 1;
    public bool alive = false;

    public void Hit(GameObject obj) {
        if (alive) {
            obj.GetComponent<Character>().TakeHit(this);
        }
    }
}
