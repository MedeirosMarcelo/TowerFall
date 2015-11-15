using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClickSetActive : MonoBehaviour {

    public GameObject obj;
    public KeyCode toggleKey = KeyCode.F1;

    void Update() {
        if (Input.GetKeyDown(toggleKey)) {
            OnClick();
        }
    }

    public void OnClick() {

        if (obj.activeSelf == true) {
            obj.SetActive(false);
        }
        else {
            obj.SetActive(true);
        }
    }
}
