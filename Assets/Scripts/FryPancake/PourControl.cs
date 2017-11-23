using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourControl : MonoBehaviour
{

    public float pourTime;
    public GameObject pancakePrefabs;
    public GameObject pancake;
    public Sprite bowl1, bowl2;
    // Use this for initialization
    void Start()
    {
        pourTime = 0;
        this.GetComponent<SpriteRenderer>().sprite = bowl1;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began ||
                Input.GetTouch(0).phase == TouchPhase.Stationary ||
                Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                StartPour();
                pourTime += Time.deltaTime;
            }
            else
            {
                StopPour();
            }
        }
    }

    void StartPour()
    {
        Debug.Log("Start Pouring");
        this.GetComponent<SpriteRenderer>().sprite = bowl2;
        /*Play Animation
        *
        * PLAY ANIMATION
        * 
        * 
        */
        if (pourTime >= 5)
        {
            //stop animation
            this.GetComponent<BowlLeave>().enabled = true;
            pancake = Instantiate(pancakePrefabs);
            pancake.transform.SetParent(FindObjectOfType<PanDownFromTop>().transform);
            this.GetComponent<PourControl>().enabled = false;
            this.GetComponent<SpriteRenderer>().sprite = bowl1;
        }
    }

    void StopPour()
    {

        this.GetComponent<SpriteRenderer>().sprite = bowl1;
        Debug.Log("Stop Pouring");
        /*Pause Animation
       *
       * PAUSE ANIMATION
       * 
       * 
       */
    }
}
