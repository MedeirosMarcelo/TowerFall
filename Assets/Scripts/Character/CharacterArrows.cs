using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class CharacterArrows {

    Character character;

    Stack<ArrowType> arrowStack = new Stack<ArrowType>();
    public Stack<ArrowType> stack { get { return arrowStack; } }
    public readonly int maxArrows = 7;

    public CharacterArrows(Character character) {
        this.character = character;

        // 6 arrows start
        stack.Push(ArrowType.Bomb);
        stack.Push(ArrowType.Bomb);
        stack.Push(ArrowType.Bomb);
        stack.Push(ArrowType.Bomb);
        stack.Push(ArrowType.Bomb);
        stack.Push(ArrowType.Bomb);
    }

    public void FixedUpdate() {
        if (character.input.shoot && (arrowStack.Count > 0)) {
            Debug.Log("Shoot");
            BuildArrow();
        }
    }

    void BuildArrow() {
        Vector3 arrowPosition = character.arrowSpawner.transform.position;
        Quaternion arrowRotation = character.arrowSpawner.transform.rotation;
        var obj = Network.Instantiate(GetArrowByType(arrowStack.Peek()), arrowPosition, arrowRotation, Arrow.group) as GameObject;
        AimArrow(obj.transform);
        var arrow = obj.GetComponent<Arrow>();
        arrow.type = arrowStack.Pop();
    }

    void AimArrow(Transform arrowTransform)
    {
        var ray = character.charCamera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));
        arrowTransform.LookAt(ray.GetPoint(15)); 
    }

    GameObject GetArrowByType(ArrowType type) {
        switch (type) {
            default:
            case ArrowType.Basic:
                return character.basicArrow;
            case ArrowType.Bomb:
                return character.bombArrow;
        }
    }
}
