using UnityEngine;
using System.Collections;

public static class Extensions {
    static Vector3 Invert(this Vector3 v) {
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
}