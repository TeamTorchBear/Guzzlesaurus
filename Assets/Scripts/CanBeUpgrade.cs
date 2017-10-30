using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanBeUpgrade : MonoBehaviour {

    public bool moneyEnough, isClick;
    public int moneyToPay;
	// Use this for initialization
	void Start () {
        moneyEnough = false;
        isClick = false;
        Resources.Load<Inventory>("Inventory").canUpgradeQuantity = 0;
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);

        switch (this.name)
        {
            case "Kitchen":
                moneyToPay = 70;
                break;
            case "Item (1)":
                moneyToPay = 60;
                break;
            case "Item (2)":
                moneyToPay = 50;
                break;
            case "Item (3)":
                moneyToPay = 40;
                break;
            case "Item (4)":
                moneyToPay = 30;
                break;
            case "Item (5)":
                moneyToPay = 20;
                break;
            case "Item (6)":
                moneyToPay = 10;
                break;
        }

        if (Resources.Load<Inventory>("Inventory").moneyWeHave >= moneyToPay)
        {
            moneyEnough = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
        
        if (Resources.Load<Inventory>("Inventory").moneyWeHave >= moneyToPay)
        {
            moneyEnough = true;
        }

        if (isClick)
        {
            if (moneyEnough)
            {
                Debug.Log("Upgrade " + this.name);
                //Upgrade();
            }
            isClick = false;
        }
	}

    void OnClick()
    {
        isClick = true;
    }
}

