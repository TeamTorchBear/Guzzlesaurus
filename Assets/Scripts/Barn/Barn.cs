using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barn : MonoBehaviour {

    private enum State {
        GetMilkFirst,
        Pasteurizer,
        GetMilkSecond
    }

    private State state = State.GetMilkFirst;

    // Scene elements
    private Cow cow;
    private Pasteurizer pasteurizer;
    private Pail pail;
    private Churner churner;
    private Milk milk;

    private void Start() {
        cow = FindObjectOfType<Cow>();
        pasteurizer = FindObjectOfType<Pasteurizer>();
        pail = FindObjectOfType<Pail>();
        churner = FindObjectOfType<Churner>();
        milk = FindObjectOfType<Milk>();
    }

    
    public void SetPailFill (bool value) {
        pail.SetFill(value);

        switch (state) {
            case State.GetMilkFirst:
                pasteurizer.Show();
                ++state;
                break;
            case State.GetMilkSecond:
                break;
        }
    }

    public void OnMilkTap() {
        ++state;

        // The player has to tap again the cow for milk
        cow.SetColliderActive(true);

        // Play here the guiding vocals!
        // ...

    }



}
