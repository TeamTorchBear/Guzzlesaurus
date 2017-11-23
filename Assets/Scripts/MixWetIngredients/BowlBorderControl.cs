using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlBorderControl : MonoBehaviour {

    public float crackCooldown = 1f;

    private MixWetIngredientsMinigame minigame;

    private void Awake() {
        minigame = FindObjectOfType<MixWetIngredientsMinigame>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        EggDrag egg = other.GetComponent<EggDrag>();
        // What collides it's an egg and also comes from the upper-left
        if (egg != null && (other.transform.position.x < transform.position.x && other.transform.position.y > transform.position.y)) {
            minigame.CrackEgg(egg);
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(DoAfter(crackCooldown, () => {
                GetComponent<Collider2D>().enabled = true;
            }));
        }
    }

    IEnumerator DoAfter(float time, Action function) {
        float start = Time.time;
        while (Time.time - start < time) {
            yield return false;
        }
        function();
    }
}
