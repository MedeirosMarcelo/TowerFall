using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class CharacterArrows  {

    Character character;
    CharacterInput input;
    GameObject arrowSpawner;

    public IList<GameObject> arrowList = new List<GameObject>();
    public string[] arrowListDetail = new string[10];

    public CharacterArrows(Character character, CharacterInput input, GameObject arrowSpawner) {
        this.character = character;
        this.input = input;
        this.arrowSpawner = arrowSpawner;
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
        //if (arrowList.Count > 0) { 
        if (true) { 
           
            //GetNextArrow().SetActive(true);

            Vector3 arrowPosition = arrowSpawner.transform.position;
            Quaternion arrowRotation = arrowSpawner.transform.rotation;
            GameObject newArrow = (GameObject)Network.Instantiate(character.basicArrow, arrowPosition, arrowRotation, 0);

            RaycastHit hit;
            Ray ray = character.charCamera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));
            if (Physics.Raycast(ray, out hit)) {
                newArrow.transform.LookAt(hit.point);
            }
            else {
                newArrow.transform.LookAt(ray.GetPoint(15));
            }
            newArrow.GetComponent<Arrow>().Shoot(character);

            //RemoveArrow(GetNextArrow()); //TEMPORÁRIO TAMBÉM!
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
