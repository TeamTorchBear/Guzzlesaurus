using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneLoader sceneLoader = GetComponent<SceneLoader>();
            if (sceneLoader != null) {
                sceneLoader.LoadScene("Scenes/StartScreen");
            } else {
                SceneManager.LoadScene("Scenes/StartScreen");
            }
        }
    }
}
