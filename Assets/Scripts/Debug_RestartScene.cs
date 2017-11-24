using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DebugAction {
    Restart,
    Advance
}

public class Debug_RestartScene : Clickable {

    public DebugAction action;

    public override void OnClick() {
        if (action == DebugAction.Restart) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } else if (action == DebugAction.Advance) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
