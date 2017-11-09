using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultNextBtn : MonoBehaviour {
    public GameObject Background;
    public Image screen;
    bool isClick;

	// Use this for initialization
	void Start () {
        isClick = false;
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
	}
	
	// Update is called once per frame
	void Update () {
        if (isClick)
        {
            switch (this.name)
            {
                case "SNext":
                    break;
                case "CNext":
                    break;
                case "XNext":
                    break;
                default:
                    isClick = false;
                    break;
            }
        }
	}

    void OnClick()
    {
        isClick = true;
    }
}
