using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : Draggable {
    public string ingredientName;
    public Sprite ingredientSprite;
    public Sprite draggingSprite;

    private SpriteRenderer spriteRenderer;
    private ShelfControl shelf;
    private BowlControl bowl;
    private bool draggingIngredient = false;

    public void Init() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        ToggleSprite();
        shelf = FindObjectOfType<ShelfControl>();
        bowl = FindObjectOfType<BowlControl>();
        initialPosition = transform.position;
    }

    public override void OnDragStart() {
        base.OnDragStart();
        Debug.Log("OnDragStart()");
        initialPosition = transform.position;
        ToggleSprite();
    }

    public override void OnDragEnd() {
        base.OnDragEnd();
        Debug.Log("OnDragEnd()");
        if (!bowl.DropIngredient(transform.position, this)) {
            MoveTo(initialPosition, false, ToggleSprite);
        }
    }

    private void ToggleSprite() {
        if (!draggingIngredient) {
            spriteRenderer.sprite = ingredientSprite;
        } else {
            spriteRenderer.sprite = draggingSprite;
        }
        draggingIngredient = !draggingIngredient;
    }


}
