using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CookBookBtn : MonoBehaviour {

    public JarBtnClick jbc;
    public Image Screen;
    public SpriteRenderer spriteRenderer;

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
            else
            {
                isClick = false;
            }
        }

    }

    void OnClick()
    {
        isClick = true;
        AkSoundEngine.PostEvent("Click_Recipe", gameObject);
    }

    void ScreenFadeOut(string scene)
    {
        FindObjectOfType<ScreenFadeIn>().enabled = false;
        if (Screen != null)
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
        else if (spriteRenderer != null)
        {
            spriteRenderer.gameObject.SetActive(true);
            if (spriteRenderer.color.a <= 1.0f)
            {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + 0.02f);
            }
            else
            {
                isClick = false;
                //Screen.gameObject.SetActive(false);
                Debug.Log(scene);
                SceneManager.LoadScene(scene);
            }
        }
    }
}
