﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornetController : MonoBehaviour {
    const float MAX_SPEED = 10.0f;
    //巡回をする場所を格納するオブジェクト群
    [SerializeField, Tooltip("巡回する場所を格納する")]
    protected GameObject[] m_wayPoints;

    //折り返しするかどうか
    [SerializeField, Tooltip("折り返しフラグ")]
    protected bool m_isTurnUp;

    [SerializeField, Range(1.0f, MAX_SPEED)]
    private float m_speed;
    //巡回をする情報を保存するキュー
    private Queue<Vector3> m_circle;

    //現在移動するかのフラグ
    private bool m_isMoved = true;
    /// <summary>
    /// 敵の移動の更新範囲
    /// </summary>
    protected float m_updateArea;

    void Start()
    {
        m_circle = new Queue<Vector3>();
        if (!m_isTurnUp)
        {
            foreach (var way in m_wayPoints)
            {
                m_circle.Enqueue(way.transform.position);
            }
        }
        else
        {

            foreach (var way in m_wayPoints)
            {
                m_circle.Enqueue(way.transform.position);
            }

            for (int i = m_wayPoints.Length - 2; i > 0; i--)
            {
                m_circle.Enqueue(m_wayPoints[i].transform.position);
            }
        }
        m_isMoved = false;
    }

    private void Update()
    {
        
        transform.LookAt(m_circle.Peek());
        Vector3 rotVec = transform.position;

        Quaternion rot = transform.rotation;
        rot.z = 0;
        rot.x = 0;
        transform.SetPositionAndRotation(transform.position + (transform.forward * (float)(m_speed / MAX_SPEED)), rot);
        
        if ((m_circle.Peek() - transform.position).magnitude < 0.5f)
        {
            UpdateWayPoints();
        }
           
    }

    public void SetMove(bool _isMoved)
    {
        m_isMoved = _isMoved;
    }


    protected void UpdateWayPoints()
    {
        m_circle.Enqueue(m_circle.Dequeue());

    }
}
