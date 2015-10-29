﻿using UnityEngine;
using System.Collections;

public class Scene : MonoBehaviour {

    GameManager gameManager;

	void Start () {
        gameManager = GetComponent<GameManager>();
        Init();
	}

    void Update() {
        SpawnArrowOnButton();
    }

    void Init() {
        gameManager.SpawnPlayer(gameManager.characterPrefab, new Vector3(-60f, 1.46f, 68f), transform.rotation);
        gameManager.SpawnPlayer(gameManager.characterPrefab, new Vector3(-30f, 1.46f, 68f), transform.rotation);
    }

    void SpawnArrowOnButton() {
        if (Input.GetKeyDown(KeyCode.F1)) {
            gameManager.SpawnItem(gameManager.basicArrowPrefab, new Vector3(-58.45f, 1f, 69.44f), transform.rotation);
        }
    }
}
