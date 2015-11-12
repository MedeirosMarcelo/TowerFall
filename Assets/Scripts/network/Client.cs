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

    public delegate void OnConnect();
    public OnConnect onConnect { get; set; }

    public delegate void OnConnected();
    public OnConnected onConnected { get; set; }

    public delegate void OnDisconnect();
    public OnDisconnect onDisconnect { get; set; }

    public delegate void OnDisconnected();
    public OnDisconnected onDisconnected { get; set; }


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

            Debug.Log("Connect");
            if (onConnect != null) { onConnect(); }
            Network.Connect(ip, port);
        }
    }

    public void Disconnect() {
        Debug.Log("Disconnect");
        if (onDisconnect != null) { onDisconnect(); }
        Network.Disconnect(250);
    }

    void OnConnectedToServer() {
        Debug.Log("Connected");
        //net.RPC("Login", RPCMode.Server, playerName);
        startButton.GetComponentInChildren<Text>().text = "Logout";
        nameInput.interactable = false;

        if (onConnected != null) { onConnected(); }
    }

    void OnDisconnectedFromServer() {
        Debug.Log("Disconnected");
        startButton.GetComponentInChildren<Text>().text = "Login";
        nameInput.interactable = true;

        if (onDisconnected != null) { onDisconnected(); }
    }

    [RPC]
    void Login(string playerName, NetworkMessageInfo info) {
    }
}
