using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HUDArrowDisplay : MonoBehaviour {

    public IList<Image> slotList;
    public Sprite spriteBasicArrow;
    public Sprite spriteBombArrow;
    
    int arrowCount;
    Character targetPlayer;

    void Start() {
        slotList = transform.GetComponentsInChildren<Image>();
    }

    void Update() {
        UpdateArrowPanel();
    }

    public void UpdateArrowPanel() {
        if (targetPlayer == null) {
            targetPlayer = GameObject.FindWithTag("World Main").GetComponent<ClientManager>().character;
        }

        if (targetPlayer != null) {
            if (arrowCount != targetPlayer.arrows.stack.Count) {
                int i = 0;
                foreach (ArrowType arrowType in targetPlayer.arrows.stack) {
                    AddArrowIcon(i, arrowType);
                    i++;
                }
                for (int k = i; k < slotList.Count; k++) {
                    RemoveArrowIcon(k);
                }
                arrowCount = targetPlayer.arrows.stack.Count;
            }
        }
    }

    void AddArrowIcon(int i, ArrowType arrowType) {
        slotList[i].sprite = GetArrowIcon(arrowType);
        slotList[i].enabled = true;
    }

    void RemoveArrowIcon(int i) {
        slotList[i].sprite = null;
        slotList[i].enabled = false;
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