using UnityEngine;
using System.Collections;

public class ArrowPickup : Item {

    public ArrowType type = ArrowType.Basic;
    NetworkView netView;
    private bool isNotMine { get { return !netView.isMine; } }

    void Start() {
        Debug.Log("Start: " + name + " id:" + GetInstanceID());
        netView = GetComponent<NetworkView>();
        if (isNotMine) {
            collider.enabled = false;
        }
    }

    void OnTriggerEnter(Collider col) {
        switch (col.tag) {
            default:
                break;
            case "Player":
            Debug.Log("Pickup");
                var character = col.gameObject.GetComponent<Character>();
                character.networkView.RPC("StoreArrow", RPCMode.Others, (int)type);
                Destroy();
                break;
        }
    }

    public void Destroy() {
        Debug.Log("Destroy: " + name + " id:" + GetInstanceID());
        Network.RemoveRPCs(netView.viewID);
        Network.Destroy(netView.viewID);
    }
}
