using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour {

    public Minigame minigame;

    private void Start() {
		minigame.StartMinigame();
    }

    public void ScreenFadeOut(string scene) {
        StartCoroutine(FadeOut(scene));
    }

    private IEnumerator FadeOut(string scene) {
        SpriteRenderer screen = GameObject.FindGameObjectWithTag("BlackScreen").GetComponent<SpriteRenderer>();
        screen.gameObject.SetActive(true);
        while (screen.color.a <= 1) {
            screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, screen.color.a + 0.02f);
            yield return false;
        }
        SceneManager.LoadScene(scene);
    }

}
