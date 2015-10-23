using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerArrows : MonoBehaviour {

    WorldMirror worldMirror { get; set; }
    PlayerInput input { get; set; }
    Camera playerCamera  { get; set; }

    public IList<GameObject> arrows = new List<GameObject>();
    public string[] arrowListDetail = new string[10];
    public GameObject basicArrow;

	// Use this for initialization
	void Start () {
        worldMirror = transform.parent.GetComponent<WorldMirror>();
        input = GetComponent<PlayerInput>();
        playerCamera = GetComponentInChildren<Camera>();
	}

    public void Shoot() {
        if (input.shoot) {
            BuildArrow();
        }
    }

    void BuildArrow() {
        Character character = GetComponent<Character>(); //GETCOMPONENT TEMPORÁRIO!
        if (arrows.Count > 0) { 
            Vector3 arrowPosition = transform.Find("ArrowSpawner").position;
            Quaternion arrowRotation = transform.Find("ArrowSpawner").rotation;
            Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));
            RaycastHit hit;
            GetArrow().SetActive(true);
            GameObject newArrow = worldMirror.InstantiateAll(GetArrow(), arrowPosition, arrowRotation);
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


    void Update() {
        int i = 0;
        foreach (GameObject arrow in arrows){
            arrowListDetail[i] = arrow.name;
            i++;
        }
    }

    public void Store(Arrow arrow) {
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
                return basicArrow;
        }
    }

}
