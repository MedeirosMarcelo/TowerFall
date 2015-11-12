using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(WorldMirror))]
public class WorldMirrorEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        var worldMirror = (WorldMirror)target;
        if (GUILayout.Button("Build World Reflections")) {
            worldMirror.BuildWorldReflections();
        }
        if (GUILayout.Button("Clear World Reflections")) {
            worldMirror.ClearWorldReflections();
        }
    }
}
#endif