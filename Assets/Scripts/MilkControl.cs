using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkControl : Clickable {

    public float animationSpeed = 20f;
    public Vector3 hoverRotation = new Vector3(0f, 0f, 60f);

    public Transform hoverMark;
    public MixWetIngredientsMinigame minigame;

    private bool hovering = false;

    public override void OnClick() {
        if (!hovering) {
            hovering = true;
            minigame.HoverMilk();
        }

    }

    private void Update() {
        if (hovering) {
            float test = Input.acceleration.z;
            Debug.Log(test);
            test += hoverRotation.z;
            transform.localRotation = Quaternion.Euler(0, 0, test);
        }
    }

    public void Hover() {
        StartCoroutine(AnimatePositionAndRotation(hoverMark.position, Quaternion.Euler(hoverRotation), PourMilk));
    }

    private void PourMilk() {
        Debug.Log("PourMilk()");
    }

    public void StopPouring() {
        Vector2 pos = new Vector2(transform.position.x + 2, transform.position.y);
        StartCoroutine(AnimatePositionAndRotation(pos, Quaternion.Euler(Vector3.zero), StopPouringMilk));
    }

    private void StopPouringMilk() {

    }


    private IEnumerator AnimatePositionAndRotation(Vector3 finalPos, Quaternion finalRotation, Action function) {
        float startTime = Time.time;
        Vector3 initialPos = transform.position;
        Quaternion initialRotation = transform.rotation;
        float distance = Vector3.Distance(initialPos, finalPos);
        float distCovered = 0, fracJourney = 0;
        if (distance > 0) {
            while (fracJourney < 1) {
                distCovered = (Time.time - startTime) * animationSpeed;
                fracJourney = distCovered / distance;
                transform.position = Vector3.Lerp(initialPos, finalPos, fracJourney);
                transform.rotation = Quaternion.Lerp(initialRotation, finalRotation, fracJourney);
                yield return false;
            }
            transform.position = finalPos;
            transform.rotation = finalRotation;

            function();
        }
    }
}
