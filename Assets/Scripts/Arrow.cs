﻿using UnityEngine;
using System.Collections;

public enum ArrowType {
    Basic
}

public class Arrow : DamageDealer {

    public ArrowType type = ArrowType.Basic;
    Character owner;

    bool shot = false;
    bool canHitOwner = false;
    float ownerHitDelay = 1f;
    float lifespan = 5f;
    float speed = 35f;
    float rotationSpeed = 10f;
    float arc = 0.2f;

    NetworkView netView;
    AudioSource audioSource;

    void Start() {
        netView = GetComponent<NetworkView>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate() {
        transform.forward = Vector3.Slerp(transform.forward, rigidbody.velocity.normalized, rotationSpeed * Time.deltaTime);
    }

    public void Shoot(Character owner) {
        this.owner = owner;
        shot = true;
        alive = true;
        Play();
        Move();
        Invoke("AllowHitOwner", ownerHitDelay);
        Destroy(this, lifespan);
    }

    void Play() {
        audioSource.Play();
    }

    void Move() {
        Vector3 direction = transform.forward;
        direction.y += arc;
        rigidbody.AddForce(direction * speed, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.name == "Floor" || col.gameObject.name == "Wall") {
            HitScenary();
        }
        else if (col.gameObject.tag == "Player") {
            HitPlayer(col.gameObject);
        }
    }

    void AllowHitOwner() {
        canHitOwner = true;
    }

    void HitScenary() {
        rigidbody.isKinematic = true;
        canHitOwner = false;
        alive = false;
        Debug.Log("HitScene");
    }

    void HitPlayer(GameObject player) {
        if (CanHit(player)) {
            rigidbody.isKinematic = true;
            canHitOwner = false;
            alive = false;
            Hit(player);
            Debug.Log("HitPlayer");
        }
    }

    bool CanHit(GameObject player) {
        return (alive && (canHitOwner || player != owner.gameObject));
    }
}