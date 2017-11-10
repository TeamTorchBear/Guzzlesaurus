using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlControl : MonoBehaviour {

    private MixIngredientsMinigame minigameManager;

	private void Awake() {
		minigameManager = (MixIngredientsMinigame) FindObjectOfType<MinigameManager>().minigame;
    }

    public bool DropIngredient(Vector2 position, Ingredient ingredient) {
        if(GetComponent<Collider2D>().OverlapPoint(position)) {
            minigameManager.AddIngredient(ingredient);
            ingredient.MoveTo(transform.position, true, null);
            return true;
        }
        return false;
    }
}
