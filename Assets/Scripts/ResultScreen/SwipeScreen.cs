using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeScreen : Draggable {

    [SerializeField, Range(0, 1)]
    private float sensitivity = 0.2f;

    [SerializeField]
    private float cameraFinalPosX = 16.99f;

    private Vector2 cameraViewSize;
    private Vector3 cameraInitialPos;

    private bool swiped = false;

    public override void OnStart() {
        cameraViewSize = Camera.main.ViewportToWorldPoint(Vector2.zero);
        cameraInitialPos = Camera.main.transform.position;
    }

    public override void OnDragStart() {

    }

    public override void OnDragHold() {

        float x = transform.position.x - initialPosition.x;
        if (x > 0) return;

        // Make x positive
        x = -x;

        // Move camera to the right
        Vector3 cameraPos = Camera.main.transform.position;
        Camera.main.transform.position = new Vector3(cameraPos.x + x, cameraPos.y, cameraPos.z);

        // Check if the distance covered with the sensitivity
        swiped = x > cameraViewSize.x * sensitivity;

    }

    public override void OnDragEnd() {
        if (!swiped) {
            StartCoroutine(AnimatePosition(Camera.main.gameObject, cameraInitialPos, false, null));
        } else {
            StartCoroutine(AnimatePosition(Camera.main.gameObject, new Vector3(cameraFinalPosX, cameraInitialPos.y, cameraInitialPos.z), false, () => {
                FindObjectOfType<Results>().ShowCoin();
            }));
        }
    }
}
