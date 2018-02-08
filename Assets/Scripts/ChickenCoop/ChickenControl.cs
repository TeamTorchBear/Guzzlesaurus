using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenControl : Clickable {

    private ChickenCoop chickenCoop;

    [SerializeField]
    private Transform eggMark;

   
    public bool hasEgg = false;

    [HideInInspector]
    public GameObject chickenObject;

    public override void OnStart() {
        base.OnStart();

        chickenCoop = FindObjectOfType<ChickenCoop>();

    }

    public override void OnClick() {
        base.OnClick();


        // Play animation
        //GetComponent<Animator>().Play("");

        // Drop egg if there is one
        if (hasEgg) {
            chickenObject.GetComponent<Animator>().Play("coop_" + chickenObject.name + "Yes");
            AkSoundEngine.PostEvent("Chicken", gameObject);
            EggCoop egg = GetComponentInChildren<EggCoop>();
            StartCoroutine(Coroutines.AnimatePosition(egg.gameObject, eggMark.position, 20f, () => {
                egg.chickenCoop = chickenCoop;
                egg.GetComponent<BoxCollider2D>().enabled = true;
                egg.PlayLandingAnimation();
            }));
            hasEgg = false;
        } else {
            chickenObject.GetComponent<Animator>().Play("coop_" + chickenObject.name + "No");
            AkSoundEngine.PostEvent("Chicken", gameObject);
        }

    }


    public void SetHasEgg(bool value) {
        hasEgg = value;
        if (!hasEgg) {
            GetComponentInChildren<EggCoop>().enabled = false;
        }
    } 

}
