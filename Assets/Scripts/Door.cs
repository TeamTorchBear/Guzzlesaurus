using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public void onClick()
    
        {
            AkSoundEngine.PostEvent("Door_Sound", gameObject);
        }
    }
