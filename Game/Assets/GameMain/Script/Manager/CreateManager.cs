using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateManager : MonoBehaviour {
   
    //プレイヤー前方
    [SerializeField]
    private GameObject front;
    [SerializeField]
    private GameObject Bubble,Wind;

    public GameObject BubbleCreateBox;

    private bool longPressFlag = false;

    private float m_BubbleScale = 0;

    private int m_ScaleMax = 1;

    public bool windflagtest = false;
    public Vector3 dir;

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
            windflagtest = true;
            }
    }
    //風作成
    public void TapWind(float test)
    {
        Instantiate(Wind, BubbleCreateBox.transform.position, Quaternion.identity);

        //   float angleDir = transform.eulerAngles.z * (Mathf.PI / 180.0f);
          dir = new Vector3(Mathf.Cos(test), Mathf.Sin(test), 0.0f);
    //    dir = new Vector3(0,test,0);
        BubbleCreateBox.transform.position += dir * 5 * Time.deltaTime;

    }
}
