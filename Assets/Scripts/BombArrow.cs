using UnityEngine;
using System.Collections;

public class BombArrow : Arrow {

    //public float explosionDelay;
    //public float destroyDelay;
    //GameObject damageArea;

    //void Start() {
    //    damageArea = transform.Find("DamageArea").gameObject;
    //    type = ArrowType.Bomb;
    //}

    //void OnCollisionEnter(Collision col) {
    //    if (Network.isClient) {
    //        return;
    //    }
    //    if (col.gameObject.tag == "Walkable") {
    //        HitScenary();
    //    }
    //    else if (col.gameObject.tag == "Player") {
    //        if (col.gameObject.GetComponent<Character>().fsm.state == CharacterFsm.State.Dodging) {
    //            PickUp(col.gameObject);
    //        }
    //        else {
    //            HitPlayer(col.gameObject);
    //        }
    //    }
    //}

    //void HitScenary() {
    //    rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    //    rigidbody.detectCollisions = false;
    //    var prefab = ServerManager.Get().arrowPickupPrefab;
    //    var newArrow = Network.Instantiate(prefab, transform.position, transform.rotation, 0) as GameObject;
    //    newArrow.GetComponent<ArrowPickup>().type = (ArrowType)type;
    //    Destroy();
    //}

    //void HitPlayer(GameObject player) {
    //    if (!alive || (!canHitOwner && (player.networkView.owner == networkView.owner))) {
    //        return;
    //    }

    //    rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    //    rigidbody.detectCollisions = false;
    //    Hit(player);
    //    Destroy();
    //}

    //public void Explode() {
    //    StartCoroutine("WaitAndExplode");
    //}

    //IEnumerator WaitAndExplode() {
    //    yield return new WaitForSeconds(explosionDelay);
    //    damageArea.SetActive(true);
    //    StartCoroutine("WaitAndDestroy");
    //}

    //IEnumerator WaitAndDestroy() {
    //    yield return new WaitForSeconds(destroyDelay);
    //    Destroy();
    //}
}
