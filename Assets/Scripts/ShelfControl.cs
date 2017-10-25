using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfControl : Clickable {

    public float animationSpeed;

    public Collider2D buttonCollider;

    private bool opened = false;
    private bool animating = false;
    private Vector2 closePos = new Vector2(0f, 9f);
    private Vector2 openedPos = Vector2.zero;

    public override void OnClick() {
        if (!animating) {
            if (opened) {
                StartCoroutine(AnimatePosition(closePos));
            } else {
                StartCoroutine(AnimatePosition(openedPos));
            }
            opened = !opened;
        }
    }

    private IEnumerator AnimatePosition(Vector3 finalPos) {
        animating = true;
        float startTime = Time.time;
        Vector3 initialPos = transform.position;
        float distance = Vector3.Distance(initialPos, finalPos);
        float distCovered = 0, fracJourney = 0;
        if (distance > 0) {
            while (fracJourney < 1) {
                distCovered = (Time.time - startTime) * animationSpeed;
                fracJourney = distCovered / distance;
                transform.position = Vector3.Lerp(initialPos, finalPos, fracJourney);
                yield return false;
            }
            transform.position = finalPos;
            ToggleButtonRotation();
            animating = false;
        }
    }

    private void ToggleButtonRotation() {
        buttonCollider.transform.Rotate(0, 0, 180);
    }


}
