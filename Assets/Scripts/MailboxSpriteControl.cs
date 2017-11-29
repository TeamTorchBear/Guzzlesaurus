using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailboxSpriteControl : MonoBehaviour {

    public GameObject open, close;
	void Start () {
        Data data = SaveNLoadTxt.Load();
        if (data.unreadMail) {
            AkSoundEngine.PostEvent("ThereIsPost", gameObject);
            close.SetActive(false);
            open.SetActive(true);

        } 
      
	}
	
}
