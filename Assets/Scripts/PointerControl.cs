using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerControl : MonoBehaviour {

    [Header("Pointing parameters")]
    public float amplitude = 0.5f;
    public float frequency = 0.5f;
    public Vector2 offset = new Vector2(0.5f, 0.5f);

    
    private enum ACTION {
        POINT,
        DRAG
    };

    private Vector2 position;
    private ACTION action;

    private void Start() {
        action = ACTION.POINT;
        position = transform.position;
    }

    private void FixedUpdate() {
        switch (action) {
            case ACTION.POINT:
                position.y = Mathf.Sin(Time.time * frequency) * amplitude;
                transform.localPosition = position;
                break;
            case ACTION.DRAG:
                break;
        }
    }

    public void PointTo(Vector2 to) {
        action = ACTION.POINT;
        // Look at target
        Vector2 diff = to - (Vector2)transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

        // Position
        transform.position = to + offset;
    }

    public void Show() {
        GetComponentInChildren<SpriteRenderer>().enabled = true;
    }

    public void Hide() {
        GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

}
