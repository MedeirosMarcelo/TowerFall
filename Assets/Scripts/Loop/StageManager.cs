using UnityEngine;
using System.Collections;

public class StageManager : MonoBehaviour {
    public GameObject reflectionPrefab;

    public bool DrawnBounds = true;
    public Bounds bounds { get; private set; }
    public GameObject[] characterSpawnList { get; private set; }
    public GameObject[] chestSpawnList { get; private set; }

    ClientManager clientManager;
    ServerManager serverManager;

    void Start() {
        if (Network.isClient) {
            clientManager = ClientManager.Get();
            clientManager.stage = this;

        }
        else {
            serverManager = ServerManager.Get();
            serverManager.stage = this;
        }
        bounds = GetBounds();
        characterSpawnList = GameObject.FindGameObjectsWithTag("Spawn");
        chestSpawnList = GameObject.FindGameObjectsWithTag("ChestSpawn");

    }

    void OnDrawGizmos() {
        if (DrawnBounds) {
            Bounds bounds = GetBounds();
            Gizmos.DrawWireCube(bounds.center + transform.position, bounds.size);
        }
    }

    public Bounds GetBounds() {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        Bounds bounds = new Bounds(transform.position, Vector3.zero);
        foreach (var render in renderers) {
            bounds.Encapsulate(render.bounds);
        }
        bounds.center -= transform.position;
        return bounds;
    }
}
