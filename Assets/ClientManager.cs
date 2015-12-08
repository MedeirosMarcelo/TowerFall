using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClientManager : MonoBehaviour {

    private static ClientManager gameManager = null;
    public static ClientManager Get() {
        var obj = GameObject.FindWithTag("GameController");
        if (obj == null) {
            Debug.LogError("Game Manager Not Found");
            return null;
        }
        return obj.GetComponent<ClientManager>();
    }

    [Header("Prefabs")]
    public GameObject arrowPickupPrefab;
    public GameObject characterPrefab;
    public GameObject chestPrefab;
    public GameObject arrowIconPrefab;
    public GameObject arrowIconReflecationPrefab;
    public GameObject bombArrowIconPrefab;
    public GameObject bombArrowRelfactionIconPrefab;
    public GameObject shieldIconPrefab;
    public GameObject shieldIconReflectionPrefab;

    [HideInInspector]
    public List<Character> characterList;

    public LobbyManager lobbyManager { get; set; }
    public ChatManager chatManager { get; set; }
    public StageManager stage { get; set; }
    public Character character { get; private set; }
    public string playerName { get; set; }
    public CharacterColor playerColor { get; set; }
    public bool lockCursor { get; set; }


    void Awake() {
        if (gameManager != null) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    void Update() {
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.currentScene == "Client") {
            if (Input.GetKeyDown(KeyCode.F12)) {
                if (Network.isClient) {
                    Debug.Log("Disconnect");
                    Network.Destroy(character.gameObject);
                    Network.Disconnect();
                }
                else {
                    Debug.Log("Connect");
                    Network.Connect("127.0.0.1", 25001);
                }
            }
        }
#endif
    }

#if UNITY_EDITOR
    bool spawnOnConnected = false;
    public void OnConnectedToServer() {
        if (UnityEditor.EditorApplication.currentScene == "Client") {
            SpawnPlayer();
        }
    }
#else 
    public void OnDisconnectedFromServer() {
        Application.LoadLevel("Menu");
    }
#endif

    public void SpawnPlayer() {
        var spawn = stage.characterSpawnList.PickRandom().transform;
        var obj = Network.Instantiate(characterPrefab, spawn.position, spawn.rotation, Character.group) as GameObject;
        obj.transform.SetParent(stage.transform);
        character = obj.GetComponent<Character>();
        character.color = playerColor;
    }

    [RPC]
    void LoadRound() {
        Application.LoadLevel("Client");
    }
    [RPC]
    void StartRound() {
        SpawnPlayer();
    }

    // This are here so we guarantee Server <> Client RPCs will find ech other
    [RPC]
    void LobbyMessage(string msg) {
        if (lobbyManager != null) {
            lobbyManager.chat.AddMessage(msg);
        }
    }
    [RPC]
    void ChatMessage(string msg) {
        if (chatManager != null) {
            chatManager.chat.AddMessage(msg);
        }
    }
    public void SendLobbyMessage(string msg) {
        networkView.RPC("LobbyMessage", RPCMode.Others, msg);
    }
    public void SendChatMessage(string msg) {
        networkView.RPC("ChatMessage", RPCMode.Others, msg);
    }
}
