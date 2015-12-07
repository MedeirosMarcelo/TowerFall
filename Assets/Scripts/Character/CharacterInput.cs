using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class CharacterInput {

    string moveHorizontalAxis = "Horizontal";
    string moveVerticalAxis = "Vertical";
    string lookHorizontalAxis = "LookHorizontal";
    string lookVerticalAxis = "LookVertical";
    string shootButton = "Shoot";
    string dodgeButton = "Dodge";
    string jumpButton = "Jump";
    string escapeButton = "Escape";
    string submitButton = "Submit";
    string cancelButton = "Cancel";

    public enum InputMode {
        InGame,
        OnChat,
        OnMenu
    }
    public InputMode mode = InputMode.InGame;

    Character character;

    public float moveHorizontal { get; private set; }

    public float moveVertical { get; private set; }

    public float lookHorizontal { get; private set; }

    public float lookVertical { get; private set; }

    public bool shoot { get; private set; }

    public bool dodge { get; private set; }

    public bool jump { get; private set; }

    public bool submit { get; private set; }

    public bool cancel { get; private set; }

    public bool escape { get; private set; }

    public Vector3 vector {
        get { return new Vector3(moveHorizontal, 0, moveVertical); }
    }

    public CharacterInput(Character character) {
        this.character = character;
        moveHorizontal = 0f;
        moveVertical = 0f;
        submit = false;
        cancel = false;
        escape = false;
        shoot = false;
        dodge = false;
        jump = false;

    }

    public void Update() {
        switch (mode) {
            case InputMode.InGame:
                lookHorizontal = Input.GetAxis(lookHorizontalAxis);
                lookVertical = Input.GetAxis(lookVerticalAxis);
                moveHorizontal = Input.GetAxis(moveHorizontalAxis);
                moveVertical = Input.GetAxis(moveVerticalAxis);
                // AccumulateButton
                jump |= Input.GetButtonDown(jumpButton);
                shoot |= Input.GetButtonDown(shootButton);
                dodge |= Input.GetButtonDown(dodgeButton);
                break;
            case InputMode.OnChat:
                lookHorizontal = Input.GetAxis(lookHorizontalAxis);
                lookVertical = Input.GetAxis(lookVerticalAxis);
                moveHorizontal = 0f;
                moveVertical = 0f;
                // AccumulateButton
                jump = false;
                shoot = false;
                dodge = false;
                break;
            default:
            case InputMode.OnMenu:
                lookHorizontal = 0f;
                lookVertical = 0f;
                moveHorizontal = 0f;
                moveVertical = 0f;
                // AccumulateButton
                jump = false;
                shoot = false;
                dodge = false;
                break;
        }

        // this buttons should be read at update
        submit = Input.GetButtonDown(submitButton);
        cancel = Input.GetButtonDown(cancelButton);
        escape = Input.GetButtonDown(escapeButton);

        /*
        Debug.Log("movoHorizontal:" + moveHorizontal +
            "\nmoveVertical:" + moveVertical +
            "\nlookHorizontal:" + lookHorizontal +
            "\nlookVertical:" + lookVertical +
            "\nsubmit:" + submit +
            "\ncancel:" + cancel +
            "\nescape:" + escape +
            "\nshoot:" + shoot +
            "\ndodge:" + dodge +
            "\njump:" + jump
        );
        */
    }

    public void FixedUpdate() {
        shoot = false;
        dodge = false;
        jump = false;
    }


}
