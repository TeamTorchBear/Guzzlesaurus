using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour {

	public void changeScene(string sceneName) {
		SceneManager.LoadScene (sceneName);
	}

	public void quitApplication(){
		Application.Quit ();
	}

}
