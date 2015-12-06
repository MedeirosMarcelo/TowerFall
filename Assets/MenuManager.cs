using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

public enum MenuState {
    OnStart,
    OnLogin,
    OnLobby
}

public class MenuManager : MonoBehaviour {

    public static MenuManager Get() {
        var obj = GameObject.FindWithTag("Canvas");
        if (obj == null) {
            Debug.LogError("Menu Manager Not Found");
            return null;
        }
        return obj.GetComponent<MenuManager>();
    }

    public Button exitButton;
    public Button backButton;

    public StartManager startPanel;
    public LoginManager loginPanel;
    public LobbyManager lobbyPanel;

    MenuState state;
    GameManager gameManager;

    void Start() {
        gameManager = GameManager.Get();
        exitButton.onClick.AddListener(() => {
            Application.Quit();
        });
        backButton.onClick.AddListener(() => {
            Back();
        });
        startPanel.startButton.onClick.AddListener(() => {
            ChangeState(MenuState.OnLogin);
        });

        if (Network.isClient) {
            lobbyPanel.gameObject.SetActive(true);
        } else {
            startPanel.gameObject.SetActive(true);
        }
    }
    void Update() {
        if (Input.GetButtonDown("Escape")) {
            Back();
        }
    }

    public void ChangeState(MenuState newState) {
        if (state == newState) {
            return;
        }
        // state exit
        switch (state) {
            case MenuState.OnStart:
                startPanel.gameObject.SetActive(false);
                break;
            case MenuState.OnLogin:
                loginPanel.gameObject.SetActive(false);
                break;
            case MenuState.OnLobby:
                lobbyPanel.gameObject.SetActive(false);
                break;
            default:
                Debug.LogError("Wait what? " + state);
                break;
        }
        // state enter
        state = newState;
        switch (state) {
            case MenuState.OnStart:
                startPanel.gameObject.SetActive(true);
                break;
            case MenuState.OnLogin:
                loginPanel.gameObject.SetActive(true);
                break;
            case MenuState.OnLobby:
                lobbyPanel.gameObject.SetActive(true);
                break;
            default:
                Debug.LogError("Wait what? " + state);
                break;
        }
    }
    public void Back() {
        switch (state) {
            default:
            case MenuState.OnStart:
                break;
            case MenuState.OnLogin:
                ChangeState(MenuState.OnStart);
                break;
            case MenuState.OnLobby:
                Disconnect();
                break;
        }
    }
    public void Disconnect() {
        Debug.Log("Disconnect");
        Network.Disconnect(250);
    }
    void OnConnectedToServer() {
        Debug.Log("Connected");
        ChangeState(MenuState.OnLobby);
    }
    void OnDisconnectedFromServer() {
        Debug.Log("Disconnected");
        ChangeState(MenuState.OnStart);
    }
}
