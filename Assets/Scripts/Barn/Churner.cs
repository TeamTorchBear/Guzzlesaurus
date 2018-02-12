using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Churner : Clickable {

    [SerializeField]
    private Transform positionMark;
    private Vector3 initial;

    [SerializeField]
    private GameObject butterObject;

    [SerializeField]
    private GameObject butterFrame;

    [SerializeField]
    private GameObject exitButton;

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
        Data data = SaveNLoadTxt.Load();
        data.butterQuantity = Pancake.butter;
        SaveNLoadTxt.Save(data);

        StartCoroutine(Coroutines.AnimateScale(butterFrame, Vector3.one, 10, () => {
            StartCoroutine(Coroutines.ExecuteAfter(() => {
                StartCoroutine(Coroutines.AnimateScale(butterFrame, Vector3.zero, 10, () => {
                    exitButton.SetActive(true);
                }));
            }, 2f));

        }));
    }

}
