using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class CharacterInput {

    Character character;
    Config config;

    public float horizontal { get; private set; }

    public float vertical { get; private set; }

    public float lookHorizontal { get; private set; }

    public float lookVertical { get; private set; }

    public bool shoot { get; private set; }

    public bool dodge { get; private set; }

    public bool jump { get; private set; }

    public bool submit { get; private set; }

    public bool cancel { get; private set; }

    public bool escape { get; private set; }

    public Vector3 vector {
        get { return new Vector3(horizontal, 0, vertical); }
    }

    public CharacterInput(Character character) {
        this.character = character;
        type = Type.Keyboard;
        horizontal = 0f;
        vertical = 0f;
        submit = false;
        cancel = false;
        escape = false;
        shoot = false;
        dodge = false;
        jump = false;

    }

    public void Update() {
        horizontal = Input.GetAxis(config.moveHorizontal);
        vertical = Input.GetAxis(config.moveVertical);
        lookHorizontal = Input.GetAxis(config.lookHorizontal);
        lookVertical = Input.GetAxis(config.lookVertical);

        // this buttons should be read at update
        submit = Input.GetButtonDown(config.submit);
        cancel = Input.GetButtonDown(config.cancel);
        escape = Input.GetButtonDown(config.escape);

        // AccumulateButtons
        shoot |= Input.GetButtonDown(config.shoot);
        dodge |= Input.GetButtonDown(config.dodge);
        jump |= Input.GetButtonDown(config.jump);

        /*
        Debug.Log("horizontal:" + horizontal +
            "\nvertical:" + vertical +
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

    public enum Type {
        Keyboard,
        Controller
    };
    Type _type;

    public Type type {
        get { return _type; }
        set {
            _type = value;
            config = (_type == Type.Controller) ? Config.controller : Config.keyboard;
        }
    }
}

public class Config {
    public string moveHorizontal;
    public string moveVertical;
    public string lookHorizontal;
    public string lookVertical;
    public string shoot;
    public string dodge;
    public string jump;
    public string escape;
    public string submit;
    public string cancel;
    public static Config keyboard = new Config() {
        moveHorizontal = "kHorizontal",
        moveVertical = "kVertical",
        lookHorizontal = "kLookHorizontal",
        lookVertical = "kLookVertical",
        shoot = "kShoot",
        dodge = "kDodge",
        jump = "kJump",
        escape = "kEscape",
        submit = "kSubmit",
        cancel = "kCancel"
    };
    public static Config controller = new Config() {
        moveHorizontal = "cHorizontal",
        moveVertical = "cVertical",
        lookHorizontal = "cLookHorizontal",
        lookVertical = "cLookVertical",
        shoot = "cShoot",
        dodge = "cDodge",
        jump = "cJump",
        escape = "cEscape",
        submit = "cSubmit",
        cancel = "cCancel"
    };
};