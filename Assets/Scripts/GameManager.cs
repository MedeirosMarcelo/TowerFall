using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject basicArrow;
    WorldMirror worldMirror;

    void Start() {
        worldMirror = GetComponent<WorldMirror>();
    }

    void Update() {
        SpawnOnButton();
    }

    void SpawnOnButton() {
        if (Input.GetKeyDown(KeyCode.F1)) {
            SpawnItem(basicArrow, new Vector3(-58.45f, 1f, 69.44f), transform.rotation);
        }
    }

    void SpawnItem(GameObject obj, Vector3 position, Quaternion rotation) {

        GameObject newArrow = worldMirror.InstantiateAll(basicArrow, position, rotation);
        newArrow.transform.parent = this.transform;
    }
}
