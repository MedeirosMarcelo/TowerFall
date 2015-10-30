using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Server : MonoBehaviour {

    public Button startButton { get; private set; }
    public InputField portInput { get; private set; }
    public Text statusText { get; private set; }


    string playerName;
    NetworkView net;

    void Start() {

        net = GetComponent<NetworkView>();
        startButton = GameObject.FindGameObjectWithTag("Server Start").GetComponent<Button>();
        portInput = GameObject.FindGameObjectWithTag("Server Port").GetComponent<InputField>();
        statusText = GameObject.FindGameObjectWithTag("Server Status").GetComponent<Text>();

        startButton.onClick.AddListener(() => {
            if (Network.peerType == NetworkPeerType.Disconnected) {
                int port;
                if (!int.TryParse(portInput.text, out port)) {
                    port = 25001;
                }
                Network.InitializeServer(10, port);
            }
            else {
                Network.Disconnect(250);
            }
        });
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
        }


    [RPC]
    void Login(string playerName, NetworkMessageInfo info) {
    }
}