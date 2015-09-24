using UnityEngine;
using System.Collections;

public class TerrainBounds : MonoBehaviour {

    Bounds bounds = new Bounds();

    public bool DrawnBounds = true;
    public Bounds localBounds {
        get {
            return bounds;
        }
    }

    void Start() {
        bounds = GetChildRendererBounds();
    }

    void OnDrawGizmos() {
        if (DrawnBounds) {
            Bounds bounds = GetChildRendererBounds();
            Gizmos.DrawWireCube(bounds.center + transform.position, bounds.size);
        }
    }

    Bounds GetChildRendererBounds() {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        Bounds bounds = new Bounds(transform.position, Vector3.zero);
        foreach (var render in renderers) {
            bounds.Encapsulate(render.bounds);
        }
        bounds.center -= transform.position;
        return bounds;
    }
}
