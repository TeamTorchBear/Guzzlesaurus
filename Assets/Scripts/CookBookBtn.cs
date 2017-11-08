using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CookBookBtn : MonoBehaviour {

    public JarBtnClick jbc;
    public Image Screen;

    bool isClick;
    // Use this for initialization
    void Start()
    {
        isClick = false;
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (isClick)
        {
            if (!jbc.isFireflies)
            {
                ScreenFadeOut("CookBookScene");
            }
            isClick = false;
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
