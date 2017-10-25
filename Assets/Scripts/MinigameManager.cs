using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour {

    public Minigame minigame;

    private void Start() {
		minigame.StartMinigame();
    }

}
