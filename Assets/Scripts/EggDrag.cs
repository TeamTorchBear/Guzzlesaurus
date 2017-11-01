using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggDrag : Draggable {

    public MixWetIngredientsMinigame minigame;



    public override void OnDragStart() {
        minigame.StartDraggingEgg();
    }

    public override void OnDragHold() {
    }

    public override void OnDragEnd() {
        minigame.EndDraggingEgg();
    }

}
