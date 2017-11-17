using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFadeIn : MonoBehaviour {

    public Image screen;
    public SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start() {
        if (screen != null) {
            screen.gameObject.SetActive(true);
            screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, 1.0f);
        } else if (spriteRenderer != null) {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1.0f);
        }
    }

    // Update is called once per frame
    void Update() {
        if (screen != null && screen.gameObject.activeSelf) {
            if (screen.color.a >= 0)
                screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, screen.color.a - 0.02f);
            else {
                screen.gameObject.SetActive(false);
                this.GetComponent<ScreenFadeIn>().enabled = false;
            }
        } else if (spriteRenderer != null && spriteRenderer.gameObject.activeSelf) {
            if (spriteRenderer.color.a >= 0)
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a - 0.02f);
            else {
                spriteRenderer.gameObject.SetActive(false);
                this.GetComponent<ScreenFadeIn>().enabled = false;
            }
        }
    }
}
