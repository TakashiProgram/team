using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateManager : MonoBehaviour {
    //m_bubbleの動き
    public Vector3 m_WingMove;

    private GameObject m_bubbleCreateBox;

    //プレイヤー前方
    [SerializeField]
    private GameObject m_playerFront;

    [SerializeField]
    private GameObject m_bubble;

    [SerializeField]
    private GameObject m_wind;

    private const int SCALE_MAX = 1;

    private float m_bubbleScale;

    public bool m_windFlag = false;
    

    void Start () {

    }
	
	void Update () {
		
	}

    //バブル生成処理
  public  void TapBubble(float scale)
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
            m_windFlag = true;
            }
    }
    //風作成
    public void TapWind(Vector3 vector)
    {
        Instantiate(m_wind, m_bubbleCreateBox.transform.position, Quaternion.identity);
        
        m_WingMove = vector;

    }
}
