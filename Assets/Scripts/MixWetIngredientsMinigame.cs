using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MixWetIngredientsMinigame : MonoBehaviour {

    [Header("Minigame parameters")]
    public float timeBeforePointingHand = 1f;

    [Header("External references")]
    public PointerControl pointer;
    public Transform eggsTarget;

    private enum STATE {
        INIT_CRACKEGG = 0,
        WAIT_CRACKEGG,
        INIT_POURMILK,
        WAIT_POURMILK,
        INIT_DROPMILK,
        WAIT_DROPMILK
    };

    private STATE state;

    private void Start() {
        state = STATE.INIT_CRACKEGG;
    }


    private void Update() {
        switch (state) {
            case STATE.INIT_CRACKEGG:
                // Show pointer after a given time
                StartCoroutine(CallAfter(timeBeforePointingHand, SetPointer));

                state++;
                break;
            case STATE.WAIT_CRACKEGG:

                break;
            case STATE.INIT_POURMILK:
                break;
            case STATE.WAIT_POURMILK:
                break;
            case STATE.INIT_DROPMILK:
                break;
            case STATE.WAIT_DROPMILK:
                break;
        }
    }

    public void StartDraggingEgg() {
        pointer.Hide();
    }

    public void EndDraggingEgg() {
        state = STATE.INIT_CRACKEGG;
    }

    private void SetPointer() {
        pointer.PointTo(eggsTarget.position);
        pointer.Show();
    }

    private IEnumerator CallAfter(float seconds, Action function) {
        float start = Time.time;
        while(Time.time - start < seconds) {
            yield return false;
        }
        function();
    }



}
