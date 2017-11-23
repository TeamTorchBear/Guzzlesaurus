using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourControl : MonoBehaviour
{

    public float pourTime;
    public GameObject pancakePrefabs;
    public GameObject pancake;
    // Use this for initialization
    void Start()
    {
        pourTime = 0;
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
                if (Input.GetKey(KeyCode.Space))
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

    }

    void StartPour()
    {
        Debug.Log("Start Pouring");
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
        }
    }

    void StopPour()
    {
        Debug.Log("Stop Pouring");
        /*Pause Animation
       *
       * PAUSE ANIMATION
       * 
       * 
       */
    }
}
