using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectIngridient : MonoBehaviour {

    public GameObject eggs,flour,sugar,salt, milk,butter;
    
    bool isClick;
    int clickTimes=0;
    Data data;
    Image eggsimage, flourimage, sugarimage, saltimage, milkimage, butterimage;

    // Use this for initialization
    void Start () {
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
        isClick = false;
        
	}
	
	// Update is called once per frame
	void Update () {
        data = SaveNLoadTxt.Load();
        if (isClick)
        {
            isClick = false;
            if (data.eggQuantity < 2)
            {
                data.eggQuantity++;
                SaveNLoadTxt.Save(data);
            }
            else if (data.flourQuantity < 2)
            {
                data.flourQuantity++;
                SaveNLoadTxt.Save(data);
            }
            else if (data.sugarQuantity < 2)
            {
                data.sugarQuantity++;
                SaveNLoadTxt.Save(data);
            }
            else if (data.saltQuantity < 1)
            {
                data.saltQuantity++;
                SaveNLoadTxt.Save(data);
            }
            else if (data.butterQuantity < 1)
            {
                data.butterQuantity++;
                SaveNLoadTxt.Save(data);
            }
            else if (data.milkQuantity < 1)
            {
                data.milkQuantity++;
                SaveNLoadTxt.Save(data);
            }
        }
	}

    void OnClick()
    {
        isClick = true;
    }

    void IngredientComesOut(string items)
    {
        switch (items)
        {
            case "egg":
                eggsimage = Instantiate(eggs).GetComponent<Image>();
                break;
            case "flour":
                flourimage = Instantiate(flour).GetComponent<Image>();
                break;
            case "salt":
                saltimage = Instantiate(salt).GetComponent<Image>();
                break;
            case "sugar":
                sugarimage = Instantiate(sugar).GetComponent<Image>();
                break;
            case "butter":
                butterimage = Instantiate(butter).GetComponent<Image>();
                break;
            case "milk":
                milkimage = Instantiate(milk).GetComponent<Image>();
                break;
            default:
                break;
        }
    }
}
