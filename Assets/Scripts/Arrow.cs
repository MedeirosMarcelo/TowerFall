using UnityEngine;
using System.Collections;

public enum ArrowType {
    Basic
}

public class Arrow : DamageDealer {

    public ArrowType type;
    public bool shot;
    float speed = 35f;
    float rotationSpeed = 10f;
    float lifespan = 3f;
    float arc = 0.2f;
    float endTime;

    void Start() {
        endTime = lifespan + Time.time;
        //	Physics.gravity = new Vector3 (0, -300, 0);
        if (shot) {
            Move();
        }
    }

    void FixedUpdate() {
        transform.forward = Vector3.Slerp(transform.forward, rigidbody.velocity.normalized, rotationSpeed * Time.deltaTime);
        //	DestroyOnTime ();
    }

    public void Shoot() {
        shot = true;
        alive = true;
        Debug.Log(shot + " " + alive);
    }

    void Move() {
        Vector3 direction = transform.forward;
        direction.y += arc;
        rigidbody.AddForce(direction * speed, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.name == "Floor" ||
            col.gameObject.name == "Wall") {
            HitScene();
        }
        else if (col.gameObject.tag == "Player") {
            HitPlayer(col.gameObject);
        }
    }

    void DestroyOnTime() {
        if (Time.time > endTime) {
            Destroy(this.gameObject);
        }
    }

    void HitScene() {
        rigidbody.isKinematic = true;
        alive = false;
    }

    void HitPlayer(GameObject player) {
        Hit(player);
        rigidbody.isKinematic = true;
        transform.parent = player.transform;
        alive = false;
    }
}