using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour {

    int p1Score = 0;
    int p2Score = 0;

    Text p1;
    Text p2;

    void Start() {
        p1 = transform.Find("P1").GetComponent<Text>();
        p2 = transform.Find("P2").GetComponent<Text>();
    }

    public void Scored(int playerNumber) {
        if (playerNumber == 1) {
            p1Score++;
            p1.text = p1Score.ToString();
        }
        else {
            p2Score++;
            p2.text = p2Score.ToString();
        }
    }
}
