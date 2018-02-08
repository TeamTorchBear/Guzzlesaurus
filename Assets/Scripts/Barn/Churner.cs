using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Churner : Clickable {

    [SerializeField]
    private Transform positionMark;
    private Vector3 initial;

    [SerializeField]
    private GameObject butterObject;

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
        GetComponent<Animator>().Play("barn_butterChurnerWorking");
    }

    public void OnWorkingAnimationEnd() {
        Debug.Log("Done");
    }

}
