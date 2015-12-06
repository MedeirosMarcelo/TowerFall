using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    private static GameManager gameManager = null;
    public static GameManager Get() {
        var obj = GameObject.FindWithTag("GameController");
        if (obj == null) {
            Debug.LogError("Game Manager Not Found");
            return null;
        }
        return obj.GetComponent<GameManager>();
    }

    public string playerName { get; set; }

    void Awake() {
        if (gameManager != null) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }
    void Start() {
        var obj = GameObject.FindGameObjectWithTag("Canvas");
        var menuManager = obj.GetComponent<MenuManager>();
        menuManager.startPanel.gameObject.SetActive(true);
    }
}
