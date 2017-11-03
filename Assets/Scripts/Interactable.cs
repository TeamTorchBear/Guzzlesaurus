using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    private void Start() {
        InputManager.ButtonClickDownDelegate += OnInteractionStart;
        InputManager.ButtonClickHoldDelegate += OnInteractionHold;
        InputManager.ButtonClickUpDelegate += OnInteractionEnd;
    }

    public abstract void OnInteractionStart(Vector3 position);
    public abstract void OnInteractionHold(Vector3 position);
    public abstract void OnInteractionEnd(Vector3 position);

    private void OnDestroy() {
        InputManager.ButtonClickDownDelegate -= OnInteractionStart;
        InputManager.ButtonClickHoldDelegate -= OnInteractionHold;
        InputManager.ButtonClickUpDelegate -= OnInteractionEnd;
    }
}
