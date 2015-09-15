using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

    Rigidbody rb;
    Vector3 movement;

	void Start () {
        rb = GetComponent<Rigidbody>();
        movement = Vector3.zero;
    }

    public void Move(Vector3 move) {
        movement += move;
    }

	void FixedUpdate () {
        rb.MovePosition(transform.position + movement);
        movement = Vector3.zero;
	}
}
