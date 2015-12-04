using UnityEngine;
using System.Collections;

public class BombArrow : Arrow {

    public float explosionDelay;
    public float destroyDelay;
    GameObject damageArea;

    void Start() {
        damageArea = transform.Find("DamageArea").gameObject;
        type = ArrowType.Bomb;
    }

    public void Explode() {
        StartCoroutine("WaitAndExplode");
    }

    IEnumerator WaitAndExplode() {
        yield return new WaitForSeconds(explosionDelay);
        damageArea.SetActive(true);
        StartCoroutine("WaitAndDestroy");
    }

    IEnumerator WaitAndDestroy() {
        yield return new WaitForSeconds(destroyDelay);
        Destroy();
    }
}
