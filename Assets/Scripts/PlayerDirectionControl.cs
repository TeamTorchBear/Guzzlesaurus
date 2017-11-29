using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirectionControl : MonoBehaviour {

    public GameObject farm, cave, mailbox;
	
	void Start () {
        Data data = SaveNLoadTxt.Load();
        if (data.unreadMail) {
            // Disable farm and cave if there is unread mail
            farm.GetComponent<Collider2D>().enabled = false;
            cave.GetComponent<Collider2D>().enabled = false;

            mailbox.GetComponent<Collider2D>().enabled = true;
            mailbox.GetComponentInChildren<Animator>().Play("ws_mbIdle");
        } else if (!data.enoughIngredients) {
            // There is no unread mail and not enough ingredients collected
            farm.GetComponent<Collider2D>().enabled = true;
            farm.GetComponent<Animator>().Play("ws_farmIdle");
            cave.GetComponent<Collider2D>().enabled = false;

            mailbox.GetComponent<Collider2D>().enabled = false;
            AkSoundEngine.PostEvent("GetIngredients", gameObject);
        } else {
            // Everything done
            farm.GetComponent<Collider2D>().enabled = true;
            cave.GetComponent<Collider2D>().enabled = true;
            cave.GetComponent<Animator>().Play("ws_caveIdle");
            mailbox.GetComponent<Collider2D>().enabled = false;
            AkSoundEngine.PostEvent("GoToKitchen", gameObject);
        }
	}
}
