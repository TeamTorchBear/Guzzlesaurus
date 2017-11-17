using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptControl : MonoBehaviour {

    public float popupSpeed = 10.0f;
    public Vector2 finalScale = Vector2.one;
    public Vector2 minimizedScale = new Vector2(0.4f, 0.4f);

    public Vector2 finalPos = Vector2.zero;
    public Vector2 minimizedPos = new Vector2(0f, -4f);

    public GameObject spriteObject;
    public GameObject amountObject;

    public List<Sprite> numberSprites;

    private float lifeTime;
    private bool opened = false;

    private string ingName;
    private int amount;

    private ShelfControl shelfControl;

    private void Start() {
        transform.localScale = Vector2.zero;
        shelfControl = FindObjectOfType<ShelfControl>();
    }

    public void ShowPromptAfter(float time, float lifeTime) {
        
        this.lifeTime = lifeTime;
        opened = false;
        StartCoroutine(ShowAfter(time));
    }

    public void Hide(){
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
        shelfControl.PlaceIngredients();
        PlaySound();
        StartCoroutine(AnimateScaleAndPosition(finalScale, finalPos));
    }
    //closes prompt after set period of time
    private IEnumerator CloseAfter(float time) {
        float startTime = Time.time;
        while (Time.time - startTime < time) {
            yield return false;
        }
        StartCoroutine(AnimateScaleAndPosition(minimizedScale, minimizedPos));
        shelfControl.OpenShelf();
    }

    private void PlaySound()
    {
        //uint swt;
        //AkSoundEngine.GetSwitch("Number", GameObject.FindGameObjectWithTag("Prompt"), out swt);
        //Debug.Log(swt);
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
        //AkSoundEngine.GetSwitch("Number", GameObject.FindGameObjectWithTag("Prompt"), out swt);
        //Debug.Log(swt); 


        AkSoundEngine.PostEvent("IngredientPrompt", GameObject.FindGameObjectWithTag("Prompt"));
    }
}
