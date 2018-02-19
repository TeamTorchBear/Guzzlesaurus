using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pail : Draggable {

    [SerializeField]
    private Sprite filledSprite;
    private Sprite emptySprite;

    private SpriteRenderer pailRenderer;
    private Collider2D pailCollider;

    public override void OnStart() {
        base.OnStart();

        pailRenderer = GetComponent<SpriteRenderer>();
        pailCollider = GetComponent<Collider2D>();

        emptySprite = pailRenderer.sprite;
    }

    public void SetFill(bool value) {
        SetColliderActive(value);
        pailRenderer.sprite = value ? filledSprite : emptySprite;
    }

    public void SetColliderActive(bool value) {
        pailCollider.enabled = value;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Pasteurizer pasteurizer = other.GetComponent<Pasteurizer>();
        if(pasteurizer != null) {
            SetFill(false);
            pasteurizer.MilkDragged();
            return;
            AkSoundEngine.PostEvent("TapCow", gameObject);
        }

        Churner churner = other.GetComponent<Churner>();
        if(churner != null) {
            SetFill(false);
            churner.MilkDragged();
            return;
        }
        
    }

}
