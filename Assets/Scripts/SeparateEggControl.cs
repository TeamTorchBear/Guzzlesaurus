using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparateEggControl : Interactable {
    public enum SIDE {
        LEFT = 0,
        RIGHT
    }

    public SIDE side = SIDE.LEFT;

    [Header("Developer magic parameters")]
    public Vector2 scale = new Vector2(2f, 10f);
    public float cap = 2f;
    public float rotation = -40f;

    private BoxCollider2D boxCollider;
    private bool interacting = false;
    private Vector2 initialPosition;
    private Vector3 initialRotation;
    private Vector2 initialPoint;
    private Vector2 delta;


    private void Awake() {
        boxCollider = GetComponent<BoxCollider2D>();
        initialPosition = transform.position;
        initialRotation = transform.localEulerAngles;
    }

    public void SetPosition(Vector2 pos) {
        initialPosition = pos;
    }

    public void SetRotation(Vector3 angles) {
        initialRotation = angles;
    }


    public override void OnInteractionStart(Vector3 position) {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(position);
        initialPoint = new Vector2(worldPos.x, worldPos.y);
        if (boxCollider == Physics2D.OverlapPoint(initialPoint)) {
            interacting = true;
            delta = Vector2.zero;
        }
    }

    public override void OnInteractionHold(Vector3 position) {
        if (!interacting) {
            return;
        }
        Vector2 currentPoint = Camera.main.ScreenToWorldPoint(position);
        delta = currentPoint - initialPosition;
        Vector2 transformation = new Vector2();

        /*
            max(-cap.x, delta.x) / -cap.x --> 0..1 / 2 
         */
        if (side == SIDE.LEFT) {
            if (delta.x < 0 && delta.y > 0) {
                delta.x /= scale.x;
                delta.y /= scale.y;
                delta.x = Mathf.Max(-cap, delta.x) / (-cap * 2);
                delta.y = Mathf.Min(cap * cap, delta.y) / (cap * cap * 2);

                float magnitude = delta.x + delta.y;
                transformation.x = (-delta.x * magnitude) * cap;
                transformation.y = transformation.x * transformation.x;
                transform.position = initialPosition + transformation;

                Vector3 newRotation = initialRotation;
                newRotation.z += rotation * magnitude;
                transform.localEulerAngles = newRotation;
            }
        } else if (side == SIDE.RIGHT) {
            if (delta.x > 0 && delta.y > 0) {
                delta.x /= scale.x;
                delta.y /= scale.y;
                delta.x = Mathf.Min(cap, delta.x) / (cap * 2);
                delta.y = Mathf.Min(cap * cap, delta.y) / (cap * cap * 2);

                float magnitude = delta.x + delta.y;
                transformation.x = (delta.x * magnitude) * cap;
                transformation.y = transformation.x * transformation.x;
                transform.position = initialPosition + transformation;

                Vector3 newRotation = initialRotation;
                newRotation.z += rotation * magnitude;
                transform.localEulerAngles = newRotation;
            }
        }




    }

    public override void OnInteractionEnd(Vector3 position) {
        interacting = false;
    }
}
