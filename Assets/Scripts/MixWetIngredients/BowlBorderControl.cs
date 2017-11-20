using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlBorderControl : MonoBehaviour {

    private MixWetIngredientsMinigame minigame;

    private void Awake() {
        minigame = FindObjectOfType<MixWetIngredientsMinigame>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        EggDrag egg = other.GetComponent<EggDrag>();
        if (egg != null) {
            minigame.CrackEgg(egg);
        }
    }
}
