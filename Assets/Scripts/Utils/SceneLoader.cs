using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class SceneLoader : MonoBehaviour {

    [SerializeField]
    private bool transitions = true;

    [SerializeField]
    private float transitionTime = 1f;

    [SerializeField]
    private SpriteRenderer image;

    public void LoadScene(string sceneName) {
        if (transitions) {
            StartCoroutine(TransitionCoroutine(() => { SceneManager.LoadScene(sceneName); }));
        } else {
            SceneManager.LoadScene(sceneName);
        }
    }

    
    private IEnumerator TransitionCoroutine(Action function = null) {
        float startTime = Time.time;
        image.enabled = true;
        image.color = new Color(0f, 0f, 0f, 0f);

        while (Time.time - startTime < transitionTime) {
            image.color = new Color(0f, 0f, 0f, Time.time - startTime / transitionTime);
            yield return false;
        }
        image.color = new Color(0f, 0f, 0f, 1f);

        if (function != null) {
            function();
        }
    }
}
