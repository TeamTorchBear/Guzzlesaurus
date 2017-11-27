using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudBtn : Clickable
{
    public SpriteRenderer cloud1, cloud2, cloud3;
    public Button mail, farm, cave;
    bool isClick;

    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    public override void OnStart()
    {
        isClick = false;
        //Button btn = this.GetComponent<Button>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //btn.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (isClick)
        {
            switch (this.name)
            {
                case "Cloud1":
                    if (spriteRenderer.color.a >= 0)
                    {
                        SetEnable(false);
                        cloud3.gameObject.SetActive(true);
                        cloud3.color = new Color(cloud3.color.r, cloud3.color.g, cloud3.color.b, cloud3.color.a + 0.02f);
                        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a - 0.02f);
                        cloud2.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a - 0.02f);
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
                    if (spriteRenderer.color.a >= 0)
                    {
                        SetEnable(false);
                        cloud3.gameObject.SetActive(true);
                        cloud3.color = new Color(cloud3.color.r, cloud3.color.g, cloud3.color.b, cloud3.color.a + 0.02f);
                        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a - 0.02f);
                        cloud1.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a - 0.02f);
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
                    if (spriteRenderer.color.a >= 0)
                    {
                        SetEnable(false);
                        cloud1.gameObject.SetActive(true);
                        cloud2.gameObject.SetActive(true);
                        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a - 0.02f);
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

    public override void OnClick()
    {
        Debug.Log("OnClick()");
        isClick = true;
    }

    private void SetEnable(bool enable)
    {
        mail.enabled = enable;
        cave.enabled = enable;
        farm.enabled = enable;
        cloud1.GetComponent<Collider2D>().enabled = enable;
        cloud2.GetComponent<Collider2D>().enabled = enable;
        cloud3.GetComponent<Collider2D>().enabled = enable;
    }
}
