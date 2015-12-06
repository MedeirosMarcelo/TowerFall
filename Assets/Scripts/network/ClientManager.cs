using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClientManager : MonoBehaviour {

    public GameObject player { get; private set; }
    public Character character { get; private set; }

    // All prefabs are referenced here to guaratee it's inclusion on build
    public GameObject basicArrowPrefab;
    public GameObject arrowPickupPrefab;
    public GameObject characterPrefab;

    //WorldMirror worldMirror;
    //Score score;

    GameManager gameManager;
    GameObject stage;
    GameObject[] spawnPoints;

    void Start() {
        gameManager = GameManager.Get();
        gameManager.lockCursor = true;

        stage = GameObject.FindGameObjectWithTag("Stage");
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");

         if (Network.isClient) {
            SpawnPlayer();
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F12)) {
            if (Network.isClient) {
                Debug.Log("Disconnect");
                Network.Destroy(player);
                Network.Disconnect();
            }
            else {
                Debug.Log("Connect");
                Network.Connect("127.0.0.1", 25001);
            }
        }

    }

    public void OnConnectedToServer() {
        SpawnPlayer();
    }

    public void OnMenuOpen() {
        if (character != null) {
            character.input.mode = CharacterInput.InputMode.OnMenu;
        }
    }
    public void OnMenuClosed() {
        if (character != null) {
            character.input.mode = CharacterInput.InputMode.InGame;
        }
    }
    public void OnChatOpened() {
        if (character != null) {
            character.input.mode = CharacterInput.InputMode.OnChat;
        }
    }
    public void OnChatClosed() {
        if (character != null) {
            character.input.mode = CharacterInput.InputMode.InGame;
        }
    }
    public void SpawnPlayer() {
        var spawn = spawnPoints.PickRandom().transform;
        player = (GameObject)Network.Instantiate(characterPrefab, spawn.position, spawn.rotation, Character.group);
        player.transform.SetParent(transform);
        character = player.GetComponent<Character>();
    }
}
