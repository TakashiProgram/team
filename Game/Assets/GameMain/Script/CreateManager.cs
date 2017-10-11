using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateManager : MonoBehaviour {
    

    private GameObject BubbleCreate;
    //プレイヤー前方
    [SerializeField]
    private GameObject front;
    
    public GameObject[] Bubble;

    private bool longPressFlag = false;



    private float m_BubbleScale = 0;
    // Use this for initialization
    void Start () {
        Debug.Log("rgfh");
        m_BubbleScale = 0;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

  public  void TapBubble()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Destroy(BubbleCreate);

            BubbleCreate = Instantiate(Bubble[0], new Vector3(front.transform.position.x, front.transform.position.y, 0), Quaternion.identity);

            longPressFlag = true;
        }
        if (longPressFlag)
        {
            m_BubbleScale += 0.01f;

            BubbleCreate.transform.localScale = new Vector3(m_BubbleScale, m_BubbleScale, m_BubbleScale);
            if (m_BubbleScale > 1)
            {
                m_BubbleScale = 1;

            }
        }

    }
}
