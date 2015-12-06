using UnityEngine;
using System.Collections;

public class ObjectReflection : MonoBehaviour {

	public GameObject original;

    public bool mirrorCharacterAnim;
    Animation animation;
    Animation originalAnim;
    CharacterController originalController;

	void Update () {
		MirrorActions ();
	}

	void MirrorActions(){
		if (original) {
			this.transform.localPosition = original.transform.position;
			this.transform.localRotation = original.transform.rotation;
            MirrorCharacterAnimations();
		}
	}

    void MirrorCharacterAnimations() {
        if (mirrorCharacterAnim) {
            animation = transform.Find("Model").GetComponent<Animation>();
            if (animation != null) {
                if (originalController == null) {
                    originalController = original.GetComponent<Character>().controller;
                }
                animation[originalController.animationPlaying].speed = originalController.animationSpeed;
                animation.CrossFade(originalController.animationPlaying);
            }
        }
    }
}