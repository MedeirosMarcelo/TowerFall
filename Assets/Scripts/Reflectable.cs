using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Reflectable : MonoBehaviour {

    public bool UseReflections = true;
    IList<GameObject> reflectionList = new List<GameObject>();

    public void SetReflections(IList<GameObject> reflections) {
        reflectionList = reflections;
    }

    public IList<GameObject> GetReflections() {
        return reflectionList;
    }
}
