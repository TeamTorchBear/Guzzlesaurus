using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct IngredientNeeded {
    public string ingredient;
    public int amount;
}
[CreateAssetMenu(menuName = "Guzzlesaurus/Minigames/MixIngredients")]
public class MixIngredientsMinigame : Minigame {
    public List<IngredientNeeded> ingredients;

    private PromptControl promptControl;
    private Dictionary<string, int> currentIngredients;
    private int totalIngredientsAmount;
    private int currentIngredientsAmount;

    public override void StartMinigame() {
        base.StartMinigame();
        promptControl = FindObjectOfType<PromptControl>();
        currentIngredients = new Dictionary<string, int>();
        totalIngredientsAmount = 0;
        currentIngredientsAmount = 0;
        foreach (IngredientNeeded i in ingredients) {
            totalIngredientsAmount += i.amount;
            currentIngredients[i.ingredient] = 0;
        }

        Ingredient[] ings = FindObjectsOfType<Ingredient>();
        foreach (Ingredient i in ings) {
            i.Init();
        }

        promptControl.ShowPromptAfter(timeToPromt, promptTime);
    }

    public void AddIngredient(Ingredient i) {
        //Debug.Log("Added ingredient: " + i.ingredientName);
        if (!currentIngredients.ContainsKey(i.ingredientName)) {
            currentIngredients[i.ingredientName] = 0;
        }
        currentIngredients[i.ingredientName]++;
        currentIngredientsAmount++;
        if (IsFinished()) {
            EndMinigame(CheckIngredients());
        }
    }

    public bool IsFinished() {
        //Debug.Log(currentIngredientsAmount + "/" + totalIngredientsAmount);
        return currentIngredientsAmount == totalIngredientsAmount;
    }

    private bool CheckIngredients() {
        foreach (IngredientNeeded i in ingredients) {
            if (currentIngredients[i.ingredient] < i.amount) {
                return false;
            }
        }
        return true;
    }

    private void EndMinigame(bool success) {
        // TODO
        if (success) {
            Debug.Log("Good Job!");
        } else {
            Debug.Log("You should try harder...");
        }

    }


}