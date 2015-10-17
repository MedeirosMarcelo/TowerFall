using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Reflectable : MonoBehaviour {

    public bool UseReflections = true;
    IList<Item> reflectionList = new List<Item>();

    public void SetReflections(IList<Item> reflections) {
        reflectionList = reflections;
    }
}
