using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldMirror : MonoBehaviour {
    public GameObject Player;
    public GameObject Reflection;
    public GameObject Terrain;
    public int ReflectionsX = 5;
    public int ReflectionsY = 0;
    public int ReflectionsZ = 0;
    public Bounds bounds {
        get {
            return (Terrain == null) ? new Bounds(transform.position, transform.localScale) : Terrain.GetComponent<TerrainBounds>().localBounds;
        }
    }

    public List<GameObject> WorldReflectionList = new List<GameObject>();

    void AddReflection(Vector3 offset) {
        Debug.Log("");
        var obj = (GameObject)Instantiate(Reflection,
                                          transform.position + offset,
                                          transform.rotation);
        obj.transform.parent = transform;
        obj.GetComponent<WorldReflection>().PlayerReflection.GetComponent<ObjectReflection>().original = Player;
        WorldReflectionList.Add(obj);
    }

    void BuildReflections(Vector3 offset, int clones) {
        Debug.Log("");
        while (clones > 0) {
            AddReflection(offset * clones);
            AddReflection(offset * -clones);
            clones--;
        }
    }

    public void BuildWorldReflections() {
        Debug.Log("");
        ClearWorldReflections();
        BuildReflections(new Vector3(bounds.size.x, 0.0f, 0.0f), ReflectionsX);
        BuildReflections(new Vector3(0.0f, bounds.size.y, 0.0f), ReflectionsY);
        BuildReflections(new Vector3(0.0f, 0.0f, bounds.size.z), ReflectionsZ);
    }
    public void ClearWorldReflections() {
        foreach (GameObject world in WorldReflectionList) {
            DestroyImmediate(world);
        }
        WorldReflectionList.Clear();
   }

    void Start() {
        BuildWorldReflections();
    }

    public GameObject InstantiateAll(GameObject obj, Vector3 position, Quaternion rotation) {
        var newObject = (GameObject)Instantiate(obj, position, rotation);
        newObject.transform.parent = transform;


        var loopController = newObject.GetComponent<LoopController>();
        if (loopController != null) {
            var reflection = newObject.GetComponent<LoopController>().Reflection;
            foreach (GameObject world in WorldReflectionList) {
                var newReflection = (GameObject)Instantiate(reflection);
                newReflection.transform.parent = world.transform;
                newReflection.transform.localPosition = world.transform.localPosition;
                newReflection.transform.localRotation = world.transform.localRotation;
                newReflection.GetComponent<ObjectReflection>().original = newObject;
            }
        }
        return newObject;
    }
}
