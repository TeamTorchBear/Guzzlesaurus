using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudBtn : MonoBehaviour
{
    public Image cloud1, cloud2, cloud3;
    public Button mail, farm, cave;
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
            switch (this.name)
            {
                case "Cloud1":
                    if (this.GetComponent<Image>().color.a >= 0)
                    {
                        SetEnable(false);
                        cloud3.gameObject.SetActive(true);
                        cloud3.color = new Color(cloud3.color.r, cloud3.color.g, cloud3.color.b, cloud3.color.a + 0.02f);
                        this.GetComponent<Image>().color = new Color(this.GetComponent<Image>().color.r, this.GetComponent<Image>().color.g, this.GetComponent<Image>().color.b, this.GetComponent<Image>().color.a - 0.02f);
                        cloud2.color = new Color(this.GetComponent<Image>().color.r, this.GetComponent<Image>().color.g, this.GetComponent<Image>().color.b, this.GetComponent<Image>().color.a - 0.02f);
                    }
                    else
                    {
                        SetEnable(true);
                        this.gameObject.SetActive(false);
                        cloud2.gameObject.SetActive(false);
                        isClick = false;
                    }

                    break;
                case "Cloud2":
                    if (this.GetComponent<Image>().color.a >= 0)
                    {
                        SetEnable(false);
                        cloud3.gameObject.SetActive(true);
                        cloud3.color = new Color(cloud3.color.r, cloud3.color.g, cloud3.color.b, cloud3.color.a + 0.02f);
                        this.GetComponent<Image>().color = new Color(this.GetComponent<Image>().color.r, this.GetComponent<Image>().color.g, this.GetComponent<Image>().color.b, this.GetComponent<Image>().color.a - 0.02f);
                        cloud1.color = new Color(this.GetComponent<Image>().color.r, this.GetComponent<Image>().color.g, this.GetComponent<Image>().color.b, this.GetComponent<Image>().color.a - 0.02f);
                    }
                    else
                    {
                        this.gameObject.SetActive(false);
                        cloud1.gameObject.SetActive(false);
                        isClick = false;
                        SetEnable(true);
                    }
                    break;
                case "Cloud3":
                    if (this.GetComponent<Image>().color.a >= 0)
                    {
                        SetEnable(false);
                        cloud1.gameObject.SetActive(true);
                        cloud2.gameObject.SetActive(true);
                        this.GetComponent<Image>().color = new Color(this.GetComponent<Image>().color.r, this.GetComponent<Image>().color.g, this.GetComponent<Image>().color.b, this.GetComponent<Image>().color.a - 0.02f);
                        cloud1.color = new Color(cloud1.color.r, cloud1.color.g, cloud1.color.b, cloud1.color.a + 0.02f);
                        cloud2.color = new Color(cloud2.color.r, cloud2.color.g, cloud2.color.b, cloud2.color.a + 0.02f);
                    }
                    else
                    {
                        this.gameObject.SetActive(false);
                        isClick = false;
                        SetEnable(true);
                    }
                    break;
            }
        }
    }

    private void OnClick()
    {
        isClick = true;
    }

    private void SetEnable(bool enable)
    {
        mail.enabled = enable;
        cave.enabled = enable;
        farm.enabled = enable;
        cloud1.GetComponent<Button>().enabled = enable;
        cloud2.GetComponent<Button>().enabled = enable;
        cloud3.GetComponent<Button>().enabled = enable;
    }
}
