using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.EventSystems;
using System.Collections;

public class LobbyManager : MonoBehaviour {
    public GameObject header;
    public Button colorButton;
    public Image colorView;
    public Button readyButton;
    public ScrollRect scroolRect;
    public InputFieldManager chatInput;
    public GameObject playerList;

    public Text textPrefab;
    public GameObject lobbyCharacterPrefab;

    GameManager gameManager;
    LobbyCharacter lobbyCharacter;

    void Awake() {
        // WARNING
        // this can only be here because This script is started disabled
        //  It however cannot be o Start because it happens after OnEnable
        gameManager = GameManager.Get();
    }

    void OnEnable() {
        header.GetComponentInChildren<Text>().text = gameManager.playerName;
        var obj = Network.Instantiate(lobbyCharacterPrefab,
                                      new Vector3(),
                                      new Quaternion(),
                                      (int)NetworkGroup.CharacterLobby) as GameObject;
        lobbyCharacter = obj.GetComponent<LobbyCharacter>();
        lobbyCharacter.lobbyManager = this;

        colorButton.onClick.AddListener(() => {
            lobbyCharacter.ChangeColor();
        });

        var readyButtonText = readyButton.GetComponentInChildren<Text>();
        readyButton.onClick.AddListener(() => {
            if (lobbyCharacter.isReady) {
                lobbyCharacter.isReady = false;
                readyButtonText.text = "Cancel";
            }
            else {
                lobbyCharacter.isReady = false;
                readyButtonText.text = "Ready";
            }
        });
    }
    void OnDisable() {
        lobbyCharacter.Destroy();
    }
    void SetColor(Color color) {
        colorView.color = color;
    }


}
