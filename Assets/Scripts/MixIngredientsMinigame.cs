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
    public List<IngredientNeeded> ingredientsNeeded;
    private PromptControl promptControl;
    private Dictionary<string, Sprite> ingredients;
    private Dictionary<string, int> currentIngredients;
    private int totalIngredientsAmount;
    private int currentIngredientsAmount;
    private int currentIngredient;
    private bool failed;

    public override void StartMinigame() {
        base.StartMinigame();
        promptControl = FindObjectOfType<PromptControl>();
        ingredients = new Dictionary<string, Sprite>();
        currentIngredients = new Dictionary<string, int>();
        totalIngredientsAmount = 0;
        currentIngredientsAmount = 0;
        currentIngredient = 0;
        failed = false;
        foreach (IngredientNeeded i in ingredientsNeeded) {
            totalIngredientsAmount += i.amount;
            currentIngredients[i.ingredient] = 0;
        }

        Ingredient[] ings = FindObjectsOfType<Ingredient>();
        foreach (Ingredient i in ings) {
            if (!ingredients.ContainsKey(i.ingredientName)) {
                ingredients.Add(i.ingredientName, i.ingredientSprite);
            }
            i.Init();
        }
        AskForIngredient(ingredientsNeeded[currentIngredient].ingredient, ingredientsNeeded[currentIngredient].amount);
    }

    private void AskForIngredient(string name, int amount) {
        promptControl.SetIngredient(ingredients[name], amount);
        promptControl.ShowPromptAfter(timeToPromt, promptTime);
    }

    private void NextIngredient() {
        currentIngredient++;
        currentIngredientsAmount = 0;
        if (currentIngredient == ingredientsNeeded.Count) {
            EndMinigame(CheckIngredients());
            return;
        }
        AskForIngredient(ingredientsNeeded[currentIngredient].ingredient, ingredientsNeeded[currentIngredient].amount);
    }

    public void AddIngredient(Ingredient i) {
        //Debug.Log("Added ingredient: " + i.ingredientName);
        if (!currentIngredients.ContainsKey(i.ingredientName)) {
            currentIngredients[i.ingredientName] = 0;
        }
        if(i.ingredientName != ingredientsNeeded[currentIngredient].ingredient) {
            failed = true;
        }
        currentIngredients[i.ingredientName]++;
        currentIngredientsAmount++;
        if (IsStepFinished()) {
            NextIngredient();
        }
    }

    public bool IsStepFinished() {
        //Debug.Log(currentIngredientsAmount + "/" + totalIngredientsAmount);
        return currentIngredientsAmount == ingredientsNeeded[currentIngredient].amount;
    }

    private bool CheckIngredients() {
        foreach (IngredientNeeded i in ingredientsNeeded) {
            if (currentIngredients[i.ingredient] < i.amount) {
                return false;
            }
        }
        return !failed;
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