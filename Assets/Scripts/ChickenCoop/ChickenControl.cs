using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenControl : Clickable {

    private ChickenCoop chickenCoop;

    [SerializeField]
    private Transform eggMark;

    [HideInInspector]
    public bool hasEgg = false;

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
            EggCoop egg = GetComponentInChildren<EggCoop>();
            StartCoroutine(Coroutines.AnimatePosition(egg.gameObject, eggMark.position, 20f, () => {
                egg.chickenCoop = chickenCoop;
                egg.GetComponent<BoxCollider2D>().enabled = true;
            }));
            hasEgg = false;
        }

    }


    public void SetHasEgg(bool value) {
        hasEgg = value;
        if (!hasEgg) {
            GetComponent<EggCoop>().enabled = false;
        }
    } 

}
