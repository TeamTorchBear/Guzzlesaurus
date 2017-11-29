using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedEgg : Clickable {

    public GameObject yolkSource;

    public override void OnClick() {
        base.OnClick();

        // When the cracked egg is clicked, play the animation
        GetComponent<Animator>().Play("Animation");
        AkSoundEngine.PostEvent("Egg_Crack", gameObject);

        yolkSource.SetActive(true);
        yolkSource.GetComponent<ParticleGenerator>().Reset();
    }

    // This event is fired when the animation is done
    public void OnEndAnimationEvent() {
        FindObjectOfType<MixWetIngredientsMinigame>().SeparatingEggCompleted();
    }
}
