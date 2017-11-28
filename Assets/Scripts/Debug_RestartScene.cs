using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DebugAction {
    Restart,
    Advance,
    Reset
}

public class Debug_RestartScene : Clickable {

    public DebugAction action;

    public override void OnClick() {
        if (action == DebugAction.Restart) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } else if (action == DebugAction.Advance) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } else if (action == DebugAction.Reset){
            Data data = new Data {
                moneyWeHave = 0,
                eggQuantity = 0,
                flourQuantity = 0,
                milkQuantity = 0,
                sugarQuantity = 0,
                saltQuantity = 0,
                butterQuantity = 0,
                tableLevel = 1,
                kitchenLevel = 1,
                unread = true
            };
            SaveNLoadTxt.Save(data);
        }
    }
}
