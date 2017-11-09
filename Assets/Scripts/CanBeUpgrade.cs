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
            case "Table":
                moneyToPay = 60;
                break;
        }
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
	
	// Update is called once per frame
	void Update () {
        if (isClick)
        {
            isClick = false;
        }
        
	}
    
    public void Upgrade()
    {
        data.moneyWeHave-=moneyToPay;
        SaveNLoadTxt.Save(data);
        if (data.moneyWeHave >= moneyToPay)
        {
            moneyEnough = true;
        }
        else
        {
            moneyEnough = false;
        }
        switch (this.name)
        {
            case "Kitchen":
                data.kitchenLevel = 2;
                SaveNLoadTxt.Save(data);
                break;
            case "Table":
                data.tableLevel = 2;
                SaveNLoadTxt.Save(data);
                break;
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

