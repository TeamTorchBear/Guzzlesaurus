using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    
    private Interactable interactable;

    public delegate void OnButtonClickDelegate(Vector3 position);
    public static event OnButtonClickDelegate ButtonClickDownDelegate;
    public static event OnButtonClickDelegate ButtonClickHoldDelegate;
    public static event OnButtonClickDelegate ButtonClickUpDelegate;

    private void Update() {
        
        // Desktop control handling
        if (Input.GetMouseButtonDown(0)) {
            ButtonClickDownDelegate(Input.mousePosition);
        } else if (Input.GetMouseButtonUp(0)) {
            ButtonClickUpDelegate(Input.mousePosition);
        } else if (Input.GetMouseButton(0)) {
            ButtonClickHoldDelegate(Input.mousePosition);
        }

        // Touch control handling
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase) {
                case TouchPhase.Began:
                    ButtonClickDownDelegate(touch.position);
                    break;
                case TouchPhase.Moved:
					ButtonClickHoldDelegate(touch.position);
                    break;
                case TouchPhase.Canceled:
                case TouchPhase.Ended:
                    ButtonClickUpDelegate(touch.position);
                    break;
            }
        }
    }
}
