using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Client : MonoBehaviour {
    /*
    public InputField ipInput;
    public InputField portInput;

    public Text PrefabText;

    public Button connectionButton { get; private set; }
    public InputFieldManager nameInput { get; private set; }
    public InputFieldManager commandInput { get; private set; }
    public ScrollRect scroolRect { get; private set; }
    bool updateScrool;

    string playerName;
    NetworkView net;

    void Start() {
        net = GetComponent<NetworkView>();
        connectionButton = GameObject.FindGameObjectWithTag("Connection").GetComponent<Button>();
        nameInput = GameObject.FindGameObjectWithTag("Name").GetComponent<InputFieldManager>();
        commandInput = GameObject.FindGameObjectWithTag("Command").GetComponent<InputFieldManager>();
        scroolRect = GameObject.FindGameObjectWithTag("Scrool").GetComponent<ScrollRect>();

        scroolRect.onValueChanged.AddListener(val => {
            if (updateScrool) {
                Debug.Log("Update Scrool");
                scroolRect.verticalNormalizedPosition = 0;
                scroolRect.verticalScrollbar.value = 0;
                updateScrool = false;
            }
        });

        commandInput.onSubmit += delegate () {
            if (Network.peerType != NetworkPeerType.Disconnected) {
                net.RPC("Command", RPCMode.Server, commandInput.text);
            }
        };
        nameInput.onSubmit += delegate () {
            if (Network.peerType == NetworkPeerType.Disconnected) {
                Connect();
            }
        };

        connectionButton.onClick.AddListener(() => {
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
            playerName = nameInput.text;
            int port;
            if (!int.TryParse(portInput.text, out port)) {
                port = 25001;
            }
            string ip = (ipInput.text == string.Empty) ? "127.0.0.1" : ipInput.text;
            Debug.Log(ip + ":" + port);
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
        net.RPC("Login", RPCMode.Server, playerName);
        connectionButton.GetComponentInChildren<Text>().text = "Logout";
        nameInput.interactable = false;
        commandInput.interactable = true;

        EventSystem.current.SetSelectedGameObject(commandInput.gameObject);
    }
    void OnDisconnectedFromServer() {
        Debug.Log("Disconnected");
        connectionButton.GetComponentInChildren<Text>().text = "Login";
        nameInput.interactable = true;
        commandInput.interactable = false;
    }

    [RPC]
    void AddText(string text) {
        Debug.Log("AddText: " + text);
        var newText = Instantiate(PrefabText) as Text;
        newText.text = text;
        newText.rectTransform.SetParent(scroolRect.content.transform);
        updateScrool = true;
    }

    [RPC]
    void Login(string playerName, NetworkMessageInfo info) {
    }

    [RPC]
    void Command(string command, NetworkMessageInfo info) {
    }
    */
}
