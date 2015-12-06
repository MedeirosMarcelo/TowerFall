using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextScroolManager : MonoBehaviour {
    public bool autoScrool = true;
    public GameObject textPrefab;

    private bool updateScrool;

    ScrollRect scrool;

    void Start() {
        scrool = gameObject.GetComponent<ScrollRect>();
        scrool.onValueChanged.AddListener(val => {
            if (updateScrool) {
                scrool.verticalNormalizedPosition = 0;
                scrool.verticalScrollbar.value = 0;
                updateScrool = false;
            }
        });
    }

    public void UpdateScrool() {
        updateScrool = true;
    }

    public void AddMessage(string msg) {
        var obj = Instantiate(textPrefab) as GameObject;
        var newText = obj.GetComponent<Text>();
        newText.text = msg;
        newText.rectTransform.SetParent(scrool.content.transform);
        if (autoScrool) {
            updateScrool = true;
        }
    }
}
