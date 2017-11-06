﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {

    [SerializeField]
    private float m_setMove;
    
    private float m_moveDisabled;

    [SerializeField]
    private int m_survivalTime;

    private GameObject m_createManager;

    private GameObject m_player;

    private Vector3 m_move;

    private const float BUBBLE_MOVE = 1.5f;

    private const int INVERTED = -1;

    private const int RISING_TIME = 3;

    private float m_floatingCount = 0;



    void Start () {
        m_createManager = GameObject.Find("CreateManager");
        m_player = GameObject.Find("Player");

        
        m_moveDisabled = m_setMove;
        Invoke("Rising", RISING_TIME);
        m_createManager.GetComponent<CreateManager>().m_WingMove = new Vector3(0, 0, 0);
    }
	
	void Update () {
        m_move = m_createManager.GetComponent<CreateManager>().m_WingMove;
        //風によって動く方向が変わる
        this.transform.position+= m_move * BUBBLE_MOVE * INVERTED * Time.deltaTime;

        //上昇
        Vector3 move = this.transform.position;
         move.y += /*m_setMove **/ m_moveDisabled* m_floatingCount * Time.deltaTime;
      //  move.y = 1 * Time.deltaTime;
        this.transform.position = move;
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag=="Player")
        {

            m_moveDisabled = 0;
           
        }
        else
        {
            m_moveDisabled = m_setMove;
            DestroyTime();
        }
    }

    public void DestroyTime()
    {
        m_player.transform.parent = null;
        m_player.GetComponent<Player>().m_bubbleFlag = false;

        m_player.GetComponent<Rigidbody>().useGravity = true;
        m_createManager.GetComponent<CreateManager>().m_createWindFlag = false;
        Destroy(gameObject);
    }
    public void Destroy()
    {
        Invoke("DestroyTime", m_survivalTime);
    }

    private void Rising()
    {
        m_floatingCount = 1;
    }
}
