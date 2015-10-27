using UnityEngine;
using System;

[Serializable]
public class Timer {
    float elapsed = 0;
    float timeout;
    public Timer(float timeout) {
        this.timeout = timeout;
    }
    public void Update(float delta) {
        if (!ended) {
            elapsed += delta;
            ended = (timeout > elapsed);
        }
    }
    public bool ended { get; private set; }
}