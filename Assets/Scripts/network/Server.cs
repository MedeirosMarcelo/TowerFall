using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Server : MonoBehaviour {

    public Button startButton { get; private set; }
    public InputField portInput { get; private set; }
    public Text statusText { get; private set; }

    NetworkView net;

    void Start() {
        net = GetComponent<NetworkView>();
        startButton = GameObject.FindGameObjectWithTag("Server Start").GetComponent<Button>();
        portInput = GameObject.FindGameObjectWithTag("Server Port").GetComponent<InputField>();
        statusText = GameObject.FindGameObjectWithTag("Server Status").GetComponent<Text>();

        startButton.onClick.AddListener(() => {
            if (Network.peerType == NetworkPeerType.Disconnected) { StartServer(); }
            else { StopServer(); }
        });
    }

    public delegate void OnStartServer();
    public OnStartServer onStartServer { get; set; }
    public void StartServer() {
        int port;
        if (!int.TryParse(portInput.text, out port)) { port = 25001; }

        Debug.Log("StartServer");
        if (onStartServer != null) { onStartServer(); }
        Network.InitializeServer(10, port);
    }

    public delegate void OnStopServer();
    public OnStopServer onStopServer { get; set; }
    public void StopServer() {
        Debug.Log("StopServer");
        if (onStopServer != null) { onStopServer(); }
        Network.Disconnect(250);
    }

    public delegate void OnServerStarted();
    public OnServerStarted onServerStarted { get; set; }
    void OnServerInitialized() {
        Debug.Log("OnServerInitialized");
        startButton.GetComponentInChildren<Text>().text = "Stop Server";
        statusText.text = "Server online\nConnections: " + Network.connections.Length;
        if (onServerStarted != null) { onServerStarted(); }
    }

    public delegate void OnServerStoped();
    public OnServerStoped onServerStoped { get; set; }
    void OnDisconnectedFromServer() {
        Debug.Log("OnDisconnectedFromServer");
        startButton.GetComponentInChildren<Text>().text = "Start Server";
        statusText.text = "Server offline";
        if (onServerStoped != null) { onServerStoped(); }
    }

    public delegate void OnConnected(NetworkPlayer player);
    public OnConnected onConnected { get; set; }
    void OnPlayerConnected(NetworkPlayer player) {
        Debug.Log("OnPlayerConnected");
        statusText.text = "Server online\nConnections: " + Network.connections.Length;
        if (onConnected != null) { onConnected(player); }
    }

    public delegate void OnDisconnected(NetworkPlayer player);
    public OnDisconnected onDisconnected { get; set; }
    void OnPlayerDisconnected(NetworkPlayer player) {
        Debug.Log("OnPlayerDisconnected");
        statusText.text = "Server online\nConnections: " + (Network.connections.Length - 1);
        if (onDisconnected != null) { onDisconnected(player); }
        Network.DestroyPlayerObjects(player);
    }

    [RPC]
    void Login(string playerName, NetworkMessageInfo info) {
    }
}