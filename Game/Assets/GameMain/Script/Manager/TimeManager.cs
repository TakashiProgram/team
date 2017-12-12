using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    [SerializeField]
    private GameObject m_timeLimit;

    [SerializeField]
    private GameObject m_player;

    [SerializeField]
    public float m_Time;

    private float m_rotationTime;

    private float m_range = 0.0f;

    private float m_resetTime; 

    private const int TIME_MAX = 360;

    void Start () {
        m_resetTime = m_Time;
        m_rotationTime = TIME_MAX / m_Time;
	}
	
	void Update () {
        Disable();
        if (m_Time<=0)
        {
            m_Time = 0;
            m_range = 0;
            m_player.GetComponent<Player>().Expiration();
        }

    }
    private void Disable()
    {

        m_range = m_range + (m_rotationTime * Time.deltaTime);
        m_timeLimit.transform.rotation = Quaternion.Euler(0,0, -m_range);
        m_Time -= Time.deltaTime;

    }
    public void TimeReset()
    {
        m_Time = m_resetTime;
        m_range = 0;
    }
}
