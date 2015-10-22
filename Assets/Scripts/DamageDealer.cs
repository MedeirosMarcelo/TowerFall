using UnityEngine;
using System.Collections;

public class DamageDealer : Item {

    public int damage = 1;
    protected bool alive = false;

    protected void HitPlayer(GameObject obj) {
        if (alive) {
            obj.GetComponent<Character>().TakeHit(this);
        }
    }
}
