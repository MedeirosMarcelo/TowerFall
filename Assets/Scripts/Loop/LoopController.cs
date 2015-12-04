using UnityEngine;
using System.Collections;

public class LoopController : MonoBehaviour {

    public GameObject reflectionPrefab;

    WorldMirror worldMirror;
    Bounds bounds;

    void Start() {
        worldMirror = GameObject.FindGameObjectWithTag("World Main").GetComponent<WorldMirror>();
        worldMirror.InstantiateReflections(gameObject);
        bounds = worldMirror.bounds;
    }

    void OnDestroy() {
        worldMirror.DestroyReflections(gameObject);
    }

    void LoopAxis(float min, float max, ref float position) {
        float size = max - min;
        if (position > max) {
            position -= size;
        }
        else if (position < min) {
            position += size;
        }
    }

    void Loop() {
        Vector3 position = transform.position;

        LoopAxis(bounds.min.x, bounds.max.x, ref position.x);
        LoopAxis(bounds.min.y, bounds.max.y, ref position.y);
        LoopAxis(bounds.min.z, bounds.max.z, ref position.z);

        if (position != transform.position) {
            transform.position = position;
            Debug.Log(name + "\n"
                + "Bound Min= " + bounds.min.ToString("N2") + " Max=" + bounds.max.ToString("N2") + "\n"
                + "Position=" + position.ToString("N2"));
        }
    }

    void FixedUpdate() {
        if (networkView.isMine) {
            Loop();
        }
    }
}
