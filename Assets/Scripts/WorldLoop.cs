using UnityEngine;
using System.Collections;

public class WorldLoop : MonoBehaviour {
    public Vector3 Offset = new Vector3(0.0f, 4.5f, 0.0f);
    public Vector3 Size = new Vector3( 35.0f, 10.0f, 30.0f) ;
    public Bounds bounds {
        get {
            return new Bounds(Offset, Size);
        }
    }
}
