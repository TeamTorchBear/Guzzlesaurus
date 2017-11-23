using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplacePancake : MonoBehaviour
{
    public Sprite pancakel1, pancakel2, pancakel3, pancakel4;
    public GameObject pancake;

    private float fryTime = 0;
    private int checkPancakeReplaceTimes = 0;
    // Use this for initialization
    void Start()
    {
        pancake = GameObject.FindGameObjectWithTag("Minigame4Pancake");
        pancake.GetComponentInChildren<SpriteRenderer>().sprite = pancakel1;
    }

    // Update is called once per frame
    void Update()
    {
        fryTime += Time.deltaTime;
        if (fryTime >= 4)
        {
            ReplaceSprites();
            FindObjectOfType<PanShake>().enabled = true;
            if (!FindObjectOfType<VibrateControl>().enabled)
            {
                fryTime = 0;
                checkPancakeReplaceTimes = 0;
                FindObjectOfType<PanShake>().enabled = false;
                FindObjectOfType<PanShake>().GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    private void ReplaceSprites()
    {
        if (checkPancakeReplaceTimes == 0)
        {
            if (pancake.GetComponentInChildren<SpriteRenderer>().sprite == pancakel1)
                pancake.GetComponentInChildren<SpriteRenderer>().sprite = pancakel2;
            else if (pancake.GetComponentInChildren<SpriteRenderer>().sprite == pancakel2)
                pancake.GetComponentInChildren<SpriteRenderer>().sprite = pancakel3;
            else if (pancake.GetComponentInChildren<SpriteRenderer>().sprite == pancakel3)
                pancake.GetComponentInChildren<SpriteRenderer>().sprite = pancakel4;
            else if (pancake.GetComponentInChildren<SpriteRenderer>().sprite == pancakel4)
                pancake.GetComponentInChildren<SpriteRenderer>().sprite = pancakel2;

            FindObjectOfType<VibrateControl>().enabled = true;
        }
        checkPancakeReplaceTimes++;
    }
}
