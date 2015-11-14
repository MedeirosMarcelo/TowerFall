using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClientManager : MonoBehaviour {


    public GameObject player { get; private set; }

    //public IList<GameObject> playerList = new List<GameObject>();
    public GameObject basicArrowPrefab;
    public GameObject characterPrefab;

    int maxArrows = 7;
    //WorldMirror worldMirror;
    //Score score;
    float spawnTime = 3f;

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
        Debug.Log("Spawn");
        var spawn = spawnPoints.PickRandom().transform;
        player = (GameObject)Network.Instantiate(characterPrefab, spawn.position, spawn.rotation, 0);
        player.transform.SetParent(transform);
    }

    /* OLD Local Code

    void Start() {
        worldMirror = GetComponent<WorldMirror>();
        score = GameObject.FindWithTag("Canvas").transform.Find("Score").GetComponent<Score>();
    }


    public void SpawnItem(GameObject obj, Vector3 position, Quaternion rotation) {

        GameObject newArrow = worldMirror.InstantiateAll(basicArrowPrefab, position, rotation);
        newArrow.transform.parent = this.transform;
    }

    public void SpawnPlayer(GameObject obj, Vector3 position, Quaternion rotation) {

        GameObject newPlayer = worldMirror.InstantiateAll(characterPrefab, position, rotation);
        playerList.Add(newPlayer);

        int playerNumber = playerList.IndexOf(newPlayer) + 1;
        newPlayer.GetComponent<Character>().Create(playerNumber);
        newPlayer.transform.parent = this.transform;

    }

    public void RespawnPlayer(GameObject obj, int playerNumber, Vector3 position, Quaternion rotation) {
    }

    public void Scored(int playerNumber) {
        score.Scored(playerNumber);
    }

    public void Respawn(int playerNumber) {
        Debug.Log("!!!!");
        StartCoroutine("SetRespawn", playerNumber);
    }

    IEnumerator SetRespawn(int playerNumber) {
        yield return new WaitForSeconds(spawnTime);
        RespawnPlayer(characterPrefab, playerNumber, new Vector3(-60f, 1.46f, 68f), transform.rotation);
    }
    */
}
