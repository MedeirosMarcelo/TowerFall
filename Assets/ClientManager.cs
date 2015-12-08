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
    public ChatManager chatManager { get; set; }
    public GameObject stage { get; set; }
    public GameObject[] spawnPoints { get; set; }
    public Character character { get; private set; }
    public string playerName { get; set; }
    public bool lockCursor { get; set; }

    public GameObject chest;
    public GameObject arrowIcon;
    public GameObject bombArrowIcon;
    public GameObject shieldIcon;
    public GameObject spawnChest;

    void Awake() {
        if (gameManager != null) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
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
        SpawnItems();
    }


    void SpawnItems() {
        GameObject[] loot = new GameObject[2];
        loot[0] = bombArrowIcon;
        loot[1] = shieldIcon;
        chest.GetComponent<Chest>().Create(loot);
        GameObject newChest = (GameObject)Network.Instantiate(chest, spawnChest.transform.position, chest.transform.rotation, 0);
        newChest.transform.SetParent(Camera.main.GetComponent<Stage>().mainStage.transform);
    }

    [RPC]
    void StartRound() {
        Application.LoadLevel("Client");
        Invoke("SpawnPlayer", 1);
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
