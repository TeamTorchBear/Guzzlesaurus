using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pasteurizer : Clickable {

    [SerializeField]
    private Transform positionMark;

    [SerializeField]
    private GameObject milkObject;

    public void Show() {
        StartCoroutine(Coroutines.AnimatePosition(gameObject, positionMark.position, 20f));
    }

    public void MilkDragged() {
        GetComponent<Animator>().Play("barn_pasturiserWorking");
    }

    public void OnWorkingAnimationEnd() {
        milkObject.SetActive(true);
    }
    	
}
