using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailboxSpriteControl : MonoBehaviour {

    public Sprite unreadSprite;
	void Start () {
        Data data = SaveNLoadTxt.Load();
        if (data.unread) {
            GetComponentInChildren<SpriteRenderer>().sprite = unreadSprite;
        } 
	}
	
}
