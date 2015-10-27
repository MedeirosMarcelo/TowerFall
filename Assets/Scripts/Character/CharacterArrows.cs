using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class CharacterArrows  {

    Character character;

    public IList<GameObject> arrows = new List<GameObject>();
    public string[] arrowListDetail = new string[10];


    public CharacterArrows(Character character) {
        this.character = character;
    }

    public void FixedUpdate() {
        Shoot();
    }

    /*
        int i = 0;
        foreach (GameObject arrow in arrows){
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
        if (arrows.Count > 0) { 
            Vector3 arrowPosition = character.transform.Find("ArrowSpawner").position;
            Quaternion arrowRotation = character.transform.Find("ArrowSpawner").rotation;
            Ray ray = character.charCamera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));
            RaycastHit hit;
            GetArrow().SetActive(true);
            GameObject newArrow = character.worldMirror.InstantiateAll(GetArrow(), arrowPosition, arrowRotation);
            RemoveArrow(GetArrow()); //TEMPORÁRIO TAMBÉM!
            if (Physics.Raycast(ray, out hit)) {
                newArrow.transform.LookAt(hit.point);
            }
            else {
                newArrow.transform.LookAt(ray.GetPoint(15));
            }
            newArrow.GetComponent<Arrow>().shot = true;
        }
    }

    public void StoreArrow(Arrow arrow) {
        GameObject newArrow = GetArrowByType(arrow.type);
        arrows.Add(newArrow);
    }

    public void RemoveArrow(GameObject arrow) {
        arrows.Remove(arrow);
    }

    public GameObject GetArrow() {
        return arrows[0];
    }

    GameObject GetArrowByType(ArrowType type) {
        switch (type) {
            default:
            case ArrowType.basic:
                return character.basicArrow;
        }
    }

}
