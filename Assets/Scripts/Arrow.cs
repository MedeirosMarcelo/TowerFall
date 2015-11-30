using UnityEngine;
using System.Collections;

public enum ArrowType {
    None = 0,
    Basic,
}

public class Arrow : DamageDealer {

    public ArrowType type = ArrowType.Basic;

    Character owner;
    bool canHitOwner = false;
    float ownerHitDelay = 1f;
    float lifespan = 5f;
    float speed = 35f;
    float rotationSpeed = 10f;
    float arc = 0.2f;

    NetworkView netView;
    AudioSource audioSource;

    private bool isNotMine { get { return !netView.isMine; } }
    private bool isNotAlive { get { return !alive; } }

    void Start() {
        Debug.Log("Start arrow " + GetInstanceID());
        netView = GetComponent<NetworkView>();
        audioSource = GetComponent<AudioSource>();

        if (Network.isServer) {
            /* No arrow may be allowed to live this long*/
            //Invoke("Destroy", lifespan);
        }
    }

    void Destroy() {
        Debug.Log("Destroy Arrow " + GetInstanceID());
        Network.RemoveRPCs(netView.viewID);
        Network.Destroy(netView.viewID);
    }

    void FixedUpdate() {
        if (isNotMine || isNotAlive) {
            return;
        }
        transform.forward = Vector3.Slerp(transform.forward, rigidbody.velocity.normalized, rotationSpeed * Time.deltaTime);
    }

    public void Shoot(Character owner) {
        this.owner = owner;
        alive = true;

        Vector3 direction = transform.forward;
        direction.y += arc;
        rigidbody.AddForce(direction * speed, ForceMode.Impulse);

        //audioSource.Play();
        Invoke("AllowHitOwner", ownerHitDelay);
    }

    void AllowHitOwner() {
        canHitOwner = true;
    }

    void OnCollisionEnter(Collision col) {
        if (Network.isClient) {
            return;
        }
        if (col.gameObject.name == "Floor" || col.gameObject.name == "Wall") {
            HitScenary();
        }
        else if (col.gameObject.tag == "Player") {
            HitPlayer(col.gameObject);
        }
    }

    void HitScenary() {
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        rigidbody.detectCollisions = false;
        alive = false;
        Debug.Log("ToServer");
        var prefab = GameObject.FindGameObjectWithTag("World Main").GetComponent<ServerManager>().arrowPickupPrefab;
        var newArrow = Network.Instantiate(prefab, transform.position, transform.rotation, 0) as GameObject;
        newArrow.GetComponent<ArrowPickup>().type = (ArrowType)type;
        Destroy();
    }

    void HitPlayer(GameObject player) {
        if (!CanHit(player)) {
            return;
        }
        Debug.Log("HitPlayer");
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        rigidbody.detectCollisions = false;
        alive = false;
        Hit(player);
    }

    bool CanHit(GameObject player) {
        return (alive && (canHitOwner || player != owner.gameObject));
    }

}