﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class CharacterArrows  {

    Character character;
    CharacterInput input;

    public IList<GameObject> arrowList = new List<GameObject>();
    public string[] arrowListDetail = new string[10];

    public CharacterArrows(Character character, CharacterInput input) {
        this.character = character;
        this.input = input;
    }

    public void FixedUpdate() {
        Shoot();
    }

    void Shoot() {
        if (input.shoot) {
            Debug.Log("SHOOT");
            BuildArrow();
        }
    }

    void BuildArrow() {
        Debug.Log("Arrow Count " + arrowList.Count);
        if (arrowList.Count > 0) { 
            Vector3 arrowPosition = character.transform.Find("ArrowSpawner").position;
            Quaternion arrowRotation = character.transform.Find("ArrowSpawner").rotation;

            Vector3 rayPos =  new Vector3(Screen.width * 0.5f, Screen.height * 0.5f).ToPlayerCamera(character.playerNumber);
            Ray ray = character.charCamera.ScreenPointToRay(rayPos);
            
            GetNextArrow().SetActive(true);
            
            //Use after reimplementing reflections.
            //GameObject newArrow = character.worldMirror.InstantiateAll(GetNextArrow(), arrowPosition, arrowRotation);

            //Temporary until reflections are up again
            GameObject newArrow = character.transform.parent.GetComponent<ClientManager>().InstantiateArrow(GetNextArrow(), arrowPosition, arrowRotation);
            RemoveArrow(GetNextArrow()); //TEMPORÁRIO TAMBÉM!
            
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                newArrow.transform.LookAt(hit.point);
            }
            else {
                newArrow.transform.LookAt(ray.GetPoint(15));
            }
            newArrow.GetComponent<Arrow>().Shoot(character);
        }
    }

    public void StoreArrow(ArrowType type) {
        GameObject newArrow = GetArrowByType(type);
        arrowList.Add(newArrow);
    }

    public void RemoveArrow(GameObject arrow) {
        arrowList.Remove(arrow);
    }

    public GameObject GetNextArrow() {
        return arrowList[arrowList.Count - 1];
    }

    GameObject GetArrowByType(ArrowType type) {
        switch (type) {
            default:
            case ArrowType.Basic:
                return character.basicArrow;
        }
    }
}
