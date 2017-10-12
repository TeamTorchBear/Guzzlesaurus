using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {


    public float cameraSwitchSpeed = 1f;

    private enum VIEW {
        DESK = 0,
        SCREEN = 1
    }

    private Interactable interactable;
    private Interactable focusedObject = null;

    public delegate void OnButtonClickDelegate(Vector3 position);
    public static event OnButtonClickDelegate ButtonClickDownDelegate;
    public static event OnButtonClickDelegate ButtonClickHoldDelegate;
    public static event OnButtonClickDelegate ButtonClickUpDelegate;


    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            ButtonClickDownDelegate(Input.mousePosition);
        } else if (Input.GetMouseButtonUp(0)) {
            ButtonClickUpDelegate(Input.mousePosition);
        } else if (Input.GetMouseButton(0)) {
            ButtonClickHoldDelegate(Input.mousePosition);
        }
    }
}
