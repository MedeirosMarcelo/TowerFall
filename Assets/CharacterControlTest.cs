using UnityEngine;
using System.Collections;

public class CharacterControlTest : MonoBehaviour {

    public GameObject camera;
    public float horizontal { get; private set; }
    public float vertical { get; private set; }
    public float lookHorizontal { get; private set; }
    public float lookVertical { get; private set; }
    Config config;

    float sensitivityX = 15F;
    float sensitivityY = 15F;
    float minimumX = -360F;
    float maximumX = 360F;
    float minimumY = -60F;
    float maximumY = 60F;
    float rotationY = 0F;

    // Use this for initialization
    void Start() {
        config = Config.keyboard;
        horizontal = 0f;
        vertical = 0f;
    }

    // Update is called once per frame
    void Update() {
        horizontal = Input.GetAxis(config.moveHorizontal);
        vertical = Input.GetAxis(config.moveVertical);
        lookHorizontal = Input.GetAxis(config.lookHorizontal);
        lookVertical = Input.GetAxis(config.lookVertical);
        Look();
    }

    void Look() {
        rotationY += lookVertical * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        //Camera
        var camTransform = camera.transform;
        camTransform.localEulerAngles = new Vector3(-rotationY, camTransform.localEulerAngles.y, 0);

        //player
        var charTransform = this.transform;
        float rotationX = charTransform.localEulerAngles.y + lookHorizontal * sensitivityX;
        charTransform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
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
            moveHorizontal = "Horizontal",
            moveVertical = "Vertical",
            lookHorizontal = "kLookHorizontal",
            lookVertical = "kLookVertical",
            shoot = "kShoot",
            dodge = "kDodge",
            jump = "kJump"
        };
    };
}
