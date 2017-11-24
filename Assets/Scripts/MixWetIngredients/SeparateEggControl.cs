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
    public float threshold = 0.7f;

    [Header("Developer magic parameters")]
    public Vector2 scale = new Vector2(2f, 10f);
    public float cap = 2f;
    public float rotation = -40f;

    private MixWetIngredientsMinigame minigame;
    private BoxCollider2D boxCollider;
    private bool interacting = false;
    private Vector3 initialRotation;
    private Vector2 initialPoint;
    private Vector2 delta;
    private bool finished = false;


    private void Awake() {
        minigame = FindObjectOfType<MixWetIngredientsMinigame>();
        boxCollider = GetComponent<BoxCollider2D>();
        initialPosition = transform.position;
        initialRotation = transform.localEulerAngles;
    }

    public void Reset() {
        transform.position = initialPosition;
        transform.localEulerAngles = initialRotation;
        finished = false;
        interacting = false;
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
            //minigame.SeparatingEgg(true);
        }
    }

    public override void OnInteractionHold(Vector3 position) {
        if (!interacting) {
            return;
        }
        Vector2 currentPoint = Camera.main.ScreenToWorldPoint(position);
        delta = currentPoint - (Vector2)initialPosition;
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
                transform.position = (Vector2)initialPosition + transformation;

                Vector3 newRotation = initialRotation;
                newRotation.z += rotation * magnitude;
                transform.localEulerAngles = newRotation;

                if (magnitude > threshold && !finished) {
                    finished = true;
                    minigame.SeparatingEggCompleted();
                }
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
                transform.position = (Vector2)initialPosition + transformation;

                Vector3 newRotation = initialRotation;
                newRotation.z += rotation * magnitude;
                transform.localEulerAngles = newRotation;

                if (magnitude > threshold && !finished) {
                    minigame.SeparatingEggCompleted();
                }
            }
        }




    }

    public override void OnInteractionEnd(Vector3 position) {
        if (interacting) {
            interacting = false;
            //minigame.SeparatingEgg(false);
        }
    }
}
