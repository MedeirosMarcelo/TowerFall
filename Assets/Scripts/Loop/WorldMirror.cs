﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldMirror : MonoBehaviour {
    //public GameObject Player;
    //public GameObject Reflection;
    StageManager stageManager;
    
    List<GameObject> WorldReflectionList = new List<GameObject>();

    public int ReflectionsX = 5;
    public int ReflectionsY = 0;
    public int ReflectionsZ = 0;

    public Bounds bounds { get; private set; }

    void Start() {
        BuildWorldReflections();
    }

    void AddReflection(Vector3 offset) {
        var obj = (GameObject)Instantiate(stageManager.reflectionPrefab,
                                          transform.position + offset,
                                          transform.rotation);
        obj.transform.parent = transform;
        WorldReflectionList.Add(obj);
    }
    void BuildReflections(Vector3 offset, int clones) {
        while (clones > 0) {
            AddReflection(offset * clones);
            AddReflection(offset * -clones);
            clones--;
        }
    }
    public void BuildWorldReflections() {
        stageManager = GameObject.FindGameObjectWithTag("Stage").GetComponent<StageManager>();
        bounds = stageManager.GetBounds();
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

    public GameObject InstantiateReflections(GameObject obj) {
        var loopController = obj.GetComponent<LoopController>();
        if (loopController != null) {
            var reflection = obj.GetComponent<LoopController>().reflectionPrefab;
            IList<GameObject> reflectionList = new List<GameObject>();
            foreach (GameObject world in WorldReflectionList) {
                var newReflection = (GameObject)Instantiate(reflection);
                newReflection.transform.parent = world.transform;
                newReflection.transform.localPosition = obj.transform.position;
                newReflection.transform.localRotation = obj.transform.rotation;
                newReflection.GetComponent<ObjectReflection>().original = obj;
                reflectionList.Add(newReflection);
            }
            obj.GetComponent<Reflectable>().SetReflections(reflectionList);
        }
        return obj;
    }

    public void DestroyReflections(GameObject obj) {
        IList<GameObject> reflectionList = obj.GetComponent<Reflectable>().GetReflections();
        foreach (GameObject reflection in reflectionList) {
            Destroy(reflection.gameObject);
        }
    }
}