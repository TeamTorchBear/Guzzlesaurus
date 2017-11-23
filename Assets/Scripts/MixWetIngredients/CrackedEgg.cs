using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedEgg : Clickable {



    public override void OnClick() {
        base.OnClick();

        GetComponent<Animator>().Play("Animation");
        AkSoundEngine.PostEvent("Egg_Crack", gameObject);


    }

    public void OnEndAnimationEvent() {
        FindObjectOfType<MixWetIngredientsMinigame>().SeparatingEggCompleted();
    }
}
