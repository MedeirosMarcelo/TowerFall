using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    private static GameManager gameManager = null;
 
	void Start () {
        if(gameManager != null){
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
	}
	
    void OnDisconnectedFromServer() {
        Debug.Log("OnDisconnected");
    }


}
