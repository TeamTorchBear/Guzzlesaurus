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
    private float startTime;

    private Vector2 lastPos = Vector2.zero;
    public float speed;
    public float relativePosition;

    public void Init() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        ToggleSprite();
        shelf = FindObjectOfType<ShelfControl>();
        bowl = FindObjectOfType<BowlControl>();
        initialPosition = transform.position;

    }
    public override void OnDragHold() {
        base.OnDragHold();
        speed = Vector2.Distance(transform.position, lastPos) / (Time.deltaTime);
        lastPos = transform.position;
        relativePosition = GetXPosition(180);
    }

    public override void OnDragStart() {
        base.OnDragStart();
        if (ingredientName == "flour") {
            AkSoundEngine.SetSwitch("Ingredient_Pickup", "Flour", GameObject.FindGameObjectWithTag("Ingredients"));
        }
        if (ingredientName == "butter") {
            AkSoundEngine.SetSwitch("Ingredient_Pickup", "Butter", GameObject.FindGameObjectWithTag("Ingredients"));
        }
        if (ingredientName == "sugar") {
            AkSoundEngine.SetSwitch("Ingredient_Pickup", "Sugar", GameObject.FindGameObjectWithTag("Ingredients"));
        }
        if (ingredientName == "salt") {
            AkSoundEngine.SetSwitch("Ingredient_Pickup", "Salt", GameObject.FindGameObjectWithTag("Ingredients"));
        }

        AkSoundEngine.PostEvent("Ingredient_Pickup", GameObject.FindGameObjectWithTag("Ingredients"));

        startTime = Time.time;

        initialPosition = transform.position;
        ToggleSprite();
    }

    public override void OnDragEnd() {
        base.OnDragEnd();
        if (!bowl.DropIngredient(transform.position, this)) {
            MoveTo(initialPosition, false, ToggleSprite);
            if (ingredientName == "flour") {
                AkSoundEngine.SetSwitch("Ingredient_Down", "Flour", GameObject.FindGameObjectWithTag("Ingredients"));
            }
            if (ingredientName == "butter") {
                AkSoundEngine.SetSwitch("Ingredient_Down", "Butter", GameObject.FindGameObjectWithTag("Ingredients"));
            }
            if (ingredientName == "sugar") {
                AkSoundEngine.SetSwitch("Ingredient_Down", "Sugar", GameObject.FindGameObjectWithTag("Ingredients"));
            }
            if (ingredientName == "salt") {
                AkSoundEngine.SetSwitch("Ingredient_Down", "Salt", GameObject.FindGameObjectWithTag("Ingredients"));
            }

            AkSoundEngine.PostEvent("Ingredient_Down", GameObject.FindGameObjectWithTag("Ingredients"));
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

    // if the min/max is -90/90, range is 180
    private float GetXPosition(float range) {
        Vector2 origin = Camera.main.ViewportToWorldPoint(Vector2.zero);
        return -(transform.position.x / origin.x) * (range / 2);
    }
}
