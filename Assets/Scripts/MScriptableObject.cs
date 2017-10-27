using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MScriptableObject {
    //load the scriptobject by name
    public static Minigame GetMinigame(string minigameName) {
        var answer = Resources.Load<Minigame>(minigameName);
        return answer;
    }

    public static void SetTimer(string minigameName, string setTimer)
    {
        var answer = Resources.Load<Minigame>(minigameName);
        answer.timer = setTimer;
        return;
    }
}
