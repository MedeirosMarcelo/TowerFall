using UnityEngine;
using System.Collections;

public class ServerManager : MonoBehaviour {

    public static ServerManager Get() {
        var obj = GameObject.FindWithTag("GameController");
        if (obj == null) {
            Debug.LogError("Server Manager Not Found");
            return null;
        }
        return obj.GetComponent<ServerManager>();
    }

    public GameObject lobbyCharacterPrefab;
    public GameObject characterPrefab;

    public GameObject arrowPrefab;
    public GameObject arrowPickupPrefab;
    public GameObject bombArrowPrefab;
    public GameObject bombArrowPickupPrefab;

    private readonly int countdownMax = 10;
    private int countDown = 10;
 
    enum ServerState {
        Empty,
        WaintingReady,
        CountDown,
        InRound
    }
    ServerState serverState = ServerState.Empty;

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
            case ServerState.Empty:
                break;
            case ServerState.WaintingReady:
                break;
            case ServerState.CountDown:
                break;
            case ServerState.InRound:
                break;
            default:
                Debug.LogError("Wait what? " + serverState);
                break;
        }
    }
    void EnterState() {
        switch (serverState) {
            case ServerState.Empty:
                break;
            case ServerState.WaintingReady:
                break;
            case ServerState.CountDown:
                CountDown();
                break;
            case ServerState.InRound:
                StartRound();
                break;
            default:
                Debug.LogError("Wait what? " + serverState);
                break;
        }
    }
    void Update() {
        switch (serverState) {
            case ServerState.Empty:
                if (LobbyCharacter.lobbyCharacterSet.Count > 0) {
                    ChangeState(ServerState.WaintingReady);
                }
                break;
            case ServerState.WaintingReady:
                if (LobbyCharacter.lobbyCharacterSet.Count == 0) {
                    ChangeState(ServerState.Empty);
                }
                else {
                    foreach (var lobbyCharacter in LobbyCharacter.lobbyCharacterSet) {
                        if (!lobbyCharacter.isReady) {
                            break;
                        }
                        ChangeState(ServerState.CountDown);
                    }
                }
                break;
            case ServerState.CountDown:
               if (LobbyCharacter.lobbyCharacterSet.Count == 0) {
                    ChangeState(ServerState.Empty);
                }
                else {
                    foreach (var lobbyCharacter in LobbyCharacter.lobbyCharacterSet) {
                        if (!lobbyCharacter.isReady) {
                            ChangeState(ServerState.WaintingReady);
                        }
                    }
                }
                break;
            case ServerState.InRound:
                break;
            default:
                break;
        }
    }

   private void CountDown() {
        if (serverState != ServerState.CountDown) {
            countDown = countdownMax;
            return;
        }
        if (countDown == 0) {
            SendLobbyMessage("Server: Starting Game now");
            countDown = countdownMax;
            ChangeState(ServerState.InRound);
            return;
        } else {

            SendLobbyMessage("Server: Starting Game in " + countDown);
            countDown--;
            Invoke("CountDown", 1);
        }
    }

    [RPC]
    void StartRound() {
        networkView.RPC("StartRound", RPCMode.Others);
    }

    void SendLobbyMessage(string msg) {
        Debug.Log("Lobby message: \"" + msg + "\"");
        networkView.RPC("LobbyMessage", RPCMode.Others,"<color=red>" + msg + "</color>");
    }
    [RPC]
    void LobbyMessage(string msg) {
    }
    [RPC]
    void ChatMessage(string msg) {
    }
    void OnDisconnectedFromServer() {
        ChangeState(ServerState.Empty);
    }
}
