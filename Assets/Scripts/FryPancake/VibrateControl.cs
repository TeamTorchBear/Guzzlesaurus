using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrateControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CheckVibrate();
	}

    private void CheckVibrate()
    {
        m_newAcceleration = Input.acceleration;
        m_detalAcceleration = m_newAcceleration - m_oldAcceleration;
        m_oldAcceleration = m_newAcceleration;

        if (m_detalAcceleration.x > m_checkValue ||
            m_detalAcceleration.y > m_checkValue ||
            m_detalAcceleration.z > m_checkValue)
        {
            Debug.Log("Vibrate!");
           // Handheld.Vibrate();  
        }
    }

    [SerializeField]
    protected float m_checkValue = 0.8f;

    private Vector3 m_detalAcceleration;
    private Vector3 m_oldAcceleration;
    private Vector3 m_newAcceleration;
 
}
