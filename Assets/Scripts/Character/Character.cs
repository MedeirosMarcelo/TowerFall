using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CharacterColor {
    White,
    Black,
    Yellow,
    Blue,
    Green,
    Red,
    Purple,
    Pink
}

public class Character : Reflectable {

    public static readonly int group = (int)NetworkGroup.Character;
    // Internals
    public CharacterController controller { get; private set; }
    public CharacterInput input { get; private set; }
    public CharacterArrows arrows { get; private set; }
    public CharacterFsm fsm { get; private set; }
    // World
    public StageManager stage { get { return clientManager.stage; } }
    // Network
    public bool isMine { get { return networkView.isMine; } }
    public bool isNotMine { get { return !networkView.isMine; } }

    [Header("Colored Materials")]
    public List<Material> materials = new List<Material>();

    [Header("Children Objects")]
    public Camera charCamera;
    public GroundCollider groundCollider;
    public HandsCollider handsCollider;
    public GameObject head;
    public GameObject feet;
    public GameObject arrowSpawner;
    public Animation modelAnimation;
    public SkinnedMeshRenderer skinnedMesh;
    public GameObject shield;

    [Header("Arrow Prefabs")]
    public GameObject arrowPrefab;
    public GameObject bombArrowPrefab;

    [Header("Character config")]
    public int health = 1;

    private CharacterColor _color;
    public CharacterColor color {
        get { return _color; }
        set { SetMaterial((int)value); }
    }

    void Update() {
        if (isNotMine) {
            return;
        }
        input.Update();
    }

    void FixedUpdate() {
        if (Network.isServer && !destroyRequested) {
            DetectJumpKill();
            return;
        }
        if (isNotMine) {
            return;
        }
        controller.Look();
        fsm.FixedUpdate(Time.deltaTime);
        arrows.FixedUpdate();

        // Input must be last here
        input.FixedUpdate();
    }

    ClientManager clientManager;
    ServerManager serverManager;

    void Start() {
        if (Network.isClient) {
            clientManager = ClientManager.Get();
            clientManager.characterList.Add(this);
        }
        else {
            serverManager = ServerManager.Get();
            serverManager.characterList.Add(this);
        }

        if (Network.isServer || (Network.isClient && isNotMine)) {
            charCamera.gameObject.SetActive(false);
        }

        input = new CharacterInput(this);
        controller = new CharacterController(this);
        arrows = new CharacterArrows(this);
        fsm = new CharacterFsm(this);

    }

    void OnDestroy() {
        if (clientManager) {
            Debug.Log("OnDestroy client");
            clientManager.characterList.Remove(this);
        }
        else {
            Debug.Log("OnDestroy server");
            serverManager.characterList.Remove(this);
        }
    }


    public void TakeHit(DamageDealer damager) {
        Debug.Log("Take hit");

        if (Network.isClient) {
            return;
        }
        int dmg = damager.GetComponent<DamageDealer>().damage;
        TakeDamage(dmg);
    }

    void MonitorHealth() {
        if (health <= 0) {
            RequestOwnerDestruction();
        }
    }

    private int headMask;
    private float headMaxDistance = 0.2f;
    private float headRadius = 0.25f;
    private float headKillForce = 25f;

    void DetectJumpKill() {
        headMask = LayerMask.GetMask("Feet");
        Vector3 position = head.transform.position;
        Vector3 direction = Vector3.up;
        RaycastHit hit;
        Ray ray = new Ray(position, direction);

        Debug.DrawRay(position, direction * headMaxDistance, Color.yellow);
        Debug.DrawRay(position + Vector3.forward * headRadius, direction * headMaxDistance, Color.yellow);
        Debug.DrawRay(position + Vector3.right * headRadius, direction * headMaxDistance, Color.yellow);
        Debug.DrawRay(position + Vector3.back * headRadius, direction * headMaxDistance, Color.yellow);
        Debug.DrawRay(position + Vector3.left * headRadius, direction * headMaxDistance, Color.yellow);

        if (Physics.SphereCast(ray, headRadius, out hit, headMaxDistance, headMask)) {
            Debug.Log("Head Hit" + hit.collider.name);
            var parent = hit.collider.transform.parent;
            if (parent.tag == "Player") {
                Debug.Log("Jump kill!");
                RequestOwnerDestruction();
                parent.rigidbody.AddForce(Vector3.up * headKillForce, ForceMode.Impulse);

            }
        }
    }

    void OnDisconnectedFromServer() {
        Destroy(gameObject);
    }

    public void ServerDestroy() {
        Debug.Log("Server Destroy: " + GetInstanceID());
        destroyRequested = true;
        Network.RemoveRPCs(networkView.owner, group);
        Network.Destroy(gameObject);
        return;
    }
    bool destroyRequested = false;
    void RequestOwnerDestruction() {
        Debug.Log("Requested Owner Destroy: " + GetInstanceID());
        destroyRequested = true;
        Network.RemoveRPCs(networkView.owner, group);
        networkView.RPC("OwnerDestroy", RPCMode.Others);
        return;
    }
    [RPC]
    void OwnerDestroy() {
        if (networkView.isMine) {
            Debug.Log("Destroy Character " + GetInstanceID());
            Network.Destroy(gameObject);
            return;
        }
    }

    [RPC]
    void TakeDamage(int damage) {
        Debug.Log(health + " " + damage);
        health -= damage;
        MonitorHealth();

        if (Network.isServer && health > 0) {
            networkView.RPC("TakeDamage", RPCMode.Others, damage);
        }
    }
    [RPC]
    void StoreArrow(int type) {
        if (isMine && (arrows.stack.Count < arrows.maxArrows)) {
            Debug.Log("Got Arrow type = " + (ArrowType)type);
            arrows.stack.Push((ArrowType)type);
        }
    }

    [RPC]
    void ApplyShield() {
        if (isMine && (health == 1)) {
            Debug.Log("Got SHIELD");
            health = 2;
            shield.SetActive(true);
        }
    }

    [RPC]
    void RemoveShield() {
        if (isMine) {
            Debug.Log("REMOVE SHIELD");
            health = 1;
            shield.SetActive(false);
        }
    }
    [RPC]
    void SetMaterial(int color) {
        _color = (CharacterColor)color;
        Material[] aux = skinnedMesh.materials;
        aux[1] = materials[color];
        skinnedMesh.materials = aux;
        if (networkView.isMine) {
            networkView.RPC("SetMaterial", RPCMode.Others, (int)_color);
        }
    }
}
