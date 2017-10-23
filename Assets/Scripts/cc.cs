using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class cc : MonoBehaviour {
    string a_minigame;
    string a_answer;
    // Use this for initialization
    void Start () {
        var answer = Resources.Load<IfAnswerIsCorrect>("minigame1");
        a_minigame = answer.minigame;
        a_answer = answer.answer;
    }
	
	// Update is called once per frame
	void Update () {
       
	}
}
