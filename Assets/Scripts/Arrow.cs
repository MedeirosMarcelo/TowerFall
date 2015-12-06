using UnityEngine;
using System.Collections;

public enum ArrowType {
    None = 0,
    Basic,
    Bomb
}

public class Arrow : DamageDealer {

    public ArrowType type = ArrowType.Basic;
    public static readonly int group = (int)NetworkGroup.Arrow;

    protected bool canHitOwner = false;
    protected float ownerHitDelay = 1f;
    protected float lifespan = 5f;
    protected float speed = 35f;
    protected float rotationSpeed = 10f;
    protected float arc = 0.2f;

    AudioSource audioSource;

    void Start() {
        if (type == ArrowType.Bomb) {
            damageArea = transform.Find("DamageArea").gameObject;
        }
        Debug.Log("Start arrow " + GetInstanceID());
        audioSource = GetComponent<AudioSource>();

        if (networkView.isMine) {
            Vector3 direction = transform.forward;
            direction.y += arc;
            rigidbody.AddForce(direction * speed, ForceMode.Impulse);
            Network.RemoveRPCs(networkView.viewID);
            //audioSource.Play();
        }
        if (Network.isServer) {
            alive = true;
            // No arrow may be allowed to live this long
            Invoke("Destroy", lifespan);
            Invoke("AllowHitOwner", ownerHitDelay);
        }
    }

    void AllowHitOwner() { canHitOwner = true; }
    
    [RPC]
    protected void Destroy() {
        if(Network.isServer) {
            if (type == ArrowType.Bomb) Explode();
            Debug.Log("Destroy Arrow " + GetInstanceID());
            Network.RemoveRPCs(networkView.owner, group);
            networkView.RPC("Destroy", RPCMode.Others);
            return;
        }
        if (networkView.isMine) {
            Debug.Log("Destroy Arrow " + GetInstanceID());
            Network.Destroy(gameObject);
            return;
        }
    }

    void FixedUpdate() {
        if (!networkView.isMine) {
            return;
        }
        transform.forward = Vector3.Slerp(transform.forward, rigidbody.velocity.normalized, rotationSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision col) {
        if (Network.isClient) {
            return;
        }
        if (col.gameObject.tag ==  "Walkable") {
            HitScenary();
        }
        else if (col.gameObject.tag == "Player") {
            if (col.gameObject.GetComponent<Character>().fsm.state == CharacterFsm.State.Dodging) {
                PickUp(col.gameObject);
            }
            else {
                HitPlayer(col.gameObject);
            }
        }
    }

    void HitScenary() {
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        rigidbody.detectCollisions = false;
        var prefab = GetPickupArrowByType(type);
        var newArrow = Network.Instantiate(prefab, transform.position, transform.rotation, 0) as GameObject;
        newArrow.GetComponent<ArrowPickup>().type = (ArrowType)type;
        Destroy();
    }

    void HitPlayer(GameObject player) {
        if (!alive || (!canHitOwner && (player.networkView.owner == networkView.owner))) {
            return;
        }

        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        rigidbody.detectCollisions = false;
        Hit(player);
        Destroy();
    }

    public void PickUp(GameObject character) {
        Debug.Log("Pickup");
        Character picker = gameObject.GetComponent<Character>();
        picker.networkView.RPC("StoreArrow", RPCMode.Others, (int)type);
        Destroy();
    }
    //Bomb Arrow
    public float explosionDelay;
    public float destroyDelay;
    GameObject damageArea;

    public void Explode() {
        Debug.Log("EXPLODE");
        damageArea.SetActive(true);
        damageArea.transform.SetParent(null);
        Destroy(damageArea, 1.2f);
        //StartCoroutine("WaitAndExplode");
    }

    IEnumerator WaitAndExplode() {
        yield return new WaitForSeconds(explosionDelay);
        damageArea.SetActive(true);
        StartCoroutine("WaitAndDestroy");
    }

    IEnumerator WaitAndDestroy() {
        yield return new WaitForSeconds(destroyDelay);
        Destroy();
    }

    GameObject GetPickupArrowByType(ArrowType type) {
        switch (type) {
            default:
            case ArrowType.Basic:
                return ServerManager.Get().arrowPickupPrefab;
            case ArrowType.Bomb:
                return ServerManager.Get().bombArrowPickupPrefab;
        }
    }
}