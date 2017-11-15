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
        uint swt;
        AkSoundEngine.GetSwitch("Ingredient_Pickup", GameObject.FindGameObjectWithTag("Ingredients"), out swt);
        Debug.Log(swt);
        if (ingredientName == "flour")
            {
            AkSoundEngine.SetSwitch("Ingredient_Pickup", "Flour", GameObject.FindGameObjectWithTag("Ingredients"));
        }
        if (ingredientName == "butter")
        {
            AkSoundEngine.SetSwitch("Ingredient_Pickup", "Butter", GameObject.FindGameObjectWithTag("Ingredients"));
        }
        if (ingredientName == "sugar")
        {
            AkSoundEngine.SetSwitch("Ingredient_Pickup", "Sugar", GameObject.FindGameObjectWithTag("Ingredients"));
        }
        if (ingredientName == "salt")
        {
            AkSoundEngine.SetSwitch("Ingredient_Pickup", "Salt", GameObject.FindGameObjectWithTag("Ingredients"));
        }
        AkSoundEngine.GetSwitch("Ingredient_Pickup", GameObject.FindGameObjectWithTag("Ingredients"), out swt);
        Debug.Log(swt);

        AkSoundEngine.PostEvent("Ingredient_Pickup", GameObject.FindGameObjectWithTag("Ingredients"));



        initialPosition = transform.position;
        ToggleSprite();
    }

    public override void OnDragEnd() {
        base.OnDragEnd();
        Debug.Log("OnDragEnd()");
        if (!bowl.DropIngredient(transform.position, this)) {
            MoveTo(initialPosition, false, ToggleSprite);

            Debug.Log(ingredientName);
            uint swt;
            AkSoundEngine.GetSwitch("Ingredient_Down", GameObject.FindGameObjectWithTag("Ingredients"), out swt);
            Debug.Log(swt);
            if (ingredientName == "flour")
            {
                AkSoundEngine.SetSwitch("Ingredient_Down", "Flour", GameObject.FindGameObjectWithTag("Ingredients"));
            }
            if (ingredientName == "butter")
            {
                AkSoundEngine.SetSwitch("Ingredient_Down", "Butter", GameObject.FindGameObjectWithTag("Ingredients"));
            }
            if (ingredientName == "sugar")
            {
                AkSoundEngine.SetSwitch("Ingredient_Down", "Sugar", GameObject.FindGameObjectWithTag("Ingredients"));
            }
            if (ingredientName == "salt")
            {
                AkSoundEngine.SetSwitch("Ingredient_Down", "Salt", GameObject.FindGameObjectWithTag("Ingredients"));
            }
            AkSoundEngine.GetSwitch("Ingredient_Down", GameObject.FindGameObjectWithTag("Ingredients"), out swt);
            Debug.Log(swt);

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
}
