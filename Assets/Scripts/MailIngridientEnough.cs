using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailIngridientEnough : MonoBehaviour {
    public Image egg, flour, sugar, salt, milk, butter;
    Data data;
    // Use this for initialization
    void Start () {
        data = SaveNLoadTxt.Load();
        if (data.eggQuantity == 2)
        {
            egg.gameObject.SetActive(true);
        }
        if (data.flourQuantity == 2)
        {
            flour.gameObject.SetActive(true);
        }
        if (data.sugarQuantity == 2)
        {
            sugar.gameObject.SetActive(true);
        }
        if (data.butterQuantity == 1)
        {
            butter.gameObject.SetActive(true);
        }
        if (data.milkQuantity == 1)
        {
            milk.gameObject.SetActive(true);
        }
        if (data.saltQuantity == 1)
        {
            salt.gameObject.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
