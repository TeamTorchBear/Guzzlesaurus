using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
			SceneManager.LoadScene ("Scenes/MainMenu");
		}
	}
}
