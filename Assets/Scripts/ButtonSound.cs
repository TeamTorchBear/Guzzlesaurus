using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{

    public void onclick()
    {
        AkSoundEngine.PostEvent("Signpost", gameObject);
    }
}
