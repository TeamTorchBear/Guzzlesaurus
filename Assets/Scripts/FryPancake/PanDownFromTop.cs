using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanDownFromTop : MonoBehaviour
{
    public Transform startMarker;
    public Transform endMarker;
    public float speed = 1.0F;
    private float startTime;
    private float journeyLength;
    // Use this for initialization
    void Start()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
    }

    // Update is called once per frame
    void Update()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        this.transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
        if (this.transform.position.y==endMarker.position.y)
        {
            this.GetComponent<PanDownFromTop>().enabled = false;
        }
    }
}
