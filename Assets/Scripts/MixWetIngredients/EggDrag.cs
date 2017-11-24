using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggDrag : Draggable {

    public MixWetIngredientsMinigame minigame;


    public override void OnDragStart() {
        GetComponent<Collider2D>().enabled = true;
        minigame.StartDraggingEgg();
        AkSoundEngine.PostEvent("Egg_Pickup", gameObject);
    }

    public override void OnDragHold() {
    }

    public override void OnDragEnd() {
        minigame.EndDraggingEgg();
        GetComponent<Collider2D>().enabled = false;
    }

}
