using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KitchenButton : MonoBehaviour
{

    //public JarBtnClick jbc;
    public Image Screen;
    Data data;

    bool isClick;
    // Use this for initialization
    void Start()
    {
        isClick = false;
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
        data = SaveNLoadTxt.Load();
    }

    // Update is called once per frame
    void Update()
    {
        if (isClick)
        {
            data.butterQuantity = 0;
            data.eggQuantity = 0;
            data.saltQuantity = 0;
            data.sugarQuantity = 0;
            data.milkQuantity = 0;
            data.flourQuantity = 0;
            data.enoughIngredients = false;
            SaveNLoadTxt.Save(data);
            ScreenFadeOut("MixingDryIngredients");
        }

    }

    void OnClick()
    {
        isClick = true;
    }

    void ScreenFadeOut(string scene)
    {
        Screen.gameObject.SetActive(true);
        if (Screen.color.a <= 1)
        {
            Screen.color = new Color(Screen.color.r, Screen.color.g, Screen.color.b, Screen.color.a + 0.02f);

        }
        else
        {
            isClick = false;
            //Screen.gameObject.SetActive(false);
            SceneManager.LoadScene(scene);
        }
    }
}
