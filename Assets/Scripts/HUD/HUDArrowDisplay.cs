using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDArrowDisplay : MonoBehaviour {

    public GameObject[] arrowIconArray = new GameObject[7];
    public int targetPlayerNumber;
    public GameObject arrowIconPrefab;
    public Sprite spriteBasicArrow;

    GameManager gameManager;
    Character targetPlayer;

    private int arrowCount;

    void Start() {
        gameManager = GameObject.FindWithTag("World Main").GetComponent<GameManager>();
        targetPlayer = gameManager.playerList[targetPlayerNumber - 1].GetComponent<Character>();
        arrowCount = targetPlayer.arrows.arrowList.Count;
    }

    void Update() {
        UpdateArrowPanel();
    }

    public void UpdateArrowPanel() {
        if (targetPlayer != null) {
            if (arrowCount != targetPlayer.arrows.arrowList.Count) {
                int i = 0;
                foreach (GameObject arrowObj in targetPlayer.arrows.arrowList) {
                    AddArrowIcon(i, arrowObj);
                    i++;
                }
                for (int k = i; k < arrowIconArray.Length; k++) {
                    RemoveArrowIcon(k);
                }
                arrowCount = targetPlayer.arrows.arrowList.Count;
            }
        }
    }

    void AddArrowIcon(int i, GameObject arrowObj) {
        if (i < arrowIconArray.Length) {
            Image arrowIcon = arrowIconArray[i].GetComponent<Image>();
            Arrow arrow = arrowObj.GetComponent<Arrow>();
            arrowIcon.sprite = GetArrowIcon(arrow.type);
            arrowIcon.enabled = true;
        }
    }

    void RemoveArrowIcon(int i) {
        if (i < arrowIconArray.Length) {
            Image arrowIcon = arrowIconArray[i].GetComponent<Image>();
            arrowIcon.enabled = false;
        }
    }

    Sprite GetArrowIcon(ArrowType arrowType) {
        switch (arrowType) {
            default:
            case ArrowType.Basic:
                return spriteBasicArrow;
        }
    }
}
