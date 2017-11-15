using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    [HideInInspector]
    public Vector3 initialPosition;

    private void Start() {
        InputManager.ButtonClickDownDelegate += OnInteractionStart;
        InputManager.ButtonClickHoldDelegate += OnInteractionHold;
        InputManager.ButtonClickUpDelegate += OnInteractionEnd;

        initialPosition = transform.position;
    }

    public abstract void OnInteractionStart(Vector3 position);
    public abstract void OnInteractionHold(Vector3 position);
    public abstract void OnInteractionEnd(Vector3 position);

    private void OnDestroy() {
        InputManager.ButtonClickDownDelegate -= OnInteractionStart;
        InputManager.ButtonClickHoldDelegate -= OnInteractionHold;
        InputManager.ButtonClickUpDelegate -= OnInteractionEnd;
    }

    protected Vector2 ScreenToWorldTouch(Vector3 position) {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(position);
        return worldPos;
    }
}
