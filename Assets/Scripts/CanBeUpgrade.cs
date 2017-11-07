using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanBeUpgrade : MonoBehaviour {
    
    public JarBtnClick jbc;
    public bool moneyEnough;

    public static Data data;
    int moneyToPay;
    bool isClick;

    // Use this for initialization
    void Start () {
        moneyEnough = false;
        isClick = false;
        data = SaveNLoadTxt.Load();


        Button btn = jbc.GetComponent<Button>();
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

        if (data.moneyWeHave >= moneyToPay)
        {
            moneyEnough = true;
        }
        else
        {
            moneyEnough = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (isClick)
        {
            isClick = false;
        }
        
	}
    
    public void Upgrade()
    { data.moneyWeHave-=moneyToPay;
        SaveNLoadTxt.Save(data);
        if (data.moneyWeHave >= moneyToPay)
        {
            moneyEnough = true;
        }
        else
        {
            moneyEnough = false;
        }
    }

    void OnClick()
    {
        isClick = true;
    }

    public void updates()
    {
        if (data.moneyWeHave >= moneyToPay)
        {
            moneyEnough = true;
        }
        else
        {
            moneyEnough = false;
        }
    }
}

