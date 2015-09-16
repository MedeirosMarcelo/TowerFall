using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldMirror : MonoBehaviour {

    public GameObject[] worldMirrorList = new GameObject[4];
    public GameObject arrowMirror;

    void Start() {
    }

    void Update() {

    }

    public GameObject InstantiateAll(GameObject obj, Vector3 position, Quaternion rotation) {
        GameObject newObj = (GameObject)Instantiate(obj, position, rotation);
        newObj.transform.parent = this.transform;

        GameObject mirrorObj = GetMirrorObject(obj);
        for (int i = 0; i < worldMirrorList.Length; i++) {
            if (worldMirrorList[i] != null) {
                GameObject newMirrorObj = (GameObject)Instantiate(mirrorObj, position, rotation);
                newMirrorObj.transform.parent = worldMirrorList[i].transform;
                newMirrorObj.transform.localPosition = newObj.transform.localPosition;
                newMirrorObj.transform.localRotation = newObj.transform.localRotation;
                newMirrorObj.GetComponent<ObjectMirror>().target = newObj;
            }
        }
        return newObj;
    }

    GameObject GetMirrorObject(GameObject obj) {
        switch (obj.name) {
            default:
            case "Arrow":
                return arrowMirror;
        }
    }
}
