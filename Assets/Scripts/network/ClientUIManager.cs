using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

public class ClientUIManager : MonoBehaviour {

    public Text PrefabText;

    public GameObject background { get; private set; }

    public GameObject loginPanel { get; private set; }
    public Animator loginAnimator { get; private set; }
    public Button loginButton { get; private set; }
    public InputField ipInput { get; private set; }
    public InputField portInput { get; private set; }
    public InputFieldManager nameInput { get; private set; }

    public GameObject chatPanel { get; private set; }
    public Animator chatAnimator { get; private set; }
    public InputFieldManager chatInput { get; private set; }
    public ScrollRect scroolRect { get; private set; }


    public bool isConnected { get { return (Network.peerType != NetworkPeerType.Disconnected); } }

    ClientManager clientManager;

    string playerName;
    bool updateScrool;
    bool lockCursor = false;
    bool isChatOpen = false;
    bool isMenuOpen = true;


    void Start() {

        clientManager = GetComponent<ClientManager>();

        background = GameObject.FindGameObjectWithTag("UI Background");

        loginPanel = GameObject.FindGameObjectWithTag("UI Login Panel");
        loginAnimator = loginPanel.GetComponent<Animator>();
        ipInput = GameObject.FindGameObjectWithTag("UI IP Input").GetComponent<InputField>();
        portInput = GameObject.FindGameObjectWithTag("UI Port Input").GetComponent<InputField>();
        nameInput = GameObject.FindGameObjectWithTag("UI Name Input").GetComponent<InputFieldManager>();
        loginButton = GameObject.FindGameObjectWithTag("UI Login Button").GetComponent<Button>();

        chatPanel = GameObject.FindGameObjectWithTag("UI Chat Panel");
        chatInput = GameObject.FindGameObjectWithTag("UI Chat Input").GetComponent<InputFieldManager>();
        scroolRect = GameObject.FindGameObjectWithTag("UI Chat Scrool").GetComponent<ScrollRect>();
        chatAnimator = chatPanel.GetComponent<Animator>();
        chatInput.interactable = false;

        nameInput.onSubmit += delegate () {
                Connect();
        };
        loginButton.onClick.AddListener(() => {
            if (isConnected) {
                Disconnect();
            }
            else {
                Connect();
            }
        });

        scroolRect.onValueChanged.AddListener(val => {
            if (updateScrool) {
                scroolRect.verticalNormalizedPosition = 0;
                scroolRect.verticalScrollbar.value = 0;
                updateScrool = false;
            }
        });
        chatInput.onSubmit += delegate () {
            SendText(playerName + ": " + chatInput.text);
        };
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

            Debug.Log("Connect");
            Network.Connect(ip, port);
        }
    }

    public void Disconnect() {
        Debug.Log("Disconnect");
        //clientManager.OnDisconnect();
        Network.Disconnect(250);
    }

    void OnConnectedToServer() {
        Debug.Log("Connected");
        loginButton.GetComponentInChildren<Text>().text = "Logout";
        ipInput.interactable = false;
        portInput.interactable = false;
        nameInput.interactable = false;
        background.SetActive(false);
        CloseMenu();
        //clientManager.OnConnected();
    }

    void OnDisconnectedFromServer() {
        Debug.Log("Disconnected");
        loginButton.GetComponentInChildren<Text>().text = "Login";
        ipInput.interactable = true;
        portInput.interactable = true;
        nameInput.interactable = true;
        background.SetActive(true);
        OpenMenu();
    }

    public void Update() {
        /* Close chat on scape or open/close menu */
        if (Input.GetButtonDown("Escape")) {
            if (isChatOpen) {
                CloseChat();
                return;
            }
            if (isMenuOpen) {
                CloseMenu();
            }
            else {
                OpenMenu();
            }
        }
        if (isChatOpen) {
            /* chat input is auto selected after releasing chat button */
            if (!(chatInput.interactable) && Input.GetButtonUp("Chat")) {
                chatInput.interactable = true;
                chatInput.Focus();
            }
        }
        else if (!isMenuOpen && Input.GetButtonDown("Chat")) {
            OpenChat();
        }
        /* if cursor is not locked as desired we relock/unlock it here */
        if (lockCursor && !Screen.lockCursor) {
            Screen.lockCursor = true;
        }
        if (!lockCursor && Screen.lockCursor) {
            Screen.lockCursor = false;
        }
    }
    private void OpenMenu() {
        Debug.Log("Open Menu");
        lockCursor = false;
        isMenuOpen = true;
        Time.timeScale = 1;
        loginAnimator.Play("Slide In");
        clientManager.OnMenuOpen();
    }
    private void CloseMenu() {
        Debug.Log("Close Menu");
        lockCursor = true;
        isMenuOpen = false;
        Time.timeScale = 1;
        loginAnimator.Play("Slide Out");
        clientManager.OnMenuClosed();
    }
    public void OpenChat() {
        Debug.Log("Open Chat");
        isChatOpen = true;
        Time.timeScale = 1;
        chatAnimator.Play("Chat Panel Slide In");
        clientManager.OnChatOpened();
    }
    public void CloseChat() {
        Debug.Log("Close Chat");
        chatInput.interactable = false;
        isChatOpen = false;
        Time.timeScale = 1;
        chatAnimator.Play("Chat Panel Slide Out");
        clientManager.OnChatClosed();
    }

    void SendText(string text) {
        AddText(text);
        if (isConnected) {
            networkView.RPC("AddText", RPCMode.Others, text);
        }
    }
    [RPC]
    void AddText(string text) {
        var newText = Instantiate(PrefabText) as Text;
        newText.text = text;
        newText.rectTransform.SetParent(scroolRect.content.transform);
        updateScrool = true;
    }
}
