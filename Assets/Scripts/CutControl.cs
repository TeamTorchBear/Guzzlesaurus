using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutControl : Interactable {

    public Collider2D topPointCollider;
    public Collider2D botPointCollider;
    public Collider2D pathCollider;

    private LineRenderer lineRenderer;
    private List<Vector3> linePoints = new List<Vector3>();
    private Collider2D endCollider;
    private bool cutting = false;

    public override void OnInteractionStart(Vector3 position) {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(position);
        if (topPointCollider.OverlapPoint(worldPos)) {
            cutting = true;
            endCollider = botPointCollider;
        } else if (botPointCollider.OverlapPoint(worldPos)) {
            cutting = true;
            endCollider = topPointCollider;
        }
    }

    public override void OnInteractionHold(Vector3 position) {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(position);
        if (!(pathCollider.OverlapPoint(worldPos) || topPointCollider.OverlapPoint(worldPos) || botPointCollider.OverlapPoint(worldPos))) {
            EndCut(false);
            return;
        }
        if (cutting) {
            linePoints.Add(new Vector2(worldPos.x, worldPos.y));
            if (endCollider.OverlapPoint(worldPos)) {
                EndCut(true);
            }
        }
    }

    public override void OnInteractionEnd(Vector3 position) {
        EndCut(true);
    }

    private void EndCut(bool success) {
        // If the action was successfully performed, do something (animation, sound, etc.)
        cutting = false;
        linePoints = new List<Vector3>();
    }

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update() {
        if (linePoints.Count >= 0) {
            lineRenderer.positionCount = linePoints.Count;
            lineRenderer.SetPositions(linePoints.ToArray());
        }
    }
}
