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
    public Transform bowlBorderTarget;


    private enum STATE {
        INIT_CRACKEGG = 0,
        WAIT_CRACKEGG,
        INIT_POURMILK,
        WAIT_POURMILK,
        INIT_DROPMILK,
        WAIT_DROPMILK
    };

    private STATE state;
    private Vector2 eggPosition;
    private bool dragging = false;

    private void Start() {
        state = STATE.INIT_CRACKEGG;
        eggPosition = eggsTarget.position;
        SetPointer(eggPosition);
    }

    public void StartDraggingEgg() {
        dragging = true;


        SetPointer(bowlBorderTarget.position);
    }

    public void EndDraggingEgg() {
        dragging = false;
        SetPointer(eggPosition);
    }

    public void CrackEgg() {
        Debug.Log("Detected that!");
    }

    private void SetPointer(Vector3 position) {
        pointer.Hide();
        pointer.PointTo(position);
        StartCoroutine(CallAfter(timeBeforePointingHand, pointer.Show));
    }

    private IEnumerator CallAfter(float seconds, Action function) {
        float start = Time.time;
        while (Time.time - start < seconds) {
            yield return false;
        }
        function();
    }



}
