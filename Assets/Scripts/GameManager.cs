using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public IList<GameObject> playerList = new List<GameObject>();
    public GameObject basicArrowPrefab;
    public GameObject characterPrefab;
    int maxArrows = 7;
    WorldMirror worldMirror;
    Score score;
    float spawnTime = 3f;

    void Start() {
        worldMirror = GetComponent<WorldMirror>();
        score = GameObject.FindWithTag("Canvas").transform.Find("Score").GetComponent<Score>();
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


        GameObject newArrow = worldMirror.InstantiateAll(basicArrowPrefab, Vector3.zero, transform.rotation);
        newPlayer.GetComponent<Character>().PickUpItem(newArrow.GetComponent<Item>());
        newArrow = worldMirror.InstantiateAll(basicArrowPrefab, Vector3.zero, transform.rotation);
        newPlayer.GetComponent<Character>().PickUpItem(newArrow.GetComponent<Item>());
        newArrow = worldMirror.InstantiateAll(basicArrowPrefab, Vector3.zero, transform.rotation);
        newPlayer.GetComponent<Character>().PickUpItem(newArrow.GetComponent<Item>());
    }

    public void RespawnPlayer(GameObject obj, int playerNumber, Vector3 position, Quaternion rotation) {

        GameObject newPlayer = worldMirror.InstantiateAll(characterPrefab, position, rotation);
        playerList.Add(newPlayer);

        newPlayer.GetComponent<Character>().Create(playerNumber);
        newPlayer.transform.parent = this.transform;
    }

    //TODO: Put this in extensions.
    public static Vector3 ConvertToPlayerCamera(int playerNumber, Vector3 position) {
        if (playerNumber == 1) {
            Debug.Log("GetCameraPosition " + playerNumber);
            return new Vector3(position.x, position.y + (Screen.height * 0.25f));
        }
        else if (playerNumber == 2) {
            Debug.Log("GetCameraPosition " + playerNumber);
            return new Vector3(position.x, position.y - (Screen.height * 0.25f));
        }
        else {
            Debug.LogError("GetCameraPosition - Wrong player number");
            return Vector3.zero;
        }
    }

    public void Scored(int playerNumber) {
        score.Scored(playerNumber);
    }

    public void Respawn(int playerNumber) {
        Debug.Log("!!!!");
        StartCoroutine("SetRespawn", playerNumber);
    }

    IEnumerator SetRespawn(int playerNumber) {
        yield return new WaitForSeconds(spawnTime);
        RespawnPlayer(characterPrefab, playerNumber, new Vector3(-60f, 1.46f, 68f), transform.rotation);
    }
}
