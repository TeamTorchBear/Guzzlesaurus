using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Results : MonoBehaviour {


    [SerializeField]
    private GameObject[] stars;
    [SerializeField]
    private TapScreenControl tapScreenControl;
    [SerializeField]
    private GameObject coin;

    private int starIndex = 0;
    private int score;

    private bool tapOnce;

    private void Start() {
        Data data = SaveNLoadTxt.Load();
        score = Mathf.CeilToInt(data.score / 26f);
    }

    public void PopStar() {
        if (score > starIndex && starIndex < stars.Length) {
            stars[starIndex].GetComponent<Animator>().Play("PopStar");
            starIndex++;
        } else {
            tapScreenControl.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    public void ShowCoin() {
        coin.SetActive(true);
        foreach (Animator anim in coin.GetComponentsInChildren<Animator>()) {
            anim.Play("Play");
        }
    }

    public void ScreenTap() {
        if (!tapOnce) {
            GetComponent<Animator>().enabled = false;
            tapScreenControl.GetComponent<Collider2D>().enabled = false;
            StartCoroutine(Coroutines.AnimateScale(gameObject, Vector3.zero, 10, () => {
                ShowCoin();
                tapScreenControl.GetComponent<Collider2D>().enabled = true;
            }));
            foreach (GameObject star in stars) {
                StartCoroutine(Coroutines.AnimateScale(star, Vector3.zero, 10));
            }
            

            tapOnce = true;

        } else {
            FindObjectOfType<SceneLoader>().LoadScene("StartScreen");
        }
    }

}
