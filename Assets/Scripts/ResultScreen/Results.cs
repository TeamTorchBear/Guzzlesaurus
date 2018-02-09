using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Results : MonoBehaviour {


    [SerializeField]
    private GameObject[] stars;
    [SerializeField]
    private SwipeScreen swipeControl;
    [SerializeField]
    private GameObject coin;

    private int starIndex = 0;
    private int score;

    private void Start() {
        Data data = SaveNLoadTxt.Load();
        score = Mathf.CeilToInt(data.score / 26f);
    }

    public void PopStar() {
        if (score > starIndex && starIndex < stars.Length) {
            stars[starIndex].GetComponent<Animator>().Play("PopStar");
            starIndex++;
        } else {
            swipeControl.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    public void ShowCoin() {
        //coin.SetActive(true);
        foreach (Animator anim in coin.GetComponentsInChildren<Animator>()) {
            anim.Play("Play");
        }
    }

}
