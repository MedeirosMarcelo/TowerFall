using UnityEngine;
using System.Collections;

public class TerrainBounds : MonoBehaviour {

    public bool DrawnBounds = true;
    public Bounds localBounds {
        get {
            return GetLocalBounds();
        }
    }

    void OnDrawGizmos() {
        if (DrawnBounds) {
            Bounds bounds = GetLocalBounds();
            Gizmos.DrawWireCube(bounds.center + transform.position, bounds.size);
        }
    }

    Bounds GetLocalBounds() {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        Bounds bounds = new Bounds(transform.position, Vector3.zero);
        foreach (var render in renderers) {
            bounds.Encapsulate(render.bounds);
        }
        bounds.center -= transform.position;
        return bounds;
    }
}
