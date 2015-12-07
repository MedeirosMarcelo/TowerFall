using UnityEngine;
using System.Collections;

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

    public LobbyManager lobbyManager { get; set; }
    public HUDManager hudManager { get; set; }
    public GameObject stage { get; set; }
    public GameObject[] spawnPoints { get; set; }
    public Character character { get; private set; }
    public string playerName { get; set; }
    public bool lockCursor { get; set; }

    void Awake() {
        if (gameManager != null) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }
    void Start() {
    }

    void Update() {
        /* if cursor is not locked as desired we relock/unlock it here */
        if (lockCursor && !Screen.lockCursor) {
            Screen.lockCursor = true;
        }
        if (!lockCursor && Screen.lockCursor) {
            Screen.lockCursor = false;
        }

#if UNITY_EDITOR
        // Simple connect for running on editor
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
#endif
    }

#if UNITY_EDITOR
    public void OnConnectedToServer() {
        SpawnPlayer();
    }
#endif

    public void SpawnPlayer() {
        var spawn = spawnPoints.PickRandom().transform;
        var obj = Network.Instantiate(characterPrefab, spawn.position, spawn.rotation, Character.group) as GameObject;
        obj.transform.SetParent(transform);
        character = obj.GetComponent<Character>();
    }

    [RPC]
    void StartRound() {
        Application.LoadLevel("Client");
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
        if (hudManager != null) {
            hudManager.chat.AddMessage(msg);
        }
    }
    public void SendLobbyMessage(string msg) {
        networkView.RPC("LobbyMessage", RPCMode.Others, msg);
    }
    public void SendChatMessage(string msg) {
        networkView.RPC("ChatMessage", RPCMode.Others, msg);
    }
}
