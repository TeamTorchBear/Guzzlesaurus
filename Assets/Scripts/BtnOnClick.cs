using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BtnOnClick : MonoBehaviour
{
    public Image Screen;
    bool isClick;
    // Use this for initialization
    void Start()
    {
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
        isClick = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isClick)
        {
            switch (this.name)
            {
                case "StartBtn":
                    ScreenFadeOut("GuzzWorldScreen");
                    break;
                case "Farm":
                    ScreenFadeOut("FarmScreen");
                    break;
                case "Cave":
                    ScreenFadeOut("CaveScreen");
                    break;
                case "Mailbox":
                    ScreenFadeOut("MailboxScreen");
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

    void ScreenFadeOut(string scene)
    {
        if (Screen.color.r >= 0)
        {
            Screen.color = new Color(Screen.color.r - 0.02f, Screen.color.g - 0.02f, Screen.color.b - 0.02f, Screen.color.a);
        }
        else
        {
            isClick = false;
            SceneManager.LoadScene(scene);
        }
    }
}
