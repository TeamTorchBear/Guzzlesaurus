using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedEgg : Clickable {



    public override void OnClick() {
        base.OnClick();

        GetComponent<Animator>().Play("Animation");


    }

    public void OnEndAnimationEvent() {
        FindObjectOfType<MixWetIngredientsMinigame>().SeparatingEggCompleted();
    }
}
