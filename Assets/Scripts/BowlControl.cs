using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlControl : MonoBehaviour {

    private MixIngredientsMinigame minigameManager;

	private void Awake() {
		minigameManager = (MixIngredientsMinigame) FindObjectOfType<MinigameManager>().minigame;
    }

    /*public void OnCollisionEnter2D(Collision2D other) {
        Ingredient ingredient = other.gameObject.GetComponent<Ingredient>();
        if (ingredient != null) {
			minigameManager.AddIngredient(ingredient);
			GameObject.Destroy(other.gameObject);
        }
    }*/

    public bool DropIngredient(Vector2 position, Ingredient ingredient) {
        if(GetComponent<Collider2D>().OverlapPoint(position)) {
            minigameManager.AddIngredient(ingredient);
            ingredient.MoveTo(transform.position, true);
            return true;
        }
        return false;
    }
}
