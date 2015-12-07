using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LobbyCharacter : MonoBehaviour {

    public static readonly int group = (int)NetworkGroup.CharacterLobby;

    public GameObject lobbyCharacterViewPrefab;
    LobbyCharacterView lobbyCharacterView;

    private string _playerName = "";
    private Color _color = new Color();
    private bool _isReady = false;

    public string playerName {
        get { return _playerName; }
        set {
            _playerName = value;
            networkView.RPC("SendPlayerName", RPCMode.Others, _playerName);
            Debug.Log("Set playerName");
        }
    }
    public Color color {
        get { return _color; }
        set {
            _color = value;
            networkView.RPC("SendColor", RPCMode.Others, _color.r, _color.g, _color.b, _color.a);
            Debug.Log("Set color");
        }
    }
    public bool isReady {
        get { return _isReady; }
        set {
            _isReady = value;
            networkView.RPC("SendIsReady", RPCMode.Others, _isReady);
            Debug.Log("Set isReady");
        }
    }

    ServerManager serverManager;
    GameManager gameManager;

    void Start() {
        if (Network.isServer) {
            serverManager = ServerManager.Get();
            serverManager.lobbyCharacterList.Add(this);
            _color = Color.green;
            networkView.RPC("SendColor", networkView.owner, _color.r, _color.g, _color.b, _color.a);
        }
        else if (Network.isClient) {
            gameManager = GameManager.Get();
            if (!networkView.isMine) {
                var obj = Instantiate(lobbyCharacterViewPrefab) as GameObject;
                obj.transform.SetParent(gameManager.lobbyManager.playerList.transform);
                lobbyCharacterView = obj.GetComponent<LobbyCharacterView>();
                SyncAllData();
            }
            else {
                _playerName = gameManager.playerName;
            }
        }
    }
    void OnDestroy() {
        if (Network.isServer) {
            serverManager.lobbyCharacterList.Remove(this);
        }
        if (Network.isClient && !networkView.isMine) {
            Destroy(lobbyCharacterView.gameObject);
        }
        Destroy();
    }

    void OnDisconnectedFromServer() {
        if (lobbyCharacterView != null) {
            Destroy(lobbyCharacterView.gameObject);
        }
        Destroy(gameObject);
    }

    [RPC]
    void SendPlayerName(string playerName) {
        Debug.Log("Send playerName");
        _playerName = playerName;
        if (Network.isClient && !networkView.isMine) {
            Debug.Log(" Show playerName");
            lobbyCharacterView.playerName = _playerName;
        }
    }
    [RPC]
    void SendColor(float r, float g, float b, float a) {
        Debug.Log("Send color");
        _color = new Color(r, g, b, a);
        if (Network.isClient) {
            if (networkView.isMine) {
                Debug.Log("Set lobbymanager color");
                gameManager.lobbyManager.SetColor(_color);
            }
            else {
                Debug.Log("Show color");
                lobbyCharacterView.color = _color;
            }
        }
    }
    [RPC]
    void SendIsReady(bool isReady) {
        Debug.Log("Send isReady");
        _isReady = isReady;
        if (Network.isClient && !networkView.isMine) {
            Debug.Log("Show isReady");
            lobbyCharacterView.isReady = _isReady;
        }
    }
    [RPC]
    public void SyncAllData() {
        if (networkView.isMine) {
            Debug.Log("Sending OwnerData");
            networkView.RPC("SendPlayerName", RPCMode.Others, _playerName);
            networkView.RPC("SendColor", RPCMode.Others, _color.r, _color.g, _color.b, _color.a);
            networkView.RPC("SendIsReady", RPCMode.Others, _isReady);
            return;
        }
        else {
            networkView.RPC("SyncAllData", RPCMode.Others);
        }
    }

    [RPC]
    public void ChangeColor() {
        Debug.Log("ChanegColor");
        if (networkView.isMine) {
            networkView.RPC("ChangeColor", RPCMode.Server);
            return;
        }
        if (Network.isServer) {
            color = Color.blue;
        }
    }
    [RPC]
    public void Destroy() {
        if (Network.isServer) {
            Debug.Log("Destroy Character " + GetInstanceID());
            Network.RemoveRPCs(networkView.owner, group);
            Network.Destroy(gameObject);
            return;
        }
        else if (Network.isClient && networkView.isMine) {
            networkView.RPC("Destroy", RPCMode.Server);
            Debug.Log("Destroy Character Lobby" + GetInstanceID());
        }
    }
}
