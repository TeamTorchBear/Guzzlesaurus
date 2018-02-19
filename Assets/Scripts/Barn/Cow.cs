using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow : Clickable {

    private Barn barn;

    private Collider2D cowCollider;

    public override void OnStart() {
        base.OnStart();

        barn = FindObjectOfType<Barn>();
        cowCollider = GetComponent<Collider2D>();
        AkSoundEngine.PostEvent("TapCow", gameObject);

    }

    public override void OnClick() {
        base.OnClick();
        AkSoundEngine.PostEvent("CowMoo", gameObject);
        GetComponent<Animator>().Play("barn_cowMilk");
        barn.SetPailFill(true);
        SetColliderActive(false);
    }


    public void SetColliderActive(bool value) {
       cowCollider.enabled = value;
    }
}
