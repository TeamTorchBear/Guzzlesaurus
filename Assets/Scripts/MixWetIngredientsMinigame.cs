using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MixWetIngredientsMinigame : MonoBehaviour {

    [Header("Minigame parameters")]
    public float timeBeforePointingHand = 1f;
    public int cracksNeeded = 1;
    public float crackForceThreshold = 0f;

    [Space(25)]
    [Header("External references")]
    public PointerControl pointer;
    public Transform eggsTarget;
    public Transform bowlBorderTarget;
    public Transform hoverMarkTarget;
    public GameObject hands;
    public Animator[] handsAnimators;


    private Vector2 eggPosition;
    private bool draggingPhase = true;
    private int cracks = 0;
    private bool blockCalls = false;

    private void Start() {
        eggPosition = eggsTarget.position;
        SetPointer(eggPosition);
    }

    public void StartDraggingEgg() {
        if (draggingPhase) {
            SetPointer(bowlBorderTarget.position);
        }
    }

    public void EndDraggingEgg() {
        if (draggingPhase) {
            SetPointer(eggPosition);
        }
    }

    public void CrackEgg(EggDrag egg) {
        //Debug.Log("Detected that! " + egg.velocity);
        if ((++cracks) == cracksNeeded) {
            draggingPhase = false;
            blockCalls = true;
            egg.CancelDrag();
            egg.MoveAndRotateTo(hoverMarkTarget.position, Quaternion.Euler(egg.transform.rotation.x, egg.transform.rotation.y, 90f), true);
            pointer.Hide();
            StartEggCrackHandsAnimation();
            SeparateEggControl sec = egg.gameObject.GetComponent<SeparateEggControl>();
            sec.enabled = true;
            sec.SetPosition(hoverMarkTarget.position);
            sec.SetRotation(new Vector3(egg.transform.rotation.x, egg.transform.rotation.y, 90f));

        }
    }

    private void StartEggCrackHandsAnimation() {
        hands.SetActive(true);
        foreach (Animator animator in handsAnimators) {
            animator.Play("Animation");
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
        if (!blockCalls) {
            function();
        }
    }



}
