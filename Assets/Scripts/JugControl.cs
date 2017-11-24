using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugControl : Draggable {

    public MixWetIngredientsMinigame minigame;
    public Transform showMark;
    public Transform milkMask;
    public float finalScale;

    public Collider2D maskCollider;

    public void Show() {
        StartCoroutine(AnimatePosition(showMark.position, false, null));
        initialPosition = showMark.position;
    }

    private void Update() {
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

    // Called when a milk particle lands into the jug
    public void Fill() {
        minigame.particlesPoured = minigame.particlesPoured + 1;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        returnToPosition = false;
        minigame.PourJugContent();
    }
}
