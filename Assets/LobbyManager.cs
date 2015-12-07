using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyManager : MonoBehaviour {

    public Text headerText;
    public Button readyButton;
    public Text readyButtonText;
    public Button colorButton;
    public Image colorImage;
    public TextScroolManager chat;
    public InputFieldManager chatInput;
    public GameObject playerList;

    public GameObject lobbyCharacterPrefab;

    ClientManager gameManager;
    LobbyCharacter lobbyCharacter;
    // Start happenas after first OnEnable 
    private bool started = false;
    void Start() {
        started = true;
        gameManager = ClientManager.Get();
        gameManager.lobbyManager = this;
        colorButton.onClick.AddListener(() => {
            // use callbacks so we can change local variables without need to add new listeners
            ChangeColorCallback();
        });
        readyButtonText.text = "Ready";
        readyButton.onClick.AddListener(() => {
            // use callbacks so we can change local variables without need to add new listeners
            ReadyButtonCallback();
        });
        chatInput.onSubmit += delegate () {
            chat.AddMessage("you: " + chatInput.text);
            gameManager.SendLobbyMessage(gameManager.playerName + ": " + chatInput.text);
        };
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
    public void SetColor(Color color) {
        colorImage.color = color;
    }
}
