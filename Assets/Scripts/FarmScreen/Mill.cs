using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mill : Clickable {

    [SerializeField]
    private GameObject flourFrame;

    private FarmPlayerDirection farmPlayerDirection;

    public override void OnStart() {
        base.OnStart();
        farmPlayerDirection = FindObjectOfType<FarmPlayerDirection>();
    }

    public override void OnClick() {
        base.OnClick();

        GetComponent<Animator>().Play("farm_millTap");

        Data data = SaveNLoadTxt.Load();
        data.flourQuantity = Pancake.flour;
        SaveNLoadTxt.Save(data);

        StartCoroutine(Coroutines.AnimateScale(flourFrame, Vector3.one, 10, () => {
            StartCoroutine(Coroutines.ExecuteAfter(() => {
                StartCoroutine(Coroutines.AnimateScale(flourFrame, Vector3.zero, 10, () => {
                    farmPlayerDirection.OnMillTap();
                }));
            }, 2f));

        }));

    }
}
