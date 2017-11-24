using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplacePancake : MonoBehaviour
{
    public Sprite pancakel1, pancakel2, pancakel3, pancakel4;
    public GameObject pancake;

    private PromptControl promt;
    private float fryTime = 0;
    private int checkPancakeReplaceTimes = 0;
    private bool isPromptFinish = false;
    private bool isCalledPrompt = false;
    // Use this for initialization
    void Start()
    {
        promt = FindObjectOfType<PromptControl>();
        pancake = GameObject.FindGameObjectWithTag("Minigame4Pancake");
        pancake.GetComponentInChildren<SpriteRenderer>().sprite = pancakel1;
        
        if (!isCalledPrompt)
        {
            //promt.ShowPromptAfter(0, 2, () =>
            //{
            //    Debug.Log("Closed");
            //    isPromptFinish = true;
            //    isCalledPrompt = true;
            //}, true);
            isCalledPrompt = true;
            promt.Hide(() => {
                promt.content.SetActive(false);
                promt.content = promt.GetComponentsInChildren<Transform>(true)[5].gameObject;
                promt.content.SetActive(true);
                promt.PlayAnimations();
                promt.ShowPromptAfter(0, 4, () => {
                    isPromptFinish = true;
                }, true);
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPromptFinish)
        {
            fryTime += Time.deltaTime;
        }
        if (fryTime >= 4)
        {
            ReplaceSprites();
            FindObjectOfType<PanShake>().enabled = true;


            /* 
             * Solved :D 
             * The problem was here, the object was being enabled every frame 
             */
            //promt.gameObject.GetComponentsInChildren<Transform>()[2].gameObject.SetActive(false);
            //promt.gameObject.GetComponentsInChildren<Transform>(true)[5].gameObject.SetActive(true);

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
