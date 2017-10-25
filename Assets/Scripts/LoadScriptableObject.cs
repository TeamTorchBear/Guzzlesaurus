using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LoadScriptableObject {
    //load the scriptobject by name
    public static Minigame Load(string minigameName) {
        var answer = Resources.Load<Minigame>(minigameName);
        return answer;
    }
}
