using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour {

    public Minigame minigame;
    public SpriteRenderer screen;

    private void Start() {
        if (minigame != null) {
            minigame.StartMinigame();
        }
    }

    public void ScreenFadeOut(string scene) {
        StartCoroutine(FadeOut(scene));
    }

    private IEnumerator FadeOut(string scene) {
        screen.gameObject.SetActive(true);
        while (screen.color.a <= 1) {
            screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, screen.color.a + 0.02f);
            yield return false;
        }
        SceneManager.LoadScene(scene);
    }

}
