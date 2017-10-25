using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : Interactable {

    private BoxCollider2D boxCollider;
    private Vector3 offset;
    private bool dragging = false;

    private void Awake() {
        boxCollider = GetComponent<BoxCollider2D>();
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

}
