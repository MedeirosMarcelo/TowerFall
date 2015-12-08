using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LobbyCharacter : MonoBehaviour {

    public static readonly int group = (int)NetworkGroup.CharacterLobby;

    public GameObject lobbyCharacterViewPrefab;
    LobbyCharacterView lobbyCharacterView;

    private string _playerName = "";
    private CharacterColor _color = CharacterColor.White;
    private bool _isReady = false;

    public string playerName {
        get { return _playerName; }
        set {
            _playerName = value;
            networkView.RPC("SendPlayerName", RPCMode.Others, _playerName);
            Debug.Log("Set playerName");
        }
    }
    public CharacterColor color {
        get { return _color; }
        set {
            _color = value;
            networkView.RPC("SendColor", RPCMode.Others, (int)_color);
            Debug.Log("Set color: " + _color);
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
    ClientManager clientManager;

    void Start() {
        if (Network.isServer) {
            serverManager = ServerManager.Get();
            serverManager.lobbyCharacterList.Add(this);
            ChangeColor();
        }
        else if (Network.isClient) {
            clientManager = ClientManager.Get();
            if (!networkView.isMine) {
                var obj = Instantiate(lobbyCharacterViewPrefab) as GameObject;
                obj.transform.SetParent(clientManager.lobbyManager.playerList.transform);
                lobbyCharacterView = obj.GetComponent<LobbyCharacterView>();
                SyncAllData();
            }
            else {
                _playerName = clientManager.playerName;
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
    void SendColor(int color) {
        _color = (CharacterColor)color;
        Debug.Log("Got color " + _color);
        if (Network.isClient) {
            if (networkView.isMine) {
                Debug.Log("Set lobbymanager color");
                clientManager.lobbyManager.SetColor(_color.ToColor());
                clientManager.playerColor = _color;
            }
            else {
                Debug.Log("Show color");
                lobbyCharacterView.color = _color.ToColor();
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
            networkView.RPC("SendColor", RPCMode.Others, (int)_color);
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
            var length = Enum.GetValues(typeof(CharacterColor)).Length;
            var value = color;

            for (int i = 1; i < length; i++) {
                value++;

                if ((int)value == length) {
                    value = 0;
                };
 
                Debug.Log("Testing color: " + value);
                if (!serverManager.lobbyCharacterList.Exists(character => character.color == value)) {
                    color = value;
                    break;
                }
           };
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
