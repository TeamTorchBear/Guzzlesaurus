
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PromptType {
    Ingredient,
    Action,
    Message
};


public class PromptControl : MonoBehaviour
{
    [Header("General Parameters")]
    public float popupSpeed = 10.0f;
    public Vector2 finalScale = Vector2.one;
    public Vector2 minimizedScale = new Vector2(0.4f, 0.4f);

    public Vector2 finalPos = Vector2.zero;
    public Vector2 minimizedPos = new Vector2(0f, -4f);

    public PromptType type;

    [Header("Ingredient prompt parameters")]
    public GameObject promptIngredientPrefab;
    public Transform anchor;
    public float separator = 4f;

    [Header("Action prompt parameters")]
    public GameObject content;


    private float lifeTime;
    private bool opened = false;

    private string ingName;
    private int amount;

    private ShelfControl shelfControl;
    private Sprite sprite;

    private void Start()
    {
        transform.localScale = Vector2.zero;
        if (type == PromptType.Ingredient)
        {
            shelfControl = FindObjectOfType<ShelfControl>();
        }
    }


    public void ShowPromptAfter(float time, float lifeTime)
    {
        this.lifeTime = lifeTime;
        opened = false;
        StartCoroutine(ShowAfter(time, null, false));
    }



    public void ShowPromptAfter(float time, float lifeTime, Action doAfter, bool after)
    {
        this.lifeTime = lifeTime;
        opened = false;
        StartCoroutine(ShowAfter(time, doAfter, after));
    }


    public void Hide(Action function)
    {
        StartCoroutine(AnimateScaleAndPosition(Vector2.zero, transform.position, function));
    }
    public void Show(Action function)
    {
        StartCoroutine(AnimateScaleAndPosition(minimizedScale, transform.position, function));
    }


    public void SetIngredient(Sprite sprt, int amount, string name)
    {

        sprite = sprt;
        ingName = name;
        this.amount = amount;
    }


    public void ChangeSprite()
    {
        foreach (Transform t in anchor)
        {
            Destroy(t.gameObject);
        }

        List<GameObject> elements = new List<GameObject>();
        for (int i = 0; i < amount; ++i)
        {
            GameObject pi = Instantiate(promptIngredientPrefab);
            pi.transform.parent = anchor;
            pi.transform.localScale = new Vector2(0.8f, 0.8f);
            pi.GetComponent<SpriteRenderer>().sprite = sprite;
            elements.Add(pi);
        }

        float position;
        if (elements.Count % 2 == 0)
        {
            position = -(elements.Count / 2) * separator + separator / 2;
        }
        else
        {
            position = -((elements.Count - 1) / 2) * separator;
        }
        foreach (GameObject e in elements)
        {
            e.transform.localPosition = new Vector2(position, 0);
            position += separator;
        }
    }

    private IEnumerator AnimateScaleAndPosition(Vector3 finalScale, Vector3 finalPosition, Action function)
    {
        float startTime = Time.time;
        Vector3 initialScale = transform.localScale;
        Vector3 initialPosition = transform.localPosition;
        float distance = Vector3.Distance(initialScale, finalScale);
        float distCovered = 0, fracJourney = 0;
        while (fracJourney < 1)
        {
            distCovered = (Time.time - startTime) * popupSpeed;
            fracJourney = distCovered / distance;
            transform.localScale = Vector3.Lerp(initialScale, finalScale, fracJourney);
            transform.localPosition = Vector3.Lerp(initialPosition, finalPosition, fracJourney);
            yield return false;
        }
        transform.localScale = finalScale;
        transform.localPosition = finalPosition;
        if (function != null)
        {
            function();
        }
    }

    public void PlayAnimations()
    {
        foreach (Animator animator in content.GetComponentsInChildren<Animator>())
        {
            if (animator != null)
            {
                Debug.Log("Anim");
                animator.Play("Animation");
            }
        }
    }

    public void SetContent(GameObject gameobject)
    {
        content.SetActive(false);
        content = gameobject;
        content.SetActive(true);
    }

    //Shows Prompt after shelf closes
    private IEnumerator ShowAfter(float time, Action function, bool after)
    {
        float startTime = Time.time;
        while (Time.time - startTime < time)
        {
            yield return false;
        }
        if (type == PromptType.Ingredient)
        {
            shelfControl.PlaceIngredients();
            PlaySound();
        }
        else if (type == PromptType.Action)
        {
            PlayAnimations();
        }

        if (after)
        {

            StartCoroutine(AnimateScaleAndPosition(finalScale, finalPos, () =>
            {
                if (!opened)
                {
                    StartCoroutine(CloseAfter(lifeTime, function));
                    opened = true;
                }
            }));
        }
        else
        {
            function();
            StartCoroutine(AnimateScaleAndPosition(finalScale, finalPos, () =>
            {
                if (!opened)
                {
                    StartCoroutine(CloseAfter(lifeTime, null));
                    opened = true;
                }
            }));
        }
    }
    //closes prompt after set period of time
    private IEnumerator CloseAfter(float time, Action function)
    {
        float startTime = Time.time;
        while (Time.time - startTime < time)
        {
            yield return false;
        }
        StartCoroutine(AnimateScaleAndPosition(minimizedScale, minimizedPos, function));
        if (type == PromptType.Ingredient)
        {
            shelfControl.OpenShelf();
        }
    }
    

    private void PlaySound() {
        switch (type) {
            case PromptType.Ingredient:
                if (amount == 1)
                {
                    AkSoundEngine.SetSwitch("Number", "One", GameObject.FindGameObjectWithTag("Prompt"));
                }
                if (amount == 2)
                {
                    AkSoundEngine.SetSwitch("Number", "Two", GameObject.FindGameObjectWithTag("Prompt"));
                }
                if (ingName == "flour")
                {
                    AkSoundEngine.SetSwitch("Ingredients", "Flour", GameObject.FindGameObjectWithTag("Prompt"));
                }
                if (ingName == "sugar")
                {
                    AkSoundEngine.SetSwitch("Ingredients", "Sugar", GameObject.FindGameObjectWithTag("Prompt"));
                }
                if (ingName == "salt")
                {
                    AkSoundEngine.SetSwitch("Ingredients", "Salt", GameObject.FindGameObjectWithTag("Prompt"));
                }
                if (ingName == "butter")
                {
                    AkSoundEngine.SetSwitch("Ingredients", "Butter", GameObject.FindGameObjectWithTag("Prompt"));
                }

                AkSoundEngine.PostEvent("IngredientPrompt", GameObject.FindGameObjectWithTag("Prompt"));
                break;
        }
    }
    
}