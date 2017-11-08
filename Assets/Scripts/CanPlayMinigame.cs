using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanPlayMinigame : MonoBehaviour {
    public Image image;
    Data data;
	// Use this for initialization
	void Start () {
        data = SaveNLoadTxt.Load();
	}
	
	// Update is called once per frame
	void Update () {
        if (data.milkQuantity == 1)
        {
            image.gameObject.SetActive(true);
        }
        else
        {
            image.gameObject.SetActive(false);
        }
	}
}
