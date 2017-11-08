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
                    //ScreenFadeOut("MixingDryIngredients");
                    break;
                case "Farm":
                    //ScreenFadeOut("FarmScreen");
                    break;
                case "Cave":
                    ScreenFadeOut("CaveScreen");
                    break;
                case "Mailbox":
                    ScreenFadeOut("MailBoxScreen");
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
                case "Cloud1":
                    if (this.GetComponent<Image>().color.a >= 0)
                    {
                        Screen3.gameObject.SetActive(true);
                        Screen3.color = new Color(Screen3.color.r, Screen3.color.g, Screen3.color.b, Screen3.color.a + 0.02f);
                        this.GetComponent<Image>().color = new Color(this.GetComponent<Image>().color.r, this.GetComponent<Image>().color.g, this.GetComponent<Image>().color.b, this.GetComponent<Image>().color.a - 0.02f);
                        Screen2.color= new Color(this.GetComponent<Image>().color.r, this.GetComponent<Image>().color.g, this.GetComponent<Image>().color.b, this.GetComponent<Image>().color.a - 0.02f);
                    }
                    else
                    {
                        this.gameObject.SetActive(false);
                        Screen2.gameObject.SetActive(false);
                        isClick = false;
                    }
                    
                    break;
                case "Cloud2":
                    if (this.GetComponent<Image>().color.a >= 0)
                    {
                        Screen3.gameObject.SetActive(true);
                        Screen3.color = new Color(Screen3.color.r, Screen3.color.g, Screen3.color.b, Screen3.color.a + 0.02f);
                        this.GetComponent<Image>().color = new Color(this.GetComponent<Image>().color.r, this.GetComponent<Image>().color.g, this.GetComponent<Image>().color.b, this.GetComponent<Image>().color.a - 0.02f);
                        Screen1.color = new Color(this.GetComponent<Image>().color.r, this.GetComponent<Image>().color.g, this.GetComponent<Image>().color.b, this.GetComponent<Image>().color.a - 0.02f);
                    }
                    else
                    {
                        this.gameObject.SetActive(false);
                        Screen1.gameObject.SetActive(false);
                        isClick = false;
                    }
                    break;
                case "Cloud3":
                    if (this.GetComponent<Image>().color.a >= 0)
                    {
                        Screen1.gameObject.SetActive(true);
                        Screen2.gameObject.SetActive(true);
                        this.GetComponent<Image>().color = new Color(this.GetComponent<Image>().color.r, this.GetComponent<Image>().color.g, this.GetComponent<Image>().color.b, this.GetComponent<Image>().color.a - 0.02f);
                        Screen1.color = new Color(Screen1.color.r, Screen1.color.g, Screen1.color.b, Screen1.color.a + 0.02f);
                        Screen2.color = new Color(Screen2.color.r, Screen2.color.g, Screen2.color.b, Screen2.color.a + 0.02f);
                    }
                    else
                    {
                        this.gameObject.SetActive(false);
                        isClick = false;
                    }
                    break;
                case "Back":
                    ScreenFadeOut("GuzzWorldScreen");
                    break;
                case "ButtonPancake":
                    Screen.gameObject.SetActive(true);
                    break;
                case "BackToCave":
                    ScreenFadeOut("CaveScreen");
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
        Screen.gameObject.SetActive(true);
        if (Screen.color.a <= 1.0f)
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
