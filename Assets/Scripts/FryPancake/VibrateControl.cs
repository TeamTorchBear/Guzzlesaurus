using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrateControl : MonoBehaviour {

    private PromptControl prompt;
    //private SpriteRenderer pancake;
    // Use this for initialization
    void Start()
    {
        //pancake = GameObject.FindGameObjectWithTag("Minigame4Pancake").GetComponentInChildren<SpriteRenderer>();
        prompt = FindObjectOfType<PromptControl>();
        AkSoundEngine.SetRTPCValue("SizzleVolume", 100f, null, 0);
    }

    // Update is called once per frame
    void Update() {
        if(prompt.GetComponent<Transform>().localScale.x==0.4f)
        CheckVibrate();
        //AkSoundEngine.SetRTPCValue("SizzleVolume", 40f, null, 20);
    }

    private void CheckVibrate() {
        m_newAcceleration = Input.acceleration;
        m_detalAcceleration = m_newAcceleration - m_oldAcceleration;
        m_oldAcceleration = m_newAcceleration;

        if (m_detalAcceleration.x > m_checkValue ||
            m_detalAcceleration.y > m_checkValue ||
            m_detalAcceleration.z > m_checkValue)
        {
            Debug.Log("Vibrate!");
            // Handheld.Vibrate();  
            this.GetComponent<VibrateControl>().enabled = false;
            //AkSoundEngine.PostEvent("WaitForIt", gameObject);
            AkSoundEngine.PostEvent("Pancake_Flip", gameObject);
            AkSoundEngine.SetRTPCValue("SizzleVolume", 24f, null, 100);
            AkSoundEngine.SetRTPCValue("SizzlePitch", 0f, null, 5);
            AkSoundEngine.PostEvent("Pancake_Splat", gameObject);

        }
        if (Input.GetKey(KeyCode.M))
        {
            Debug.Log("FlipIT!!");
            // Handheld.Vibrate();  
            AkSoundEngine.PostEvent("Pancake_Flip", gameObject);
            AkSoundEngine.SetRTPCValue("SizzleVolume", 0f, null, 0);
            AkSoundEngine.SetRTPCValue("SizzlePitch", 0f, null, 5);
            AkSoundEngine.PostEvent("Pancake_Splat", gameObject);
            //AkSoundEngine.SetRTPCValue("SizzleVolume", 0f, null, 50);
            //AkSoundEngine.SetRTPCValue("SizzlePitch", 100f, null, 50);
            //AkSoundEngine.SetRTPCValue("SizzleVolume", 40f, null, 50);
            //AkSoundEngine.SetRTPCValue("SizzlePitch", 40f, null, 10);
            this.GetComponent<VibrateControl>().enabled = false;
        }
    }

    [SerializeField]
    protected float m_checkValue = 1.5f;

    private Vector3 m_detalAcceleration;
    private Vector3 m_oldAcceleration;
    private Vector3 m_newAcceleration;
}