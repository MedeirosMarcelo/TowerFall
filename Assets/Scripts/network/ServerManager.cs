using UnityEngine;
using System.Collections;

public class ServerManager : MonoBehaviour {

    public static ServerManager Get() {
        var obj = GameObject.FindWithTag("GameController");
        if (obj == null) {
            Debug.LogError("Server Manager Not Found");
            return null;
        }
        return obj.GetComponent<ServerManager>();
    }

    public GameObject lobbyCharacterPrefab;
    public GameObject characterPrefab;

    public GameObject arrowPrefab;
    public GameObject arrowPickupPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
