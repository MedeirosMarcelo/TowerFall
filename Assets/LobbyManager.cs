using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.EventSystems;
using System.Collections;

public class LobbyManager : MonoBehaviour {
    public GameObject header;
    public Button colorButton;

    public Button readyButton;
    public TextScroolManager chat;
    public InputFieldManager chatInput;
    public GameObject playerList;

    public GameObject lobbyCharacterPrefab;

    GameManager gameManager;
    LobbyCharacter lobbyCharacter;
    Text headerText;
    Text readyButtonText;
    Image colorView;

    // Start happenas after first OnEnable 
    private bool started = false;
    void Start() {
        started = true;
        gameManager = GameManager.Get();
        headerText =  header.GetComponentInChildren<Text>();
        readyButtonText = readyButton.GetComponentInChildren<Text>();
        colorView = colorButton.GetComponentInChildren<Image>();
        gameManager.onLobbyMessage += chat.AddMessage;
        colorButton.onClick.AddListener(() => {
            // use callbacks so we can change local variables without need to add new listeners
            ChangeColorCallback();
        });
        readyButtonText.text = "Ready";
        readyButton.onClick.AddListener(() => {
            // use callbacks so we can change local variables without need to add new listeners
            ReadyButtonCallback();
        });
        BuildLobbyCharacter();
    }

    void OnEnable() {
        // First Enable happens before start cousing errors so we skip it here and start will build lobbyCharacter
        if (started) {
            BuildLobbyCharacter();
        }
    }
    void BuildLobbyCharacter() {
        headerText.text = gameManager.playerName;
        readyButtonText.text = "Ready";
        var obj = Network.Instantiate(lobbyCharacterPrefab,
                                      new Vector3(),
                                      new Quaternion(),
                                      (int)NetworkGroup.CharacterLobby) as GameObject;
        lobbyCharacter = obj.GetComponent<LobbyCharacter>();
        lobbyCharacter.lobbyManager = this;
    }
    void OnDisable() {
        lobbyCharacter.Destroy();
    }

    // This callbacks use local variable
    void ChangeColorCallback() {
        lobbyCharacter.ChangeColor();
    }
    void ReadyButtonCallback() {
        if (lobbyCharacter.isReady) {
            lobbyCharacter.isReady = false;
            readyButtonText.text = "Ready";
        }
        else {
            lobbyCharacter.isReady = true;
            readyButtonText.text = "Cancel";
        }
    }
    void SetColor(Color color) {
        colorView.color = color;
    }
}
