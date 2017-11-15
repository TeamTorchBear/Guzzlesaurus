using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : MonoBehaviour {

    public void onclick()
    {
        AkSoundEngine.PostEvent("Chicken", gameObject);
    }
}


