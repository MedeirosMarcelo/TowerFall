using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputFieldManager : MonoBehaviour {

    public delegate void OnSubmit();
    public OnSubmit onSubmit { get; set; }
    public Scrollbar scroolbar { get; private set; }
    public InputField inputField { get; private set; }
    public bool refocus = false;

    public string text {
        get { return inputField.text; }
        set { inputField.text = value; }
    }
    public bool interactable {
        get { return inputField.interactable; }
        set { inputField.interactable = value; }
    }
    void Awake() {
        inputField = GetComponent<InputField>();
    }
    void Start() {
        scroolbar = transform.parent.GetComponentInChildren<Scrollbar>();
        onSubmit += delegate () {
            if (refocus) {
                inputField.ActivateInputField();
            }
        };
        inputField.onEndEdit.AddListener(val => {
            if (Input.GetButtonDown("Submit")) {
                onSubmit();
            }
        });
    }

    void Update() {
        if (text == "\n") {
            text = "";
        }
    }
    public void Focus() {
        inputField.ActivateInputField();
        inputField.Select();
        text = "";
    }
}
