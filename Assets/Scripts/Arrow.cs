using UnityEngine;
using System.Collections;

public enum ArrowType {
    None = 0,
    Basic,
}

public class Arrow : DamageDealer {

    public ArrowType type = ArrowType.Basic;
    public static readonly int group = (int)NetworkGroup.Arrow;

    bool canHitOwner = false;
    float ownerHitDelay = 1f;
    float lifespan = 5f;
    float speed = 35f;
    float rotationSpeed = 10f;
    float arc = 0.2f;

    AudioSource audioSource;

    void Start() {
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
    void Destroy() {
        if(Network.isServer) {
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
            HitPlayer(col.gameObject);
        }
    }

    void HitScenary() {
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        rigidbody.detectCollisions = false;
        Debug.Log("ToServer");
        var prefab = GameObject.FindGameObjectWithTag("World Main").GetComponent<ServerManager>().arrowPickupPrefab;
        var newArrow = Network.Instantiate(prefab, transform.position, transform.rotation, 0) as GameObject;
        newArrow.GetComponent<ArrowPickup>().type = (ArrowType)type;
        Destroy();
    }

    void HitPlayer(GameObject player) {
        if (!alive || (!canHitOwner && (player.networkView.owner == networkView.owner))) {
            return;
        }

        Debug.Log("HitPlayer");
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        rigidbody.detectCollisions = false;
        Hit(player);
        Destroy();
    }
}