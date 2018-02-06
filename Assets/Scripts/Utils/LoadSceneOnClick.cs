using UnityEngine;

public class LoadSceneOnClick : Clickable {

    [SerializeField]
    private string sceneName;

    private SceneLoader sceneLoader;

    public override void OnStart() {
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    public override void OnClick() {
        sceneLoader.LoadScene(sceneName);
    }


}
