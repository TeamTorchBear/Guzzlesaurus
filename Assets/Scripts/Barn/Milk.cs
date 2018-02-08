using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milk : Clickable {

    private Barn barn;

    public override void OnStart() {
        base.OnStart();

        barn = FindObjectOfType<Barn>();
    }

    public override void OnClick() {
        base.OnClick();

        barn.OnMilkTap();
        // Play animation??

        Data data = SaveNLoadTxt.Load();

        data.milkQuantity = Mathf.Min(data.milkQuantity + 1, Pancake.milk);

        SaveNLoadTxt.Save(data);


        Destroy(gameObject);
    }

}
