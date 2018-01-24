using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public enum COLLECTING_MODE {
    ALL_AT_ONCE,
    ONE_AT_A_TIME
}

public class CollectIngridient : Clickable {

    public COLLECTING_MODE collectingMode;

    public GameObject eggs, flour, sugar, salt, milk, butter;
    public Canvas canvas;
    public Transform child;

    public GameObject ingredients;
    public float timeBetweenShots;

    Data data;
    Image eggsimage, flourimage, sugarimage, saltimage, milkimage, butterimage;

    // Use this for initialization
    public override void OnStart() {
    }

    public override void OnClick() {
        GetComponentInChildren<Animator>().Play("ws_farmShoot");
        try {
            data = SaveNLoadTxt.Load();
            if (data.enoughIngredients) {
                return;
            }

            if (collectingMode == COLLECTING_MODE.ONE_AT_A_TIME) {
                string ing = "";
                if (data.eggQuantity < 2) {
                    data.eggQuantity = 2;
                    ing = "Egg";
                } else if (data.flourQuantity < 2) {
                    data.flourQuantity = 2;
                    ing = "Flour";
                } else if (data.sugarQuantity < 2) {
                    data.sugarQuantity = 2;
                    ing = "Sugar";
                } else if (data.saltQuantity < 1) {
                    data.saltQuantity = 1;
                    ing = "Salt";
                } else if (data.butterQuantity < 1) {
                    data.butterQuantity = 1;
                    ing = "Butter";
                } else if (data.milkQuantity < 1) {
                    data.milkQuantity = 1;
                    ing = "Milk";

                    // Here we know that the player has collected all the ingredients
                    data.enoughIngredients = true;
                }

                Animator[] anims = ingredients.GetComponentsInChildren<Animator>();
                foreach (Animator anim in anims) {
                    if (anim.name == ing) {
                        anim.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                        anim.Play("FlyingIngredient");
                    }
                }
            } else if (collectingMode == COLLECTING_MODE.ALL_AT_ONCE) {
                StartCoroutine(ShootAnimation());
                data.eggQuantity = 2;
                data.flourQuantity = 2;
                data.sugarQuantity = 2;
                data.saltQuantity = 1;
                data.butterQuantity = 1;
                data.milkQuantity = 1;
            }

            SaveNLoadTxt.Save(data);

        } catch (IOException) {
            StartCoroutine(ShootAnimation());
        }
    }

    // This is executed when the shoot animation finishes
    public void OnShootAnimationEnd() {
        data = SaveNLoadTxt.Load();
        if (!data.enoughIngredients) {
            GetComponent<Animator>().Play("ws_farmIdle");
        }
    }

    // Coroutine that launches each ingredient after timeBetweenShots seconds
    private IEnumerator ShootAnimation() {
        Animator[] anims = ingredients.GetComponentsInChildren<Animator>();
        float startTime = Time.time;
        int index = 0;

        while (index < anims.Length) {

            if (Time.time - startTime > timeBetweenShots) {
                startTime = Time.time;
                anims[index].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                anims[index].Play("FlyingIngredient");
                index++;
            } else {
                yield return false;
            }
        }
    }


    void IngredientComesOut(string items) {
        switch (items) {
            case "egg":
                eggsimage = Instantiate(eggs).GetComponent<Image>();
                eggsimage.transform.SetParent(canvas.transform);
                eggsimage.transform.position = child.position;
                break;
            case "flour":
                flourimage = Instantiate(flour).GetComponent<Image>();
                flourimage.transform.SetParent(canvas.transform);
                flourimage.transform.position = child.position;
                break;
            case "salt":
                saltimage = Instantiate(salt).GetComponent<Image>();
                saltimage.transform.SetParent(canvas.transform);
                saltimage.transform.position = child.position;
                break;
            case "sugar":
                sugarimage = Instantiate(sugar).GetComponent<Image>();
                sugarimage.transform.SetParent(canvas.transform);
                sugarimage.transform.position = child.position;
                break;
            case "butter":
                butterimage = Instantiate(butter).GetComponent<Image>();
                butterimage.transform.SetParent(canvas.transform);
                butterimage.transform.position = child.position;
                break;
            case "milk":
                milkimage = Instantiate(milk).GetComponent<Image>();
                milkimage.transform.SetParent(canvas.transform);
                milkimage.transform.position = child.position;
                break;
            default:
                break;
        }
    }

    void FadeOut(Image image) {
        if (image) {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.05f);
            if (image.color.a <= 0) {
                Destroy(image.gameObject);
            }
        }
    }
}
