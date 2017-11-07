using System.Collections;
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

    private const float SCALE_MAX = 0.7f;

    private float m_bubbleScale;
    

    void Start () {

    }
	
	void Update () {
        Debug.Log("createFlag"+m_createWindFlag);
	}

    //バブル生成処理
  public void TapBubble(float scale)
    {
        //バブルボタンを押された時の処理
        if (Input.GetMouseButtonDown(0))
        {
            m_bubbleScale = scale;

            Destroy(m_bubbleCreateBox);

            m_bubbleCreateBox = Instantiate(m_bubble, new Vector3(m_playerFront.transform.position.x, m_playerFront.transform.position.y, 0), Quaternion.identity);
            
        }

            m_bubbleScale+=Time.deltaTime;

            m_bubbleCreateBox.transform.localScale = new Vector3(m_bubbleScale, m_bubbleScale, m_bubbleScale);
            if (m_bubbleScale > SCALE_MAX)
            {
                m_bubbleScale = SCALE_MAX;
            m_createWindFlag = true;
        }
    }
    //風作成
    public void TapWind(Vector3 vector)
    {
        Instantiate(m_wind, m_bubbleCreateBox.transform.position, Quaternion.identity);
        
        m_WingMove = vector;

    }
}
