using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudWoosh : MonoBehaviour {

    public void onclick()
    {
        AkSoundEngine.PostEvent("PlayWind", gameObject);
    }
}

