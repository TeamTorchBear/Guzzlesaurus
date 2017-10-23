using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class LoadScriptableObject {
    // Use this for initialization
    public static IfAnswerIsCorrect Load (string minigameName) {
        IfAnswerIsCorrect answer = Resources.Load<IfAnswerIsCorrect>(minigameName);
        return answer;
    }
}
