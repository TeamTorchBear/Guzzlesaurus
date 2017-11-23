using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanShake : MonoBehaviour
{
    private float z = 0;
    private bool zPlus = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (zPlus)
            z += 50 * Time.deltaTime;
        else
            z -= 50 * Time.deltaTime;

        if (z >= 5f)
            zPlus = false;
        else if (z <= -5f)
            zPlus = true;
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, z);
    }
}
