using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerTutAnimation : MonoBehaviour
{
    private bool status;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (status)
            this.transform.localScale = new Vector3(this.transform.localScale.x - Time.deltaTime, this.transform.localScale.y - Time.deltaTime, 1);
        else
            this.transform.localScale = new Vector3(this.transform.localScale.x + Time.deltaTime, this.transform.localScale.y + Time.deltaTime, 1);

        if (this.transform.localScale.x > 1.1f)
            status = true;
        else if (this.transform.localScale.x < 0.9f)
            status = false;
    }
}
