using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HUDArrowDisplay : MonoBehaviour {

    public GameObject[] arrowIconArray = new GameObject[7];
    public int targetPlayerNumber;
    public GameObject arrowIconPrefab;
    public Sprite spriteBasicArrow;
    public Sprite spriteBombArrow;

    Character targetPlayer;
    Stack<ArrowType> arrowStack = new Stack<ArrowType>();

    private int arrowCount;

    void Start() {
        targetPlayer = GameObject.FindWithTag("World Main").GetComponent<ClientManager>().character;
        arrowStack = targetPlayer.arrows.stack;
    }

    public void Show() {
        arrowCount = arrowStack.Count;
    }

    public void Hide() {
        arrowCount = arrowStack.Count;
    }

    void Update() {
        UpdateArrowPanel();
    }

    public void UpdateArrowPanel() {
    
        if (targetPlayer != null) {
            if (arrowCount != arrowStack.Count) {
                int i = 0;
                foreach (ArrowType arrow in targetPlayer.arrows.stack) {
                    AddArrowIcon(i, arrow);
                    i++;
                }
                for (int k = i; k < arrowIconArray.Length; k++) {
                    RemoveArrowIcon(k);
                }
                arrowCount = targetPlayer.arrows.stack.Count;
            }
        }
    }

    void AddArrowIcon(int i, ArrowType arrowType) {
        if (i < arrowIconArray.Length) {
            Image arrowIcon = arrowIconArray[i].GetComponent<Image>();
            arrowIcon.sprite = GetArrowIcon(arrowType);
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
            case ArrowType.Bomb:
                return spriteBombArrow;
        }
    }
}
