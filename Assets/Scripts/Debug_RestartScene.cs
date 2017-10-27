using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Debug_RestartScene : Clickable {

    public override void OnClick() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
