using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    [SerializeField]
    private Text timeText;

    [SerializeField]
    private float m_Time;
    
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Disable();

    }
    private void Disable()
    {
        timeText.text = "Time: " + Mathf.FloorToInt(m_Time);
        m_Time -= Time.deltaTime;
        
    }
}
