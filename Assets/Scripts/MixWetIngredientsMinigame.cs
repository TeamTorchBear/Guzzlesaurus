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
        CRACKEGG = 0,
        POURMILK,
        DROPMILK
    };

    private STATE state;

    private void Start() {
        state = 0;
    }


    private void Update() {
        switch (state) {
            case STATE.CRACKEGG:
                // Show pointer after a given time
                CallAfter(timeBeforePointingHand, SetPointer);



                break;
            case STATE.POURMILK:
                break;
            case STATE.DROPMILK:
                break;
        }
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
