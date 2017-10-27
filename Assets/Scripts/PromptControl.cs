using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptControl : MonoBehaviour {

    public float popupSpeed = 10.0f;
    public Vector2 finalScale = Vector2.one;

    public GameObject spriteObject;
    public GameObject amountObject;

    public List<Sprite> numberSprites;

    private float lifeTime;
    private bool opened = false;
    private Sprite ingredientSprite;
    private int ingredientAmount;


    private void Start() {
        transform.localScale = Vector2.zero;
    }


    public void ShowPromptAfter(float time, float lifeTime) {
        this.lifeTime = lifeTime;
        opened = false;
        StartCoroutine(ShowAfter(time));
    }

    public void SetIngredient(Sprite sprt, int amount) {
        ingredientSprite = sprt;
        ingredientAmount = amount;

        spriteObject.GetComponent<SpriteRenderer>().sprite = ingredientSprite;
        amountObject.GetComponent<SpriteRenderer>().sprite = numberSprites[amount - 1];
    }

    private IEnumerator AnimateScale(Vector3 finalScale) {
        float startTime = Time.time;
        Vector3 initialScale = transform.localScale;
        float distance = Vector3.Distance(initialScale, finalScale);
        float distCovered = 0, fracJourney = 0;
        while (fracJourney < 1) {
            distCovered = (Time.time - startTime) * popupSpeed;
            fracJourney = distCovered / distance;
            transform.localScale = Vector3.Lerp(initialScale, finalScale, fracJourney);
            yield return false;
        }
        transform.localScale = finalScale;
        if (!opened) {
            StartCoroutine(CloseAfter(lifeTime));
            opened = true;
        }
    }

    private IEnumerator ShowAfter(float time) {
        float startTime = Time.time;
        while (Time.time - startTime < time) {
            yield return false;
        }
        StartCoroutine(AnimateScale(finalScale));
    }

    private IEnumerator CloseAfter(float time) {
        float startTime = Time.time;
        while (Time.time - startTime < time) {
            yield return false;
        }
        StartCoroutine(AnimateScale(Vector3.zero));
    }
}
