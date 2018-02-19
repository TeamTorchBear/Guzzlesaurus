using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pasteurizer : Clickable {

    [SerializeField]
    private Transform positionMark;

    private Vector3 initial;

    [SerializeField]
    private GameObject milkObject;

    public override void OnStart() {
        base.OnStart();

        initial = transform.position;
    }

    public void Show() {
        StartCoroutine(Coroutines.AnimatePosition(gameObject, positionMark.position, 20f));
    }

    public void Hide() {
        StartCoroutine(Coroutines.AnimatePosition(gameObject, initial, 20f));
    }

    public void MilkDragged() {
        GetComponent<Animator>().Play("barn_pasturiserWorking");
        AkSoundEngine.PostEvent("PasturisingMachine", gameObject);
    }

    public void OnWorkingAnimationEnd() {
        milkObject.SetActive(true);
    }
    	
}
