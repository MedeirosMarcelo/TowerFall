using UnityEngine;
using System.Collections;

public class HUDManager : MonoBehaviour {

    [Header("Children Objects")]
    public ChatManager chat;
    public InputFieldManager chatInput;

    string playerName;
    bool updateScrool;
    bool lockCursor = false;

    bool isMenuOpen = true;

    ClientManager clientManager;

    void Start() {
        clientManager = ClientManager.Get();
    }

    public void Update() {

        // Close chat on scape or open/close menu
        if (Input.GetButtonDown("Escape")) {
            if (chat.isOpen) {
                chat.isOpen = false;
                return;
            }
            if (isMenuOpen) {
                CloseMenu();
            }
            else {
                OpenMenu();
            }
        }
        if (Input.GetButtonUp("Chat") && !chat.isOpen & !isMenuOpen) {
            chat.isOpen = true;
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
        clientManager.character.input.mode = CharacterInput.InputMode.OnMenu;
    }
    private void CloseMenu() {
        Debug.Log("Close Menu");
        lockCursor = true;
        isMenuOpen = false;
        clientManager.character.input.mode = CharacterInput.InputMode.InGame;
    }

}
