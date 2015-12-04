using UnityEngine;
using System.Collections;

public enum NetworkGroup {
    None = 0,
    Character = 1,
    Arrow = 2
}

public static class Extensions {

    public static T PickRandom<T>(this T[] source) {
        return source[Random.Range(0, source.Length)];
    }

    public static Vector3 Invert(this Vector3 v) {
        float[] array = new float[3];
        array[0] = v.x;
        array[1] = v.y;
        array[2] = v.z;

        for (int i = 0; i < 3; i++) {
            if (array[i] != 0) array[i] = 0;
            else array[i] = 1;
        }
        Vector3 result = new Vector3(array[0], array[1], array[2]);
        return result;
    }

    public static Vector3 ToPlayerCamera(this Vector3 position, int playerNumber) {
        if (playerNumber == 1) {
            return new Vector3(position.x, position.y + (Screen.height * 0.25f));
        }
        else if (playerNumber == 2) {
            return new Vector3(position.x, position.y - (Screen.height * 0.25f));
        }
        else {
            Debug.LogError("GetCameraPosition - Wrong player number");
            return Vector3.zero;
        }
    }
}