using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class LoadScriptableObject {
    //load the scriptobject by name
    public static IfAnswerIsCorrect Load(string minigameName) {
        var answer = Resources.Load<IfAnswerIsCorrect>(minigameName);
        return answer;
    }
}
