using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour {

	public string sceneName;

	public void changeScene() {
		SceneManager.LoadScene (sceneName);
	}

	public void quitApplication(){
		Application.Quit ();
	}
}
