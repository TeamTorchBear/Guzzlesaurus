using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartBtnOnClick : MonoBehaviour
{
    public Image Screen;
    bool isClick = false;
    // Use this for initialization
    void Start()
    {
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
       // Screen = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isClick) {
            if(Screen.color.r >= 0)
            {
                Screen.color = new Color(Screen.color.r - 0.02f, Screen.color.g - 0.02f, Screen.color.b - 0.02f, Screen.color.a);
            }
            else
            {
                SceneManager.LoadScene("GuzzWorldScreen");
            }
        }
       
    }

    void OnClick()
    {
        isClick = true;
        
    }
}
