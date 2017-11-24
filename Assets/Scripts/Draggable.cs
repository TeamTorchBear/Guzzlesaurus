using System;
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
    public float velocity;
    private Vector2 lastPos;
    private bool startedDragging = false;

    private void Awake() {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public override void OnInteractionStart(Vector3 position) {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(position);
        Vector2 touchPos = new Vector2(worldPos.x, worldPos.y);
        if (boxCollider == Physics2D.OverlapPoint(touchPos)) {
            startedDragging = true;
            offset = boxCollider.bounds.center - worldPos;
            offset.z = 0;
            lastPos = worldPos;
        }
    }

    public override void OnInteractionHold(Vector3 position) {
        if (startedDragging) {
            dragging = true;
            OnDragStart();
            startedDragging = false;
        }
        if (dragging) {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(position);
            this.transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z) + offset;
            OnDragHold();
            Vector2 delta = ((Vector2)worldPos - lastPos) / Time.deltaTime;
            velocity = delta.magnitude;
            lastPos = worldPos;
        }
    }

    public override void OnInteractionEnd(Vector3 position) {
        if (startedDragging) {
            boxCollider.enabled = true;
            startedDragging = false;
            OnDragEnd();
            return;
        }
        if (dragging) {
            if (returnToPosition) {
                boxCollider.enabled = false;
                MoveTo(initialPosition, false, () => {
                    boxCollider.enabled = true;
                });
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
        InputManager.ButtonClickDownDelegate -= OnInteractionStart;
        InputManager.ButtonClickHoldDelegate -= OnInteractionHold;
        InputManager.ButtonClickUpDelegate -= OnInteractionEnd;
        dragging = false;
    }

    public void MoveTo(Vector3 position, bool destroyAfter, Action function) {
        StartCoroutine(AnimatePosition(position, destroyAfter, function));
    }

    public void MoveAndRotateTo(Vector3 position, float rotation, bool destroyAfter, Action function) {
        StartCoroutine(AnimatePositionAndRotation(position, rotation, destroyAfter, function));
    }

    public void CancelDrag() {
        dragging = false;
    }

    protected IEnumerator AnimatePosition(Vector3 finalPos, bool destroyAfter, Action function) {
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
        }
        if (destroyAfter) {
            Destroy(this.gameObject);
        } else {
            GetComponent<Collider2D>().enabled = true;
        }
        if (function != null) {
            function();
        }
    }

    protected IEnumerator AnimatePositionAndRotation(Vector3 finalPos, float finalRotation, bool destroyAfter, Action function) {
        float startTime = Time.time;
        Vector3 initialPos = transform.position;
        float initialRotation = transform.localEulerAngles.z;
        float distance = Vector3.Distance(initialPos, finalPos);
        float distCovered = 0, fracJourney = 0;
        if (distance > 0) {
            while (fracJourney < 1) {
                distCovered = (Time.time - startTime) * returnSpeed;
                fracJourney = distCovered / distance;
                transform.position = Vector3.Lerp(initialPos, finalPos, fracJourney);
                float z = Mathf.LerpAngle(initialRotation, finalRotation, fracJourney);
                transform.localEulerAngles = new Vector3(0, 0, z);
                transform.rotation = Quaternion.Euler(transform.localEulerAngles);
                yield return false;
            }
            transform.position = finalPos;
            transform.localEulerAngles = new Vector3(0, 0, finalRotation);
            if (destroyAfter) {
                function();
                Destroy(gameObject);
            }
        }
    }

}
