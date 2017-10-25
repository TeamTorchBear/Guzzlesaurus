using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : Interactable {

    private Collider2D objectCollider;

    private void Awake() {
        objectCollider = GetComponentInChildren<Collider2D>();
    }

    public override void OnInteractionStart(Vector3 position) {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(position);
        Vector2 touchPos = new Vector2(worldPos.x, worldPos.y);
        if (objectCollider == Physics2D.OverlapPoint(touchPos)) {
            OnClick();
        }
    }

    public override void OnInteractionEnd(Vector3 position) {
    }

    public override void OnInteractionHold(Vector3 position) {
    }


    public virtual void OnClick(){
        
    }

}
