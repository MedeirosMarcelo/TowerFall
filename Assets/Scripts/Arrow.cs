﻿using UnityEngine;
using System.Collections;

public enum ArrowType {
    Basic
}

public class Arrow : DamageDealer {

    public ArrowType type;
    public Character owner;
    public bool shot;
    public float ownerHitDelay = 5f;
    public bool canHitOwner;
    float speed = 35f;
    float rotationSpeed = 10f;
    float lifespan = 3f;
    float arc = 0.2f;
    float endTime;
    float delayTime;
    State state;
    GameManager gameManager;
    AudioSource audioSource;

    void Start() {
        gameManager = GameObject.FindWithTag("World Main").GetComponent<GameManager>();
        endTime = lifespan + Time.time;
        delayTime = ownerHitDelay + Time.time;
        //	Physics.gravity = new Vector3 (0, -300, 0);

    }

    void FixedUpdate() {
        transform.forward = Vector3.Slerp(transform.forward, rigidbody.velocity.normalized, rotationSpeed * Time.deltaTime);
        StateMachine();
        //	DestroyOnTime ();
    }

    public enum State {
        Shot,
        Flying,
        Hit
    }

    void StateMachine() {
        switch (state) {
            case State.Shot:
                state = State.Flying;
                break;
            case State.Flying:
                AllowOwnerCollision();
                break;
            case State.Hit:
                break;
        }
    }

    public void Shoot(Character owner) {
        this.owner = owner;
        shot = true;
        alive = true;
        Move();
        state = State.Shot;

        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
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

    void AllowOwnerCollision() {
        if (!canHitOwner) {
            if (Time.time > delayTime) {
                canHitOwner = true;
            }
        }
    }

    void HitScene() {
        rigidbody.isKinematic = true;
        alive = false;
        canHitOwner = false;
        state = State.Hit;
        Debug.Log("HitScene");
    }

    void HitPlayer(GameObject player) {
        if (CanHit(player)) {
            Hit(player);
            rigidbody.isKinematic = true;
            transform.parent = player.transform;
            alive = false;
            canHitOwner = false;
            state = State.Hit;
            gameManager.Scored(owner.playerNumber);
            Debug.Log("HitPlayer");
        }
    }

    bool CanHit(GameObject player) {
        if (alive && (player.GetComponent<Character>().playerNumber != owner.playerNumber || canHitOwner)) {
            return true;
        }
        else {
            return false;
        }
    }
}