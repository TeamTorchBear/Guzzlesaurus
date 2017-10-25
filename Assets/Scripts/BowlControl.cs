using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlControl : MonoBehaviour {

    private MixIngredientsMinigame minigameManager;

	private void Awake() {
		minigameManager = (MixIngredientsMinigame) FindObjectOfType<MinigameManager>().minigame;
    }

    public void OnCollisionEnter2D(Collision2D other) {
        Ingredient ingredient = other.gameObject.GetComponent<Ingredient>();
        if (ingredient != null) {
			minigameManager.AddIngredient(ingredient);
			GameObject.Destroy(other.gameObject);
        }
    }
}
