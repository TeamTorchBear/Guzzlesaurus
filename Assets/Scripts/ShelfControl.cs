using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfControl : Clickable {

    public float closeAnimationSpeed = 20f;
    public float openAnimationSpeed = 10f;
    public Collider2D buttonCollider;
    public float separationX = 3f;
    public float separationY = 3f;

    private bool opened = false;
    private bool animating = false;
    private Vector2 closePos = new Vector2(0f, 9.2f);
    private Vector2 openedPos = Vector2.zero;

    public override void OnClick() {
        base.OnClick();
        if (!animating) {
            if (opened) {
                StartCoroutine(AnimatePosition(closePos, closeAnimationSpeed));
            } else {
                StartCoroutine(AnimatePosition(openedPos, openAnimationSpeed));
            }
            opened = !opened;
        }
    }

    public void PlaceIngredients() {

        List<Ingredient> ingredients = new List<Ingredient>(FindObjectsOfType<Ingredient>());

        // Randomize list of ingredients
        Shuffle(ingredients);
        float posX, posY;

        posX = -(separationX * (ingredients.Count / 4));
        posY = separationY / 2;

        int i;
        for (i = 0; i < ingredients.Count / 2; i++) {
            ingredients[i].transform.localPosition = new Vector2(posX, posY);
            ingredients[i].initialPos = ingredients[i].transform.position;
            posX += separationX;
        }

        posY -= separationY;
        posX = -(separationX * (ingredients.Count / 4));

        for (; i < ingredients.Count; i++) {
            ingredients[i].transform.localPosition = new Vector2(posX, posY);
            ingredients[i].initialPos = ingredients[i].transform.position;
            posX += separationX;
        }



    }

    private void Shuffle(List<Ingredient> ingredients) {
        int n = ingredients.Count;
        while (n > 1) {
            n--;
            int k = Random.Range(0, n + 1);
            Ingredient value = ingredients[k];
            ingredients[k] = ingredients[n];
            ingredients[n] = value;
        }
    }


    private IEnumerator AnimatePosition(Vector3 finalPos, float speed) {
        animating = true;
        float startTime = Time.time;
        Vector3 initialPos = transform.position;
        float distance = Vector3.Distance(initialPos, finalPos);
        float distCovered = 0, fracJourney = 0;
        if (distance > 0) {
            while (fracJourney < 1) {
                distCovered = (Time.time - startTime) * speed;
                fracJourney = distCovered / distance;
                transform.position = Vector3.Lerp(initialPos, finalPos, fracJourney);
                yield return false;
            }
            transform.position = finalPos;
            animating = false;
            ToggleButtonRotation();
        }
    }

    private void ToggleButtonRotation() {
        buttonCollider.transform.Rotate(0, 0, 180);
    }
}
