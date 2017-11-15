using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public delegate void OnButtonClickDelegate(Vector3 position);
    public static event OnButtonClickDelegate ButtonClickDownDelegate;
    public static event OnButtonClickDelegate ButtonClickHoldDelegate;
    public static event OnButtonClickDelegate ButtonClickUpDelegate;

    private void Update() {

#if UNITY_EDITOR
        // Desktop control handling
        if (Input.GetMouseButtonDown(0)) {
            ButtonClickDownDelegate(Input.mousePosition);
        } else if (Input.GetMouseButtonUp(0)) {
            ButtonClickUpDelegate(Input.mousePosition);
        } else if (Input.GetMouseButton(0)) {
            ButtonClickHoldDelegate(Input.mousePosition);
        }
#endif
        // Touch control handling
        if (Input.touchCount > 0) {
            foreach (Touch touch in Input.touches) {

                //Touch touch = Input.GetTouch(0);
                switch (touch.phase) {
                    case TouchPhase.Began:
                        Debug.Log("Began");
                        ButtonClickDownDelegate(touch.position);
                        break;
                    case TouchPhase.Moved:
                        ButtonClickHoldDelegate(touch.position);
                        break;
                    case TouchPhase.Canceled: // If the interaction ends for any reason, we have to call the listeners
                    case TouchPhase.Ended:
                        ButtonClickUpDelegate(touch.position);
                        break;
                }
            }
        }
    }
}
