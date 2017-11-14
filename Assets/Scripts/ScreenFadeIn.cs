using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFadeIn : MonoBehaviour {

    public Image screen;
	// Use this for initialization
	void Start () {
        screen.gameObject.SetActive(true);
        screen.color = new Color(screen.color.r,screen.color.g,screen.color.b,1.0f);
	}

    // Update is called once per frame
    void Update()
    {
        if (screen.gameObject.activeSelf)
        {
            if (screen.color.a >= 0)
                screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, screen.color.a - 0.02f);
            else
            {
                screen.gameObject.SetActive(false);
                this.GetComponent<ScreenFadeIn>().enabled = false;
            }
        }
    }
}
