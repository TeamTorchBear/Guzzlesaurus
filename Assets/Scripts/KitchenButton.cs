using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KitchenButton : MonoBehaviour {

    public JarBtnClick jbc;
    public Image Screen,item1, item2, item3, item4, item5, item6;

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
            if (!jbc.isFireflies)
            {
                ScreenFadeOut("MixingDryIngredients");
            }
        }
    }

    void OnClick()
    {
        isClick = true;
    }

    void ScreenFadeOut(string scene)
    {
        if (Screen.color.r >= 0)
        {
            Screen.color = new Color(Screen.color.r - 0.02f, Screen.color.g - 0.02f, Screen.color.b - 0.02f, Screen.color.a);
            item1.color = new Color(Screen.color.r - 0.02f, Screen.color.g - 0.02f, Screen.color.b - 0.02f, Screen.color.a);
            item2.color = new Color(Screen.color.r - 0.02f, Screen.color.g - 0.02f, Screen.color.b - 0.02f, Screen.color.a);
            item3.color = new Color(Screen.color.r - 0.02f, Screen.color.g - 0.02f, Screen.color.b - 0.02f, Screen.color.a);
            item4.color = new Color(Screen.color.r - 0.02f, Screen.color.g - 0.02f, Screen.color.b - 0.02f, Screen.color.a);
            item5.color = new Color(Screen.color.r - 0.02f, Screen.color.g - 0.02f, Screen.color.b - 0.02f, Screen.color.a);
            item6.color = new Color(Screen.color.r - 0.02f, Screen.color.g - 0.02f, Screen.color.b - 0.02f, Screen.color.a);
            this.GetComponent<Image>().color = new Color(Screen.color.r - 0.02f, Screen.color.g - 0.02f, Screen.color.b - 0.02f, Screen.color.a);
        }
        else
        {
            isClick = false;
            SceneManager.LoadScene(scene);
        }
    }
}
