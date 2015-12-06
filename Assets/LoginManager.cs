using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoginManager : MonoBehaviour {
    public InputFieldManager nameInput;
    public Button loginButton;
    public InputField ipInput;
    public InputField portInput;
    GameManager gameManager;
    void Start() {
        gameManager = GameManager.Get();
        nameInput.onSubmit += delegate () {
            Connect();
        };
        loginButton.onClick.AddListener(() => {
            Connect();
        });
    }
    bool IsValidName(string name) {
        return (name.Length > 2);
    }
    public void Connect() {
        if (IsValidName(nameInput.text)) {
            gameManager.playerName = nameInput.text;
            int port;
            if (!int.TryParse(portInput.text, out port)) {
                port = 25001;
            }
            string ip = (ipInput.text == string.Empty) ? "127.0.0.1" : ipInput.text;

            Debug.Log("Connect");
            Network.Connect(ip, port);
        }
    }
}
