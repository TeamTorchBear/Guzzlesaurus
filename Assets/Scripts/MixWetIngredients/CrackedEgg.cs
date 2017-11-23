using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedEgg : Clickable {



    public override void OnClick() {
        base.OnClick();

        // When the cracked egg is clicked, play the animation
        GetComponent<Animator>().Play("Animation");
    }

    // This event is fired when the animation is done
    public void OnEndAnimationEvent() {
        FindObjectOfType<MixWetIngredientsMinigame>().SeparatingEggCompleted();
    }
}
