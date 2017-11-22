using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugControl : Draggable {

    public MixWetIngredientsMinigame minigame;
    public Transform showMark;
    public Transform milkMask;
    public float finalScale;

    public void Show() {
        StartCoroutine(AnimatePosition(showMark.position, false, null));
        initialPosition = showMark.position;
    }

    public void EnableDrag() {
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public override void OnDragStart() {
        base.OnDragStart();
        minigame.StartDraggingJug();
    }

    public override void OnDragEnd() {
        base.OnDragEnd();
        minigame.EndDraggingJug();

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Done");
    }
}
