using UnityEngine;
using System.Collections;

public class StageManager : MonoBehaviour {
    public GameObject reflectionPrefab;

    public bool DrawnBounds = true;
    public Bounds bounds { get; private set; }

    ClientManager clienManager;

    void Start () {
        clienManager = ClientManager.Get();
        clienManager.stage = gameObject;
        clienManager.spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
        bounds = GetBounds();
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
