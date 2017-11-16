using System.Collections;
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

    private float m_floatingCount = 0;

    [SerializeField]
    private  float m_bubbleMove;
    //スワイプでbubbleを動かしたときに反対の値
    private int m_inverted = -1;
    //自動で上昇が発動するまでの時間
    private const int RISING_TIME = 3;
    
    void Start () {
        m_createManager = GameObject.Find("CreateManager");
        m_player = GameObject.Find("Player");

        transform.parent.parent = GameObject.Find("BubbleStart").transform;
       
        Invoke("Rising", RISING_TIME);
        m_createManager.GetComponent<CreateManager>().m_WingMove = new Vector3(0, 0, 0);
        m_inverted = -1;
    }
	
	void Update () {
        m_move = m_createManager.GetComponent<CreateManager>().m_WingMove;
        //風によって動く方向が変わる
        this.transform.parent.parent.position += m_move * m_bubbleMove * m_inverted * Time.deltaTime;
        
      //上昇
        this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0,
                                                                         m_floatingCount,
                                                                         0);

    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            if (m_createManager.GetComponent<CreateManager>().m_createWindFlag)
            {
                m_moveDisabled = 0;
            }else{
              
                Death();
            }
            

        }else if(collision.gameObject.tag == "Enemy")
        {
            m_inverted *= -1;
        }
        else
        {
            m_inverted *= -1;
            m_moveDisabled = m_setMove;
           
            Death();
        }
    }

    public void Death()
    {
        this.GetComponent<BubbleController>().Burst();
        
    }
    public void DestroyTime()
    {
        Invoke("Death", m_survivalTime);
    }

    private void Rising()
    {
        m_floatingCount = 0.5f;
    }

    public void ParentRelease()
    {
        m_player.transform.parent = null;
        m_player.GetComponent<Player>().m_bubbleFlag = false;

        m_player.GetComponent<Rigidbody>().useGravity = true;
        m_createManager.GetComponent<CreateManager>().m_createWindFlag = false;
        
    }
}
