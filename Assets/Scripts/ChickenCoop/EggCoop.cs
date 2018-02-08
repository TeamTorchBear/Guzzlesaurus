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

        chickenCoop.OnEggTap(this);
    }
}
