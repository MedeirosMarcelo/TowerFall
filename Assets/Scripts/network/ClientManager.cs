using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClientManager : MonoBehaviour {
    public GameObject player { get; private set; }

    // All prefabs are referenced here to guaratee it's inclusion on build
    public GameObject basicArrowPrefab;
    public GameObject arrowPickupPrefab;
    public GameObject characterPrefab;

    //WorldMirror worldMirror;
    //Score score;

    Client client;
    GameObject stage;
    GameObject[] spawnPoints;

    void Start() {
        client = GameObject.FindGameObjectWithTag("Client").GetComponent<Client>();
        stage = GameObject.FindGameObjectWithTag("Stage");
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");

        client.onConnected += delegate () {
            SpawnPlayer();
        };

        client.onDisconnect += delegate () {
            Network.Destroy(player);
        };
    }

    public void SpawnPlayer() {
        var spawn = spawnPoints.PickRandom().transform;
        player = (GameObject)Network.Instantiate(characterPrefab, spawn.position, spawn.rotation, Character.group);
        player.transform.SetParent(transform);
    }
}
