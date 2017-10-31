using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : Draggable {
    public string ingredientName;
    public Sprite ingredientSprite;

    private ShelfControl shelf;
    private BowlControl bowl;

    public void Init() {
        GetComponentInChildren<SpriteRenderer>().sprite = ingredientSprite;
        shelf = FindObjectOfType<ShelfControl>();
        bowl = FindObjectOfType<BowlControl>();
        initialPosition = transform.position;
    }

    public override void OnDragStart() {
        base.OnDragStart();
        shelf.Close();
    }

    public override void OnDragEnd() {
        base.OnDragEnd();

        if (!bowl.DropIngredient(transform.position, this)) {
            MoveTo(initialPosition, false);
        }
    }

   
}
