using UnityEngine;
using System.Collections;

public class DestroyOnTime : MonoBehaviour {

    public float time = 0f;

	void Update () {
        Destroy(this.gameObject, time);
	}
}
