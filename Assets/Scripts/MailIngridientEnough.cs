using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailIngridientEnough : MonoBehaviour {
    public SpriteRenderer egg, flour, sugar, salt, milk, butter;
    public Sprite yes;

    Data data;
    // Use this for initialization
    void Start() {
        data = SaveNLoadTxt.Load();
        if (data.eggQuantity == 2) {
            egg.sprite = yes;
        }
        if (data.flourQuantity == 2) {
            flour.sprite = yes;
        }
        if (data.sugarQuantity == 2) {
            sugar.sprite = yes;
        }
        if (data.butterQuantity == 1) {
            butter.sprite = yes;
        }
        if (data.milkQuantity == 1) {
            milk.sprite = yes;
        }
        if (data.saltQuantity == 1) {
            salt.sprite = yes;
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
