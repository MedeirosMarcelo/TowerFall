using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldMirror : MonoBehaviour {
    public GameObject Player;
    public GameObject Reflection;
    public bool MirrorOnObjects = true;
    public int ClonesX = 5;
    public int ClonesY = 0;
    public int ClonesZ = 0;
    public Vector3 Offset = new Vector3(0.0f, 4.5f, 0.0f);
    public Vector3 Size = new Vector3(35.0f, 10.0f, 30.0f);
    public Bounds bounds {
        get {
            return new Bounds(Offset, Size);
        }
    }

    List<GameObject> WorldReflectionList = new List<GameObject>();

    void AddReflection(Vector3 offset) {
        var obj = (GameObject)Instantiate(Reflection,
                                          transform.position + offset,
                                          transform.rotation);
        obj.transform.parent = transform;

        /*
        var playerReflection = obj.GetComponent<WorldReflection>().PlayerReflection;
        playerReflection.GetComponent<ObjectMirror>().target = Player;
        */


        var character = transform.FindChild("Third Person Test").gameObject;
        obj.transform.FindChild("Character Mirror").GetComponent<ObjectMirror>().target = character;


        WorldReflectionList.Add(obj);
    }

    void BuildReflections(Vector3 offset, int clones) {
        while (clones > 0) {
            AddReflection(offset * clones);
            AddReflection(offset * -clones);
            clones--;
        }
    }


    void EvaluateObjectsBounds() {

        Transform terrain = gameObject.transform.FindChild("Terrain");
        Transform floor = terrain.FindChild("Floor");
        Transform wall = terrain.FindChild("Wall");

        Vector3 floorSize = floor.renderer.bounds.size;
        Vector3 wallSize = wall.renderer.bounds.size;

        float height = floorSize.y + wallSize.y;

        Size = new Vector3(floorSize.x, height, floorSize.z);
        Offset = new Vector3(0.0f, height / 2.0f, 0.0f);
        Debug.Log(name + Size + Offset);
    }

    void Start() {
        if (MirrorOnObjects) {
            EvaluateObjectsBounds();
        }
        Debug.Log(name + Size + Offset);
        BuildReflections(new Vector3(Size.x, 0.0f, 0.0f), ClonesX);
        BuildReflections(new Vector3(0.0f, Size.y, 0.0f), ClonesY);
        BuildReflections(new Vector3(0.0f, 0.0f, Size.z), ClonesZ);
    }

    public GameObject ArrowReflection;

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
                newReflection.GetComponent<ObjectMirror>().target = newObject;
                Debug.Log(world.name);
            }
        }
        return newObject;
    }

    GameObject GetMirrorObject(GameObject obj) {
        switch (obj.name) {
            default:
            case "Arrow":
                return ArrowReflection;
        }
    }
}
