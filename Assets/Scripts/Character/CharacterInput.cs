using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class CharacterInput {

    Character character;
    Config config;

    public enum Type {
        Keyboard,
        Controller1,
        Controller2
    };

    public float horizontal { get; private set; }
    public float vertical { get; private set; }
    public float lookHorizontal { get; private set; }
    public float lookVertical { get; private set; }

    public bool shoot { get; private set; }
    public bool dodge { get; private set; }
    public bool jump { get; private set; }

    float sensitivityX = 15F;
    float sensitivityY = 15F;
    float minimumX = -360F;
    float maximumX = 360F;
    float minimumY = -60F;
    float maximumY = 60F;
    float rotationY = 0F;
    bool mouseLook;

    public Vector3 vector {
        get { return new Vector3(horizontal, 0, vertical); }
    }

    public CharacterInput(Character character) {
        this.character = character;
        type = Type.Keyboard;
        horizontal = 0f;
        vertical = 0f;
        shoot = false;
        dodge = false;
        jump = false;
        HideCursor();

        // Make the rigid body not change rotation
        character.rigidbody.freezeRotation = true;
    }

    public void Update() {
        horizontal = Input.GetAxis(config.moveHorizontal);
        vertical = Input.GetAxis(config.moveVertical);
        lookHorizontal = Input.GetAxis(config.lookHorizontal);
        lookVertical = Input.GetAxis(config.lookVertical);

        // AccumulateButtons
        shoot |= Input.GetButtonDown(config.shoot);
        dodge |= Input.GetButtonDown(config.dodge);
        jump |= Input.GetButtonDown(config.jump);

        UpdateCursor();
        Look();
    }

    public void FixedUpdate() {
        shoot = false;
        dodge = false;
        jump = false;
    }

    void ShowCursor() {
        if (type == Type.Keyboard) {
            mouseLook = false;
            Screen.showCursor = true;
            Screen.lockCursor = false;
        }
    }

    void HideCursor() {
        if (type == Type.Keyboard) {
            mouseLook = true;
            Screen.showCursor = false;
            Screen.lockCursor = true;
        }
    }

    void UpdateCursor() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (Screen.showCursor) { HideCursor(); }
            else { ShowCursor(); }
        }
    }

    void Look() {
        if (mouseLook) {
            rotationY += lookVertical * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            //Camera
            var camTransform = character.charCamera.transform;
            camTransform.localEulerAngles = new Vector3(-rotationY, camTransform.localEulerAngles.y, 0);

            //player
            var charTransform = character.transform;
            float rotationX = charTransform.localEulerAngles.y + lookHorizontal * sensitivityX;
            charTransform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
    }

    Type _type;
    public Type type {
        get { return _type; }
        set {
            _type = value;
            switch (_type) {
                default:
                case Type.Keyboard:
                    config = Config.keyboard;
                    break;
                case Type.Controller1:
                    config = Config.controller1;
                    break;
                case Type.Controller2:
                    config = Config.controller2;
                    break;
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

        public static Config keyboard = new Config() {
            moveHorizontal = "kHorizontal",
            moveVertical = "kVertical",
            lookHorizontal = "kLookHorizontal",
            lookVertical = "kLookVertical",
            shoot = "kShoot",
            dodge = "kDodge",
            jump = "kJump"
        };

        public static Config controller1 = new Config() {
            moveHorizontal = "c1Horizontal",
            moveVertical = "c1Vertical",
            lookHorizontal = "c1LookHorizontal",
            lookVertical = "c1LookVertical",
            shoot = "c1Shoot",
            dodge = "c1Dodge",
            jump = "c1Jump"
        };

        public static Config controller2 = new Config() {
            moveHorizontal = "c2Horizontal",
            moveVertical = "c2Vertical",
            lookHorizontal = "c2LookHorizontal",
            lookVertical = "c2LookVertical",
            shoot = "c2Shoot",
            dodge = "c2Dodge",
            jump = "c2Jump"
        };
    };

}