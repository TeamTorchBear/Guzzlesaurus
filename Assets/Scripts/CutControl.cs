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
    private bool inverseCutting = false;

    public override void OnInteractionStart(Vector3 position) {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(position);
        if (topPointCollider.OverlapPoint(worldPos)) {
            Debug.Log("TOP");
            cutting = true;
            endCollider = botPointCollider;
        } else if (botPointCollider.OverlapPoint(worldPos)) {
            Debug.Log("BOT");
            cutting = true;
            endCollider = topPointCollider;
        }
    }

    public override void OnInteractionHold(Vector3 position) {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(position);
        if (!(pathCollider.OverlapPoint(worldPos) || topPointCollider.OverlapPoint(worldPos) || botPointCollider.OverlapPoint(worldPos))) {
            cutting = false;
            return;
        }
        if (cutting) {
            linePoints.Add(new Vector2(worldPos.x, worldPos.y));
            if(endCollider.OverlapPoint(worldPos)) {
                EndCut();
            }
        }
    }

    public override void OnInteractionEnd(Vector3 position) {
        cutting = false;
        linePoints = new List<Vector3>();
    }

    private void EndCut() {
        Debug.Log("CUT!");
    }

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update() {
        lineRenderer.positionCount = linePoints.Count;
        lineRenderer.SetPositions(linePoints.ToArray());
    }

}
