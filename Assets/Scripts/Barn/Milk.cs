using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milk : Clickable {

    private Barn barn;

    public override void OnStart() {
        base.OnStart();
        AkSoundEngine.PostEvent("TapCow", gameObject);
        barn = FindObjectOfType<Barn>();
    }

    public override void OnClick() {
        base.OnClick();
        AkSoundEngine.PostEvent("Milk_Pickup", gameObject);
        barn.OnMilkTap();
        // Play animation??

        Data data = SaveNLoadTxt.Load();

        data.milkQuantity = Mathf.Min(data.milkQuantity + 1, Pancake.milk);

        SaveNLoadTxt.Save(data);


        Destroy(gameObject);
    }

}
