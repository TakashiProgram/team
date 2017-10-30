using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    [SerializeField]
    private Text m_timeText;

    [SerializeField]
    private float m_Time;
    
    void Start () {
		
	}
	
	void Update () {
        Disable();

    }
    private void Disable()
    {
        m_timeText.text = "Time: " + Mathf.FloorToInt(m_Time);
        m_Time -= Time.deltaTime;
        
    }
}
