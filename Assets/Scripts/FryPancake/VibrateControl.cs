using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrateControl : MonoBehaviour {

    private PromptControl prompt;
    // Use this for initialization
    void Start()
    {
        prompt = FindObjectOfType<PromptControl>();
    }

    // Update is called once per frame
    void Update() {
        if(prompt.GetComponent<Transform>().localScale.x==0.4f)
        CheckVibrate();
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

        }
        if (Input.GetKey(KeyCode.M))
        {
            Debug.Log("Vibrate!");
            // Handheld.Vibrate();  
            this.GetComponent<VibrateControl>().enabled = false;
        }
    }

    [SerializeField]
    protected float m_checkValue = 1.5f;

    private Vector3 m_detalAcceleration;
    private Vector3 m_oldAcceleration;
    private Vector3 m_newAcceleration;
}