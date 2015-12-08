using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerManager : MonoBehaviour {

    public static ServerManager Get() {
        var obj = GameObject.FindWithTag("GameController");
        if (obj == null) {
            Debug.LogError("Server Manager Not Found");
            return null;
        }
        return obj.GetComponent<ServerManager>();
    }

    [HideInInspector]
    public List<Character> characterList = new List<Character>();
    [HideInInspector]
    public List<LobbyCharacter> lobbyCharacterList = new List<LobbyCharacter>();

    public StageManager stage { get; set; }

    [Header("Prefabs")]
    public GameObject lobbyCharacterPrefab;
    public GameObject characterPrefab;
    public GameObject arrowPrefab;
    public GameObject arrowPickupPrefab;
    public GameObject bombArrowPrefab;
    public GameObject bombArrowPickupPrefab;

    public GameObject chestPrefab;
    public GameObject arrowIconPrefab;
    public GameObject bombArrowIconPrefab;
    public GameObject shieldIconPrefab;

    private readonly int countdownMax = 5;
    private int countDown = 5;

    enum ServerState {
        WaitingMorePlayers,
        WaintingReady,
        CountDown,
        RoundLoad,
        RoundStart,
        InRound
    }
    ServerState serverState = ServerState.WaitingMorePlayers;

    void ChangeState(ServerState newState) {
        Debug.Log("Change State: " + serverState + " >> " + newState);
        if (serverState == newState) {
            return;
        }
        ExitState();
        serverState = newState;
        EnterState();
    }
    void ExitState() {
        switch (serverState) {
            case ServerState.WaitingMorePlayers:
            case ServerState.WaintingReady:
            case ServerState.CountDown:
            case ServerState.RoundLoad:
            case ServerState.RoundStart:
            default:
                break;
            case ServerState.InRound:
                foreach (var character in characterList) {
                    character.ServerDestroy();
                }
                characterList.Clear();
                break;
        }
    }
    void EnterState() {
        switch (serverState) {
            case ServerState.WaitingMorePlayers:
            case ServerState.WaintingReady:
            case ServerState.CountDown:
                CountDown();
                break;
            case ServerState.RoundLoad:
                LoadRound();
                break;
            case ServerState.RoundStart:
                StartRound();
                break;
            case ServerState.InRound:
                break;
            default:
                Debug.LogError("Wait what? " + serverState);
                break;
        }
    }
    void Update() {
        switch (serverState) {
            case ServerState.WaitingMorePlayers:
                if (lobbyCharacterList.Count > 1) {
                    ChangeState(ServerState.WaintingReady);
                }
                break;
            case ServerState.WaintingReady:
                if (lobbyCharacterList.Count < 1) {
                    ChangeState(ServerState.WaitingMorePlayers);
                }
                else {
                    if (!lobbyCharacterList.Exists(character => !character.isReady)) {
                        ChangeState(ServerState.CountDown);
                    }
                }
                break;
            case ServerState.CountDown:
                if (lobbyCharacterList.Count < 1) {
                    ChangeState(ServerState.WaitingMorePlayers);
                }
                else {
                    foreach (var lobbyCharacter in lobbyCharacterList) {
                        if (!lobbyCharacter.isReady) {
                            ChangeState(ServerState.WaintingReady);
                        }
                    }
                }
                break;
            case ServerState.RoundLoad:
                break;
            case ServerState.RoundStart:
                break;
            case ServerState.InRound:
                Debug.Log("InRound: " + characterList.Count);
                if (characterList.Count < 2) {
                    ChangeState(ServerState.RoundLoad);
                }
                break;
            default:
                break;
        }
    }
    int log = 0;


    private void CountDown() {
        if (serverState != ServerState.CountDown) {
            countDown = countdownMax;
            return;
        }
        if (countDown == 0) {
            SendLobbyMessage("Server: Starting Game now");
            countDown = countdownMax;
            ChangeState(ServerState.RoundLoad);
            return;
        }
        else {
            SendLobbyMessage("Server: Starting Game in " + countDown);
            countDown--;
            Invoke("CountDown", 1);
        }
    }

    void SpawnItems() {
        GameObject[] loot = new GameObject[2];
        loot[0] = bombArrowIconPrefab;
        loot[1] = shieldIconPrefab;
        chestPrefab.GetComponent<Chest>().Create(loot);
        var spawn = stage.chestSpawnList.PickRandom();
        var newChest = Network.Instantiate(chestPrefab, spawn.transform.position, chestPrefab.transform.rotation, 0) as GameObject;
        newChest.transform.SetParent(stage.transform);
    }

    [RPC]
    void LoadRound() {
        networkView.RPC("LoadRound", RPCMode.Others);
        //should be a response, using delay
        Invoke("DelayedStart", 5);
    }
    void DelayedStart () { ChangeState(ServerState.RoundStart); }
    [RPC]
    void StartRound() {
        networkView.RPC("StartRound", RPCMode.Others);
        SpawnItems();
        Invoke("DelayedInRound", 5);
    }
    void DelayedInRound () { ChangeState(ServerState.InRound); }


    void SendLobbyMessage(string msg) {
        Debug.Log("Lobby message: \"" + msg + "\"");
        networkView.RPC("LobbyMessage", RPCMode.Others, "<color=red>" + msg + "</color>");
    }
    [RPC]
    void LobbyMessage(string msg) {
    }
    [RPC]
    void ChatMessage(string msg) {
    }
    void OnDisconnectedFromServer() {
        ChangeState(ServerState.WaitingMorePlayers);
    }
}
