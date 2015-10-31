using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Client : MonoBehaviour {

    public Button startButton { get; private set; }
    public InputField ipInput { get; private set; }
    public InputField portInput { get; private set; }
    public InputField nameInput { get; private set; }

    NetworkView net;

    void Start() {
        net = GetComponent<NetworkView>();
        startButton = GameObject.FindGameObjectWithTag("Client Start").GetComponent<Button>();
        ipInput = GameObject.FindGameObjectWithTag("Client IP").GetComponent<InputField>();
        portInput = GameObject.FindGameObjectWithTag("Client Port").GetComponent<InputField>();
        nameInput = GameObject.FindGameObjectWithTag("Client Name").GetComponent<InputField>();

        startButton.onClick.AddListener(() => {
            if (Network.peerType == NetworkPeerType.Disconnected) {
                Connect();
            }
            else {
                Disconnect();
            }
        });
    }

    bool IsValidName(string name) {
        return (name.Length > 2);
    }

    public void Connect() {
        if (IsValidName(nameInput.text)) {
            //playerName = nameInput.text;
            int port;
            if (!int.TryParse(portInput.text, out port)) {
                port = 25001;
            }
            string ip = (ipInput.text == string.Empty) ? "127.0.0.1" : ipInput.text;
            Network.Connect(ip, port);
            Debug.Log("Connect");
        }
    }

    public void Disconnect() {
        Network.Disconnect(250);
        Debug.Log("Disconnect");
    }

    void OnConnectedToServer() {
        Debug.Log("Connected to server");
        //net.RPC("Login", RPCMode.Server, playerName);
        startButton.GetComponentInChildren<Text>().text = "Logout";
        nameInput.interactable = false;
    }

    void OnDisconnectedFromServer() {
        Debug.Log("Disconnected");
        startButton.GetComponentInChildren<Text>().text = "Login";
        nameInput.interactable = true;
    }

    [RPC]
    void Login(string playerName, NetworkMessageInfo info) {
    }
}
