using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PromptType {
    Ingredient,
    Action,
    Message
};

public class PromptControl : MonoBehaviour {

    [Header("General Parameters")]
    public float popupSpeed = 10.0f;
    public Vector2 finalScale = Vector2.one;
    public Vector2 minimizedScale = new Vector2(0.4f, 0.4f);

    public Vector2 finalPos = Vector2.zero;
    public Vector2 minimizedPos = new Vector2(0f, -4f);

    public PromptType type;

    [Header("Ingredient prompt parameters")]
    public GameObject spriteObject;
    public GameObject amountObject;
    public List<Sprite> numberSprites;

    [Header("Action prompt parameters")]
    public GameObject content;


    private float lifeTime;
    private bool opened = false;

    private string ingName;
    private int amount;

    private ShelfControl shelfControl;

    private Action function;

    private void Start() {
        transform.localScale = Vector2.zero;
        if (type == PromptType.Ingredient) {
            shelfControl = FindObjectOfType<ShelfControl>();
        }
    }

    public void ShowPromptAfter(float time, float lifeTime) {
        this.lifeTime = lifeTime;
        opened = false;
        StartCoroutine(ShowAfter(time));
    }
    public void ShowPromptAfter(float time, float lifeTime, Action doAfter) {
        this.lifeTime = lifeTime;
        opened = false;
        StartCoroutine(ShowAfter(time));
        function = doAfter;
    }

    public void Hide() {
        StartCoroutine(AnimateScaleAndPosition(Vector2.zero, transform.position));
    }

    public void SetIngredient(Sprite sprt, int amount, string name) {
        spriteObject.GetComponent<SpriteRenderer>().sprite = sprt;
        amountObject.GetComponent<SpriteRenderer>().sprite = numberSprites[amount - 1];
        ingName = name;
        this.amount = amount;
    }

    private IEnumerator AnimateScaleAndPosition(Vector3 finalScale, Vector3 finalPosition) {
        float startTime = Time.time;
        Vector3 initialScale = transform.localScale;
        Vector3 initialPosition = transform.localPosition;
        float distance = Vector3.Distance(initialScale, finalScale);
        float distCovered = 0, fracJourney = 0;
        while (fracJourney < 1) {
            distCovered = (Time.time - startTime) * popupSpeed;
            fracJourney = distCovered / distance;
            transform.localScale = Vector3.Lerp(initialScale, finalScale, fracJourney);
            transform.localPosition = Vector3.Lerp(initialPosition, finalPosition, fracJourney);
            yield return false;
        }
        transform.localScale = finalScale;
        transform.localPosition = finalPosition;
        if (!opened) {
            StartCoroutine(CloseAfter(lifeTime));
            opened = true;
        }
    }

    //Shows Prompt after shelf closes
    private IEnumerator ShowAfter(float time) {
        float startTime = Time.time;
        while (Time.time - startTime < time) {
            yield return false;
        }
        if (type == PromptType.Ingredient) {
            shelfControl.PlaceIngredients();
            PlaySound();
        } else if (type == PromptType.Action) {
            Animator animator = content.GetComponent<Animator>();
            if (animator == null) {
                Debug.Log("Content animator is missing!");
            } else {
                animator.Play("Animation");
            }
        }
        StartCoroutine(AnimateScaleAndPosition(finalScale, finalPos));
    }
    //closes prompt after set period of time
    private IEnumerator CloseAfter(float time) {
        float startTime = Time.time;
        while (Time.time - startTime < time) {
            yield return false;
        }
        StartCoroutine(AnimateScaleAndPosition(minimizedScale, minimizedPos));
        if (type == PromptType.Ingredient) {
            shelfControl.OpenShelf();
        }
        if (function != null) {
            function();
            function = null;
        }
    }

    private void PlaySound() {
        switch (type) {
            case PromptType.Ingredient:
                if (amount == 1) {
                    AkSoundEngine.SetSwitch("Number", "One", GameObject.FindGameObjectWithTag("Prompt"));
                }
                if (amount == 2) {
                    AkSoundEngine.SetSwitch("Number", "Two", GameObject.FindGameObjectWithTag("Prompt"));
                }
                if (ingName == "flour") {
                    AkSoundEngine.SetSwitch("Ingredients", "Flour", GameObject.FindGameObjectWithTag("Prompt"));
                }
                if (ingName == "sugar") {
                    AkSoundEngine.SetSwitch("Ingredients", "Sugar", GameObject.FindGameObjectWithTag("Prompt"));
                }
                if (ingName == "salt") {
                    AkSoundEngine.SetSwitch("Ingredients", "Salt", GameObject.FindGameObjectWithTag("Prompt"));
                }
                if (ingName == "butter") {
                    AkSoundEngine.SetSwitch("Ingredients", "Butter", GameObject.FindGameObjectWithTag("Prompt"));
                }

                AkSoundEngine.PostEvent("IngredientPrompt", GameObject.FindGameObjectWithTag("Prompt"));
                break;
        }
    }
}
