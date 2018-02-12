using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggCoop : Clickable {

    [HideInInspector]
    public ChickenCoop chickenCoop;

    public override void OnStart() {
        base.OnStart();
    }

    public override void OnClick() {
        base.OnClick();
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Animator>().Play("coop_eggGet");
        chickenCoop.OnEggTap(this);
    }

    public void PlayLandingAnimation() {
        GetComponent<Animator>().Play("coop_eggLand");
    }

    public void OnPickUp() {
        Destroy(gameObject);
    }
}
