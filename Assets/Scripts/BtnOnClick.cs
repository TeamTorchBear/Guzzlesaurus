using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BtnOnClick : MonoBehaviour
{
    public Image Screen, Screen1, Screen2, Screen3, Screen4;
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
                    ScreenFadeOut("MailBoxScreen");
                    break;
                case "Kitchen":
                    if (Screen1.color.r >= 0)
                    {
                        Screen1.color = new Color(Screen1.color.r - 0.02f, Screen1.color.g - 0.02f, Screen1.color.b - 0.02f, Screen1.color.a);
                    }
                    if (Screen2.color.r >= 0)
                    {
                        Screen2.color = new Color(Screen2.color.r - 0.02f, Screen2.color.g - 0.02f, Screen2.color.b - 0.02f, Screen2.color.a);
                    }
                    ScreenFadeOut("KitchenScreen");
                    break;
                case "PosterExit":
                    if (Screen1.color.r >= 0)
                    {
                        Screen1.color = new Color(Screen1.color.r - 0.02f, Screen1.color.g - 0.02f, Screen1.color.b - 0.02f, Screen1.color.a);
                    }
                    ScreenFadeOut("GuzzWorldScreen");
                    break;
                case "Mini1":
                    if (Screen1.color.r >= 0)
                    {
                        Screen1.color = new Color(Screen1.color.r - 0.02f, Screen1.color.g - 0.02f, Screen1.color.b - 0.02f, Screen1.color.a);
                    }
                    ScreenFadeOut("MixingDryIngredients");
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
