﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateManager : MonoBehaviour {
    //m_bubbleの動き
    public Vector3 m_WingMove;

    public bool m_createWindFlag = false;

    private GameObject m_bubbleCreateBox;

    //プレイヤー前方
    [SerializeField]
    private GameObject m_playerFront;

    [SerializeField]
    private GameObject m_bubble;

    [SerializeField]
    private GameObject m_wind;

    private float m_bubbleScale;
    

    //m_bubbleの最大scale
    private const float SCALE_MAX = 0.7f;

    
    void Start () {

    }
	
	void Update () {
	}
  
    //BubbleボタンをTapした時の処理
  public void TapBubble(float scale,float flip)
    {
        if (Input.GetMouseButtonDown(0))
        {
           
           
            m_bubbleScale = scale;
            Destroy(m_bubbleCreateBox);
            m_bubbleCreateBox = Instantiate(m_bubble, new Vector3(m_playerFront.transform.position.x, m_playerFront.transform.position.y, 0), Quaternion.identity);
            
        }

            m_bubbleScale+=Time.deltaTime* flip;

            m_bubbleCreateBox.transform.localScale = new Vector3(m_bubbleScale, m_bubbleScale, m_bubbleScale);
            if (m_bubbleScale > SCALE_MAX || m_bubbleScale<-SCALE_MAX)
            {
                m_bubbleScale = SCALE_MAX;
                m_bubbleScale *= flip;

            m_createWindFlag = true;
        }
    }
    //風作成
    public void TapWind(Vector3 vector)
    {
        if (GameObject.Find("Bubble") != null)

        {
            Instantiate(m_wind, m_bubbleCreateBox.transform.position, Quaternion.identity);
            m_WingMove = vector;
        }
    }
}
