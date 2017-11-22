using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourControl : MonoBehaviour {

    public float pourTime;
	// Use this for initialization
	void Start () {
        pourTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (!this.GetComponent<BowlDownFromRight>().enabled)
        {
            if (Input.acceleration.x > 30)
            {
                StartPour();
                pourTime += Time.deltaTime;
            }
            else
            {
                StopPour();
            }

            if (Input.GetKey(KeyCode.Space))
            {
                StartPour();
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
    }

    void StopPour()
    {
        Debug.Log("Stop Pouring");
    }
}
