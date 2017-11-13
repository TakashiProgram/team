﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TimeManager : MonoBehaviour {

  
    [SerializeField]
    private float m_Time;

    [SerializeField]
    private GameObject m_timeLimit;

    [SerializeField]
    private GameObject m_player;

    private float m_rotationTime;

    private float range = 0;

    private const float TIME_MAX = 360;

    void Start () {
        m_rotationTime = TIME_MAX / m_Time;
	}
	
	void Update () {
        Disable();
        if (m_Time<=0)
        {
            m_Time = 0;
            range = 0;
            Destroy(m_player);
        }

    }
    private void Disable()
    {
       
        range = range + (m_rotationTime * Time.deltaTime);
        m_timeLimit.transform.rotation = Quaternion.Euler(0,0, -range);
        m_Time -= Time.deltaTime;

    }
}
