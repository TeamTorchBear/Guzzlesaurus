using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : Interactable {

    public bool returnToPosition = false;
    public float returnSpeed = 20f;
    private BoxCollider2D boxCollider;
    private Vector3 offset;
    private bool dragging = false;

    [HideInInspector]
    public Vector3 initialPosition;

    private void Awake() {
        boxCollider = GetComponent<BoxCollider2D>();
        initialPosition = transform.position;
    }

    public override void OnInteractionStart(Vector3 position) {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(position);
        Vector2 touchPos = new Vector2(worldPos.x, worldPos.y);
        if (boxCollider == Physics2D.OverlapPoint(touchPos)) {
            dragging = true;
            offset = boxCollider.bounds.center - worldPos;
            OnDragStart();
        }
    }

    public override void OnInteractionHold(Vector3 position) {
        if (dragging) {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(position);
            this.transform.position = new Vector3(worldPos.x, worldPos.y, 0) + offset;
            OnDragHold();
        }
    }

    public override void OnInteractionEnd(Vector3 position) {
        if (dragging) {
            if (returnToPosition) {
                MoveTo(initialPosition, false);
            }
            dragging = false;
            OnDragEnd();
        }
    }

    public virtual void OnDragStart() {
    }
    public virtual void OnDragHold() {
    }
    public virtual void OnDragEnd() {
    }

    private void OnDestroy() {
        dragging = false;
    }

    public void MoveTo(Vector2 position, bool destroyAfter) {
        StartCoroutine(AnimatePosition(position, destroyAfter));
    }


    private IEnumerator AnimatePosition(Vector3 finalPos, bool destroyAfter) {
        float startTime = Time.time;
        Vector3 initialPos = transform.position;
        float distance = Vector3.Distance(initialPos, finalPos);
        float distCovered = 0, fracJourney = 0;
        if (distance > 0) {
            while (fracJourney < 1) {
                distCovered = (Time.time - startTime) * returnSpeed;
                fracJourney = distCovered / distance;
                transform.position = Vector3.Lerp(initialPos, finalPos, fracJourney);
                yield return false;
            }
            transform.position = finalPos;
            if (destroyAfter) {
                Destroy(this.gameObject);
            }
        }
    }
}
