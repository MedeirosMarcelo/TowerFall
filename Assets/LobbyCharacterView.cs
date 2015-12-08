using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyCharacterView : MonoBehaviour {
    public Text nameText;
    public Image colorImage;
    public Image readyImage;

    public string playerName {
        set { nameText.text = value; }
    }
    public Color color {
        set { colorImage.color = value; }
    }
    public bool isReady {
        set { readyImage.gameObject.SetActive(value);  }
    }
    public void Set(LobbyCharacter lobbyCharacter) {
        playerName = lobbyCharacter.playerName;
        color = lobbyCharacter.color.ToColor();
        isReady = lobbyCharacter.isReady;
    }
}
