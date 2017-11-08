using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    [SerializeField]
    private Text m_timeText;

    [SerializeField]
    private float m_Time;

    [SerializeField]
    private GameObject m_player;

    void Start () {
		
	}
	
	void Update () {
        Disable();
        if (m_Time<=0)
        {
            m_Time = 0;
            Destroy(m_player);
        }

    }
    private void Disable()
    {
        m_timeText.text = "Time: " + Mathf.FloorToInt(m_Time);
        m_Time -= Time.deltaTime;
        
    }
}
