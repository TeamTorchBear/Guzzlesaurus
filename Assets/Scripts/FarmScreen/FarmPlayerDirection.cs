using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmPlayerDirection : MonoBehaviour {

    [SerializeField]
    private Animator coop;

    [SerializeField]
    private Animator barn;

    [SerializeField]
    private Animator mill;

    [SerializeField]
    private Animator home;

    private void Start() {
        UpdateAnimations();
        //AkSoundEngine.PostEvent("Mill_Loop", gameObject);
    }
    public void OnMillTap() {
        UpdateAnimations();
    }

    private void UpdateAnimations() {
        coop.Play("Static");
        barn.Play("Static");
        mill.Play("Static");
        home.Play("Static");

        Data data = SaveNLoadTxt.Load();

        if (data.eggQuantity == 0) {
            coop.Play("farm_coopIdle");
            AkSoundEngine.PostEvent("TapCoop", gameObject);
        } else if (data.milkQuantity == 0 || data.butterQuantity == 0) {
            barn.Play("farm_barnIdle");
            AkSoundEngine.PostEvent("TapBarn", gameObject);
        } else if (data.flourQuantity == 0) {
            mill.Play("farm_millIdle");
            AkSoundEngine.PostEvent("TapMill", gameObject);
        } else {
            home.Play("farm_signIdle");
        }
    }


}
