using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutControl : Interactable {

    private LineRenderer lineRenderer;
    private List<Vector2> linePoints = new List<Vector2>();



    public override bool OnInteractionStart(Vector3 position) {
        return true;
    }

    public override void OnInteractionHold(Vector3 position) {

    }

    public override void OnInteractionEnd(Vector3 position) {

    }

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void FixedUpdate() {

    }

}
