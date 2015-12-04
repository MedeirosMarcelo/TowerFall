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

    ClientUIManager clientUIManager;
    GameObject stage;
    GameObject[] spawnPoints;

    void Start() {
        clientUIManager = GetComponent<ClientUIManager>();
        stage = GameObject.FindGameObjectWithTag("Stage");
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
    }

    public void OnConnected() {
            SpawnPlayer();
    }
    public void OnDisconnect() {
            Network.Destroy(player);
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
