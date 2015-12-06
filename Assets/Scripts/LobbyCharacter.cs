using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LobbyCharacter : MonoBehaviour {

    public static readonly int group = (int)NetworkGroup.CharacterLobby;
    public static HashSet<LobbyCharacter> lobbyCharacterSet = new HashSet<LobbyCharacter>();

    public LobbyManager lobbyManager { get; set; }
    GameObject lobbyCharacterView;

    private string _playerName = "";
    private Color _color = new Color();
    private bool _isReady = false;

    public string playerName {
        get { return _playerName; }
        set {
            if (networkView.isMine) {
                _playerName = value;
                networkView.RPC("SetPlayerName", RPCMode.Others, _playerName);
            }
        }
    }
    public Color color {
        get { return _color; }
    }
    public bool isReady {
        get { return _isReady; }
        set {
            if (networkView.isMine) {
                _isReady = value;
                networkView.RPC("SetIsReady", RPCMode.Others, _isReady);
            }
        }
    }

    void Start() {
        lobbyCharacterSet.Add(this);
    }
    void OnDestroy() {
        lobbyCharacterSet.Remove(this);
    }
    void OnDisconnectedFromServer() {
        Destroy(gameObject);
    }

   [RPC]
    void SetPlayerName(string playerName) {
        if (!networkView.isMine) {
            _playerName = playerName;
        }
    }
    [RPC]
    public void ChangeColor() {
        if (networkView.isMine) {
            networkView.RPC("ChangeColor", RPCMode.Server);
            return;
        }
        if (Network.isServer) {
            _color = Color.red;
            networkView.RPC("SetColor", RPCMode.Others, _color.r, _color.g, _color.b, _color.a);
        }
    }
    [RPC]
    void SetColor(float r, float g, float b, float a) {
        if (Network.isClient) {
            _color = new Color(r, g, b, a);
            if (networkView.isMine) {

            }
        }
    }
    [RPC]
    void SetIsReady(bool isReady) {
        if (!networkView.isMine) {
            _isReady = isReady;
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
        if (networkView.isMine) {
            networkView.RPC("Destroy", RPCMode.Server);
            Debug.Log("Destroy Character Lobby" + GetInstanceID());
        }
        else {
            Debug.LogError("Destroy Character Lobby" + GetInstanceID());
        }
    }
}
