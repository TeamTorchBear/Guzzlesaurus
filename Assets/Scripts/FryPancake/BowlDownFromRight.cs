using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlDownFromRight : MonoBehaviour {
    
    public Transform startMarker;
    public Transform endMarker;
    public float speed = 10F;

    void Start() {
        StartCoroutine(BowlDownFromRightAnimation());
    }

    IEnumerator BowlDownFromRightAnimation(){
        float journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        float startTime = Time.time;
        float distCovered, fracJourney = 0;

        while(fracJourney < 1) {
            distCovered = (Time.time - startTime) * speed;
            fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
            yield return false;
        }

        transform.position = new Vector3 (transform.position.x, endMarker.position.y, transform.position.z);
        GetComponent<BowlDownFromRight>().enabled = false;
        GetComponent<PourControl>().enabled = true;
    }
}
