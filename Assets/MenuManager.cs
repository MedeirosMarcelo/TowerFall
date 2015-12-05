using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

public class MenuManager : MonoBehaviour {
    public GameObject background;
    public Button exitButton;
    public Button backButton;

    public GameObject menuPanel;
    public Button startButton;

    public GameObject loginPanel;
    public InputFieldManager nameInput;
    public Button loginButton;
    public InputField ipInput;
    public InputField portInput;

    public GameObject lobbyPanel;
    public ScrollRect scroolRect;
    public InputFieldManager chatInput;
    public Text textPrefab;

    public bool isConnected { get { return (Network.peerType != NetworkPeerType.Disconnected); } }
    public bool isDisconnected { get { return (Network.peerType == NetworkPeerType.Disconnected); } }

    enum State {
        OnMenu,
        OnLogin,
        OnLobby
    }
    State state = State.OnMenu;

    void ChangeState(State newState) {
        if (state == newState) {
            return;
        }
        // state exit
        switch (state) {
            case State.OnMenu:
                menuPanel.SetActive(false);
                break;
            case State.OnLogin:
                loginPanel.SetActive(false);
                break;
            case State.OnLobby:
                lobbyPanel.SetActive(false);

                break;
            default:
                Debug.LogError("Wait what? " + state);
                break;
        }
        // state enter
        state = newState;
        switch (state) {
            case State.OnMenu:
                menuPanel.SetActive(true);
                break;
            case State.OnLogin:
                loginPanel.SetActive(true);
                break;
            case State.OnLobby:
                lobbyPanel.SetActive(true);
                break;
            default:
                Debug.LogError("Wait what? " + state);
                break;
        }
    }

    //bool updateScrool;
    string playerName;


    void Start() {
        exitButton.onClick.AddListener(() => {
            //Application.Quit();
        });
        backButton.onClick.AddListener(() => {
            Back();
        });
        startButton.onClick.AddListener(() => {
            ChangeState(State.OnLogin);
        });
        nameInput.onSubmit += delegate () {
            Connect();
        };
        loginButton.onClick.AddListener(() => {
            Connect();
        });
    }
   void Update() {
        if (Input.GetButtonDown("Escape")) {
            Back();
       }
    }

    void Back() {
            switch (state) {
                default:
                case State.OnMenu:
                    break;
                case State.OnLogin:
                    ChangeState(State.OnMenu);
                    break;
                case State.OnLobby:
                    Disconnect();
                    break;
            }
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
        Network.Disconnect(250);
    }

    void OnConnectedToServer() {
        Debug.Log("Connected");
        ChangeState(State.OnLobby);
    }

    void OnDisconnectedFromServer() {
        Debug.Log("Disconnected");
        ChangeState(State.OnMenu);
    }
}
