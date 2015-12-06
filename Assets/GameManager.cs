using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    private static GameManager gameManager = null;
    public static GameManager Get() {
        var obj = GameObject.FindWithTag("GameController");
        if (obj == null) {
            Debug.LogError("Game Manager Not Found");
            return null;
        }
        return obj.GetComponent<GameManager>();
    }

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
    }

    [RPC]
    void StartRound() {
        Application.LoadLevel("Client");
    }

    // This are here so we guarantee Server <> Client RPCs will find ech other

    public delegate void OnTextMessage(string msg);
    public OnTextMessage onLobbyMessage;
    public OnTextMessage onChatMessage;
    [RPC]
    void LobbyMessage(string msg) {
        if (onLobbyMessage != null) {
            onLobbyMessage(msg);
        }
    }
    [RPC]
    void ChatMessage(string msg) {
        if (onChatMessage != null) {
            onChatMessage(msg);
        }
    }
}
