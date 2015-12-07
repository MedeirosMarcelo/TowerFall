using UnityEngine;
using System.Collections;

public class Explosion : DamageDealer {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col) {
        if (Network.isServer) {
            if (col.gameObject.tag == "Player") {
                Debug.Log("PLAYER");
                HitPlayer(col.gameObject);
            }
        }
    }

    void HitPlayer(GameObject player) {
        Hit(player);
    }
}
