using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Coroutines {

    public static IEnumerator AnimatePosition(GameObject objToMove, Vector3 finalPos, float speed, Action function = null) {
        float startTime = Time.time;
        Vector3 initialPos = objToMove.transform.position;
        float distance = Vector3.Distance(initialPos, finalPos);
        float distCovered = 0, fracJourney = 0;
        if (distance > 0) {
            while (fracJourney < 1) {
                distCovered = (Time.time - startTime) * speed;
                fracJourney = distCovered / distance;
                objToMove.transform.position = Vector3.Lerp(initialPos, finalPos, fracJourney);
                yield return false;
            }
            objToMove.transform.position = finalPos;
        }
        if (function != null) {
            function();
        }
    }

    public static IEnumerator AnimateScale(GameObject objToScale, Vector3 finalScale, float speed, Action function = null) {
        float startTime = Time.time;
        Vector3 initialScale = objToScale.transform.localScale;
        float distance = Vector3.Distance(initialScale, finalScale);
        float distCovered = 0, fracJourney = 0;
        if (distance > 0) {
            while (fracJourney < 1) {
                distCovered = (Time.time - startTime) * speed;
                fracJourney = distCovered / distance;
                objToScale.transform.localScale = Vector3.Lerp(initialScale, finalScale, fracJourney);
                yield return false;
            }
            objToScale.transform.localScale = finalScale;
        }
        if (function != null) {
            function();
        }
    }

    public static IEnumerator ExecuteAfter(Action function, float seconds) {
        yield return new WaitForSeconds(seconds);
        function();
    }
}
