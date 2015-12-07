using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChatManager : MonoBehaviour {

    [Header("Child Objects")]
    public TextScroolManager chat;
    public Image chatBackground;
    public Image slider;
    public InputFieldManager chatInput;
    public Image inputBackground;

    bool _isOpen = false;
    public bool isOpen {
        get { return _isOpen; }
        set {
            _isOpen = value;
            if (_isOpen) {
                chatBackground.color = Color.black;
                inputBackground.color = Color.white;
                slider.color = Color.white;
                chatInput.interactable = true;
                chatInput.Focus();
                clientManager.character.input.mode = CharacterInput.InputMode.OnChat;
            }
            else {
                chatBackground.color = new Color(0f, 0f, 0f, 0.2f);
                inputBackground.color = new Color(1f, 1f, 1f, 0.2f);
                slider.color = new Color(0f, 0f, 0f, 0f);
                chatInput.interactable = false;
                clientManager.character.input.mode = CharacterInput.InputMode.InGame;
            }
        }
    }

    ClientManager clientManager;

    void Start() {
        clientManager = ClientManager.Get();
        clientManager.chatManager = this;
        chatInput.interactable = false;
        chatBackground.color = new Color(0f, 0f, 0f, 0.2f);
        inputBackground.color = new Color(1f, 1f, 1f, 0.2f);
        slider.color = new Color(0f, 0f, 0f, 0f);

        chatInput.onSubmit += delegate () {
            chat.AddMessage("you: " + chatInput.text);
            clientManager.SendLobbyMessage(clientManager.playerName + ": " + chatInput.text);
        };

    }
}
