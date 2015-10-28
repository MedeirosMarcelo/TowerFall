using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class CharacterArrows  {

    Character character;

    public IList<GameObject> arrowList = new List<GameObject>();
    public string[] arrowListDetail = new string[10];

    public CharacterArrows(Character character) {
        this.character = character;
    }

    public void FixedUpdate() {
        Shoot();
    }

    /*
        int i = 0;
        foreach (GameObject arrow in arrowList){
            arrowListDetail[i] = arrow.name;
            i++;
        }
    }
    */

    void Shoot() {
        if (character.input.shoot) {
            BuildArrow();
        }
    }

    void BuildArrow() {
        if (arrowList.Count > 0) { 
            Vector3 arrowPosition = character.transform.Find("ArrowSpawner").position;
            Quaternion arrowRotation = character.transform.Find("ArrowSpawner").rotation;

            Vector3 rayPos =  new Vector3(Screen.width * 0.5f, Screen.height * 0.5f).ToPlayerCamera(character.playerNumber);
            Ray ray = character.charCamera.ScreenPointToRay(rayPos);
            
            GetNextArrow().SetActive(true);
            GameObject newArrow = character.worldMirror.InstantiateAll(GetNextArrow(), arrowPosition, arrowRotation);
            RemoveArrow(GetNextArrow()); //TEMPORÁRIO TAMBÉM!
            
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                newArrow.transform.LookAt(hit.point);
            }
            else {
                newArrow.transform.LookAt(ray.GetPoint(15));
            }
            newArrow.GetComponent<Arrow>().Shoot();
        }
    }

    public void StoreArrow(Arrow arrow) {
        GameObject newArrow = GetArrowByType(arrow.type);
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
