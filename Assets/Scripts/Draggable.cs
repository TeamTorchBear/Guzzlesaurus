using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour {

    private bool dragging;
    private BoxCollider2D boxCollider;
    private Vector3 offset;

    private void Start() {
        boxCollider = this.GetComponent<BoxCollider2D>();
        dragging = false;
    }

    private void FixedUpdate() {
        Vector2 touchPos = new Vector2();
        Vector3 screenPos, worldPos;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0)) {
            screenPos = Input.mousePosition;
            screenPos.z = this.transform.position.z;
            worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            touchPos = new Vector2(worldPos.x, worldPos.y);
            if (boxCollider == Physics2D.OverlapPoint(touchPos)) {
                dragging = true;
                offset = boxCollider.bounds.center - worldPos;
            }
        } else if (Input.GetMouseButton(0) && dragging) {
            screenPos = Input.mousePosition;
            screenPos.z = this.transform.position.z;
            worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            this.transform.position = new Vector3(worldPos.x, worldPos.y, 0) + offset;
        } else {
            dragging = false;
        }
#else


        if (Input.touchCount > 0) {
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began) {
                screenPos = touch.position;
                screenPos.z = this.transform.position.z;
                worldPos = Camera.main.ScreenToWorldPoint(screenPos);
                touchPos = new Vector2(worldPos.x, worldPos.y);
                if (boxCollider == Physics2D.OverlapPoint(touchPos)) {
                    dragging = true;
                    offset = boxCollider.bounds.center - worldPos;
                }
            } else if ((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && dragging) {
                screenPos = touch.position;
                screenPos.z = this.transform.position.z;
                worldPos = Camera.main.ScreenToWorldPoint(screenPos);
                this.transform.position = new Vector3(worldPos.x, worldPos.y, 0) + offset;
            }
        } else {
            dragging = false;
        }
#endif
    }
}
