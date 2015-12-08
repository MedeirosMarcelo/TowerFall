using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {

    public GameObject[] loot;
    Transform itemAnimator;
    Animator animator;
    bool opened;

    public void Create() {
        animator = GetComponent<Animator>();
        itemAnimator = transform.Find("Item Animator");
    }

    public void Create(GameObject[] newloot) {
        animator = GetComponent<Animator>();
        itemAnimator = transform.Find("Item Animator");
        for (int i = 0; i < loot.Length; i++) {
            loot[i] = newloot[i];
        }
    }

    void Start() {
        Create();
    }

    void OnCollisionEnter(Collision col) {
        if (Network.isServer) {
            if (col.gameObject.tag == "Player") {
                Open();
            }
        }
    }

    public void Open() {
        if (!opened) {
            for (int i = 0; i < loot.Length; i++) {
                if (Network.isServer) {
                    GameObject item = (GameObject)Network.Instantiate(loot[i], transform.position, transform.rotation, 0);
                    item.transform.SetParent(itemAnimator);
                }
                itemAnimator.GetChild(i).transform.SetParent(itemAnimator);
            }
            animator.SetBool("Open", true);
            opened = true;
        }
    }

    public void DropLootAndDie() {
        for (int i = 0; i < loot.Length; i++) {
            itemAnimator.GetChild(i).transform.SetParent(null);
        }
        Destroy(this.gameObject, 1f);
    }
}