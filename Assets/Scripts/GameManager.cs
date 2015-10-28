using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public IList<GameObject> playerList = new List<GameObject>();
    public GameObject basicArrowPrefab;
    public GameObject characterPrefab;
    int maxArrows = 7;
    WorldMirror worldMirror;

    void Start() {
        worldMirror = GetComponent<WorldMirror>();
    }

    public void SpawnItem(GameObject obj, Vector3 position, Quaternion rotation) {

        GameObject newArrow = worldMirror.InstantiateAll(basicArrowPrefab, position, rotation);
        newArrow.transform.parent = this.transform;
    }

    public void SpawnPlayer(GameObject obj, Vector3 position, Quaternion rotation) {

        GameObject newPlayer = worldMirror.InstantiateAll(characterPrefab, position, rotation);
        playerList.Add(newPlayer);

        int playerNumber = playerList.IndexOf(newPlayer) + 1;
        newPlayer.GetComponent<Character>().Create(playerNumber);

        newPlayer.transform.parent = this.transform;
    }
}
