using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    private float m_bubbleScale;
    private GameObject m_bubbleCreateBox;

    [SerializeField]
    private GameObject m_bubble;

    [SerializeField]
    private GameObject m_playerFront;

    private const float SCALE_MAX = 0.7f;

    public bool m_createWindFlag = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        TapBubble( 0.0f);

    }

    public void TapBubble(float scale)
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_bubbleScale = scale;
            //  Invoke("Tap", 0.2f);
            Destroy(m_bubbleCreateBox);
            //1.17f
            //仮
            //    qq();
            m_bubbleCreateBox = Instantiate(m_bubble, new Vector3(m_playerFront.transform.position.x, m_playerFront.transform.position.y, 0), Quaternion.identity);

        }

        m_bubbleScale += Time.deltaTime;

        m_bubbleCreateBox.transform.localScale = new Vector3(m_bubbleScale, m_bubbleScale, m_bubbleScale);
        if (m_bubbleScale > SCALE_MAX)
        {
            m_bubbleScale = SCALE_MAX;
            m_createWindFlag = true;
        }
    }
}
