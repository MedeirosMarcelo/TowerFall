using UnityEngine;
using System.Collections;

public class HUDManager : MonoBehaviour {

    [Header("Children Objects")]
    public GameObject background;
    public TextScroolManager chat;
    public InputFieldManager chatInput;

    string playerName;
    bool updateScrool;
    bool lockCursor = false;
    bool isChatOpen = false;
    bool isMenuOpen = true;

    ClientManager clientManager;

    void Start() {
        clientManager = ClientManager.Get();
        clientManager.hudManager = this;
        chatInput.interactable = false;
        chatInput.onSubmit += delegate () {
            chat.AddMessage("you: " + chatInput.text);
            clientManager.SendChatMessage(clientManager.playerName + ": " + chatInput.text);
        };
    }
    public void Update() {
        // Close chat on scape or open/close menu
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
            // chat input is auto selected after releasing chat button
            if (!(chatInput.interactable) && Input.GetButtonUp("Chat")) {
                chatInput.interactable = true;
                chatInput.Focus();
            }
        }
        else if (!isMenuOpen && Input.GetButtonDown("Chat")) {
            OpenChat();
        }
        // if cursor is not locked as desired we relock/unlock it here
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
        clientManager.character.input.mode = CharacterInput.InputMode.OnMenu;
    }
    private void CloseMenu() {
        Debug.Log("Close Menu");
        lockCursor = true;
        isMenuOpen = false;
        clientManager.character.input.mode = CharacterInput.InputMode.InGame;
    }
    public void OpenChat() {
        Debug.Log("Open Chat");
        isChatOpen = true;
        clientManager.character.input.mode = CharacterInput.InputMode.OnChat;
    }
    public void CloseChat() {
        Debug.Log("Close Chat");
        isChatOpen = false;
        clientManager.character.input.mode = CharacterInput.InputMode.InGame;
    }
}
