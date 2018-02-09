using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapScreenControl : Clickable {

    private Results results;

    
    public override void OnStart() {
        results = FindObjectOfType<Results>();
    }

    public override void OnClick() {
        results.ScreenTap();
    }
    
}
