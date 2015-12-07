using UnityEngine;
using System.Collections;

public class HUDManager : MonoBehaviour {

    public GameObject background;
    public TextScroolManager chat;
    public InputFieldManager chatInput;

    string playerName;
    bool updateScrool;
    bool lockCursor = false;
    bool isChatOpen = false;
    bool isMenuOpen = true;

    GameManager gameManager;

    void Start() {
        gameManager = GameManager.Get();
        gameManager.hudManager = this;
        chatInput.interactable = false;
        chatInput.onSubmit += delegate () {
            chat.AddMessage("you: " + chatInput.text);
            gameManager.SendChatMessage(gameManager.playerName + ": " + chatInput.text);
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
    }
    private void CloseMenu() {
        Debug.Log("Close Menu");
        lockCursor = true;
        isMenuOpen = false;
    }
    public void OpenChat() {
        Debug.Log("Open Chat");
        isChatOpen = true;
    }
    public void CloseChat() {
        Debug.Log("Close Chat");
        isChatOpen = false;
    }
}
