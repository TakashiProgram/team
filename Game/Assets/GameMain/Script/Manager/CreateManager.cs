using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateManager : MonoBehaviour {

    public Vector3 WingMove;

    public GameObject BubbleCreateBox;

    //プレイヤー前方
    [SerializeField]
    private GameObject front;
    [SerializeField]
    private GameObject Bubble,Wind;

    private int m_ScaleMax = 1;

    private float m_BubbleScale = 0;

    private bool longPressFlag = false;

    public bool windFlag = false;
    

    void Start () {

        m_BubbleScale = 0;

    }
	
	void Update () {
		
	}

    //バブル生成処理
  public  void TapBubble(float scale)
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_BubbleScale = scale;

            Destroy(BubbleCreateBox);

            BubbleCreateBox = Instantiate(Bubble, new Vector3(front.transform.position.x, front.transform.position.y, 0), Quaternion.identity);
            
        }

            m_BubbleScale+=Time.deltaTime;

            BubbleCreateBox.transform.localScale = new Vector3(m_BubbleScale, m_BubbleScale, m_BubbleScale);
            if (m_BubbleScale > m_ScaleMax)
            {
                m_BubbleScale = m_ScaleMax;
            windFlag = true;
            }
    }
    //風作成
    public void TapWind(Vector3 vector)
    {
        Instantiate(Wind, BubbleCreateBox.transform.position, Quaternion.identity);
        
        WingMove = vector;

    }
}
