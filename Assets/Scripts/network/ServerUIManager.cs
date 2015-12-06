using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ServerUIManager : MonoBehaviour {

    public Button startButton;
    public InputField portInput;
    public Text statusText;


    void Start() {
        startButton.onClick.AddListener(() => {
            if (Network.peerType == NetworkPeerType.Disconnected) { StartServer(); }
            else { StopServer(); }
        });
    }

    public void StartServer() {
        int port;
        if (!int.TryParse(portInput.text, out port)) { port = 25001; }

        Debug.Log("StartServer");
        Network.InitializeServer(10, port);
    }

    public void StopServer() {
        Debug.Log("StopServer");
        Network.Disconnect(250);
    }

    void OnServerInitialized() {
        Debug.Log("OnServerInitialized");
        startButton.GetComponentInChildren<Text>().text = "Stop Server";
        statusText.text = "Server online\nConnections: " + Network.connections.Length;
    }

    void OnDisconnectedFromServer() {
        Debug.Log("OnDisconnectedFromServer");
        startButton.GetComponentInChildren<Text>().text = "Start Server";
        statusText.text = "Server offline";
    }

    void OnPlayerConnected(NetworkPlayer player) {
        Debug.Log("OnPlayerConnected");
        statusText.text = "Server online\nConnections: " + Network.connections.Length;
    }

    void OnPlayerDisconnected(NetworkPlayer player) {
        Debug.Log("OnPlayerDisconnected");
        statusText.text = "Server online\nConnections: " + (Network.connections.Length - 1);

        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }

    [RPC]
    void Login(string playerName, NetworkMessageInfo info) {
    }
    [RPC]
    void AddText(string text) {
    }
}
