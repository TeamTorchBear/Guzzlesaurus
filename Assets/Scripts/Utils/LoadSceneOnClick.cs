using UnityEngine;

public class LoadSceneOnClick : Clickable {

    [SerializeField]
    private string sceneName;

    [SerializeField]
    private string animationName;

    private SceneLoader sceneLoader;
    private Animator animator;

    public override void OnStart() {
        sceneLoader = FindObjectOfType<SceneLoader>();
        animator = GetComponent<Animator>();
    }

    public override void OnClick() {
        if(animator != null && animationName != "") {
            animator.Play(animationName);
        }
        sceneLoader.LoadScene(sceneName);
    }
}
