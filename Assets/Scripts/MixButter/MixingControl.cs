using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixingControl : Interactable {

    public Transform debugMark;
    public float multiplier = 0.02f;

    [SerializeField]
    private Collider2D outCollider;
    [SerializeField]
    private Collider2D inCollider;

    private float lx;
    private float ly;

    private bool mixing = false;
    private float speed = 0f;

    public override void OnInteractionStart(Vector3 position) {
        Vector2 touchPos = ScreenToWorldTouch(position);
        if (outCollider.OverlapPoint(touchPos) && !inCollider.OverlapPoint(touchPos)) {
            // The touch was made inside the desired area

            mixing = true;
            lx = touchPos.x;
            ly = touchPos.y;
        }
    }
    public override void OnInteractionHold(Vector3 position) {
        if(!mixing) {
            return;
        }
        Vector2 touchPos = ScreenToWorldTouch(position);
        if(!(outCollider.OverlapPoint(touchPos) && !inCollider.OverlapPoint(touchPos))) {
            mixing = false;
            Debug.Log("PointOutOfBounds");
            return;
        }

        speed = (Vector2.Distance(touchPos, new Vector2(lx, ly))) / Time.deltaTime;

        if((touchPos.y > 0.0 && touchPos.x < (lx - (multiplier * speed))) ||
           (touchPos.y < -0.0 && touchPos.x > (lx + (multiplier * speed))) ||
           (touchPos.x > 0.0 && touchPos.y > (ly + (multiplier * speed))) ||
           (touchPos.x < -0.0 && touchPos.y < (ly - (multiplier * speed)))) {

            Debug.Log("Not that way! \nLast position (" + lx + ", " + ly + ")\nCurrent Position (" + touchPos.x + ", " + touchPos.y + ")\nSpeed: " + speed);
            mixing = false;
            return;

        }

        lx = touchPos.x;
        ly = touchPos.y;

        debugMark.position = touchPos;




    }
	public override void OnInteractionEnd(Vector3 position) {
        mixing = false;
	}
}
