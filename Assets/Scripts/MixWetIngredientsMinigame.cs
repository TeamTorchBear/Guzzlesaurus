using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MixWetIngredientsMinigame : MonoBehaviour {

    [Header("Minigame parameters")]
    public float timeBeforePointingHand = 1f;
    public int cracksNeeded = 1;
    public float crackForceThreshold = 0f;

    [Header("External references")]
    public PointerControl pointer;
    public Transform eggsTarget;
    public Transform bowlBorderTarget;
    public Transform hoverMarkTarget;



    private Vector2 eggPosition;
    private bool dragging = false;
    private int cracks = 0;

    private void Start() {
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

    public void CrackEgg(EggDrag egg) {
        //Debug.Log("Detected that! " + egg.velocity);
        if (egg.velocity > crackForceThreshold && (++cracks) == cracksNeeded) {
            egg.CancelDrag();
            egg.MoveAndRotateTo(hoverMarkTarget.position, Quaternion.Euler(egg.transform.rotation.x, egg.transform.rotation.y, 90f));
            pointer.Hide();
        }
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
