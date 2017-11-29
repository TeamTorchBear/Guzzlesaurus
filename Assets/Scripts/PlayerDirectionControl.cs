using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirectionControl : MonoBehaviour {

    private enum IDLE {
        NONE,
        MB_IDLE,
        FARM_IDLE,
        CAVE_IDLE
    }

    public GameObject farm, cave, mailbox, mailboxOpen;
    private IDLE state = IDLE.NONE;

    void Start() {
        Data data = SaveNLoadTxt.Load();
        if (data.unreadMail) {
            // Disable farm and cave if there is unread mail
            farm.GetComponent<Collider2D>().enabled = false;
            cave.GetComponent<Collider2D>().enabled = false;

            mailbox.GetComponent<Collider2D>().enabled = true;
            if (state != IDLE.MB_IDLE) {
                Debug.Log("Play mailbox Idle");
                state = IDLE.MB_IDLE;
                farm.GetComponent<Animator>().Play("Static");
                cave.GetComponent<Animator>().Play("Static");
                mailboxOpen.GetComponent<Animator>().Play("ws_mbOpenIdle");
            }

        } else if (!data.enoughIngredients) {
            // There is no unread mail and not enough ingredients collected
            farm.GetComponent<Collider2D>().enabled = true;
            if (state != IDLE.FARM_IDLE) {
                Debug.Log("Play farm Idle");
                state = IDLE.FARM_IDLE;
                farm.GetComponent<Animator>().Play("ws_farmIdle");
                mailbox.GetComponentInChildren<Animator>().Play("Static");
                cave.GetComponent<Animator>().Play("Static");
            }
            cave.GetComponent<Collider2D>().enabled = false;

            mailbox.GetComponent<Collider2D>().enabled = false;
        } 
    }

    void Update() {
        Data data = SaveNLoadTxt.Load();
        if(data.enoughIngredients) {
            // Everything done
            farm.GetComponent<Collider2D>().enabled = true;
            cave.GetComponent<Collider2D>().enabled = true;
            if (state != IDLE.CAVE_IDLE) {
                Debug.Log("Play cave Idle");
                state = IDLE.CAVE_IDLE;
                cave.GetComponent<Animator>().Play("ws_caveIdle");
                farm.GetComponent<Animator>().Play("Static");
                mailbox.GetComponentInChildren<Animator>().Play("Static");
            }
            mailbox.GetComponent<Collider2D>().enabled = false;
        }
    }
}
