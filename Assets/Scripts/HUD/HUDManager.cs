using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

    public GameObject arrowPanel;
    public GameObject[] arrowIconArray = new GameObject[7];

    public GameObject arrowIconPrefab;
    public Sprite spriteBasicArrow;

    WorldMirror worldMirror;
    Character mainPlayer;

    private int arrowCount;

    void Start() {
        worldMirror = GameObject.FindWithTag("World Main").GetComponent<WorldMirror>();
        mainPlayer = worldMirror.Player.GetComponent<Character>();
        arrowCount = mainPlayer.arrows.arrowList.Count;
    }

    void Update() {
        UpdateArrowPanel();
    }

    public void UpdateArrowPanel() {
        if (arrowCount != mainPlayer.arrows.arrowList.Count) {
            int i = 0;
            foreach (GameObject arrowObj in mainPlayer.arrows.arrowList) {
                AddArrowIcon(i, arrowObj);
                i++;
            }
            for (int k = i; k < arrowIconArray.Length; k++) {
                RemoveArrowIcon(k);
            }
            arrowCount = mainPlayer.arrows.arrowList.Count;
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
