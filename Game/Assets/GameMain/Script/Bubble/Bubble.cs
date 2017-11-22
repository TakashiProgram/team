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


    private bool m_switchingFlag = false;

    private readonly Vector3 m_smallerScale = new Vector3(0.2f, 0.2f, 0.2f);

    private bool m_enemyFlag = false;

    public GameObject m_switchingObject;

    private GameObject test;
    

    Collider col;
    void Start () {
        m_createManager = GameObject.Find("CreateManager");
        m_player = GameObject.Find("Player");

        transform.parent.parent = GameObject.Find("BubbleStart").transform;
       
        Invoke("Rising", RISING_TIME);
        m_createManager.GetComponent<CreateManager>().m_WingMove = new Vector3(0, 0, 0);
        m_inverted = -1;
        m_switchingFlag = false;

    }
	
	void Update () {
        m_move = m_createManager.GetComponent<CreateManager>().m_WingMove;
        //風によって動く方向が変わる
        this.transform.parent.parent.position += m_move * m_bubbleMove 
                                                 * m_inverted * Time.deltaTime;
        
      //上昇
        this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0,
                                                                         m_floatingCount,
                                                                         0);
     
      
    }
    private void OnTriggerEnter(Collider other)
    {
        //最初にシャボン玉に入ったオブジェクトを保持する
        if (m_switchingObject == null)
        {
            m_switchingObject = other.gameObject;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
       
        if (collision.gameObject.tag=="Player")
        {
            GameObject gameobject = collision.gameObject;
            if (m_switchingObject == gameobject)
            {

               
                //バブルが最大かどうか
                if (m_createManager.GetComponent<CreateManager>().m_createWindFlag)
                {
                    m_moveDisabled = 0;
                    m_switchingFlag = true;

                }
                //最大じゃない時に当たると破裂
                else
                {

                    Death();

                }
            }
        }
       
        else if (collision.gameObject.tag=="Enemy" && m_switchingObject.tag=="Enemy")
        {
            GameObject gameobject = collision.gameObject;

            if (m_createManager.GetComponent<CreateManager>().m_createWindFlag)
            {
                m_createManager.GetComponent<CreateManager>().m_WingMove = new Vector3(0, 0, 0);

                collision.GetComponent<Rigidbody>().useGravity = false;
                collision.transform.position = this.transform.position;
                collision.GetComponent<Enemy>().WindStop();
                collision.transform.localScale = m_smallerScale;

                col = collision;
                m_enemyFlag = true;

               
            }
            else
            {
                Death();
                this.GetComponent<SphereCollider>().enabled = false;
            }
        }
        else
        {
            m_inverted *= -1;
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
        if (m_enemyFlag)
        {
            col.GetComponent<Rigidbody>().useGravity = true;
            col.transform.parent = null;
            col.transform.localScale = col.GetComponent<Enemy>().m_scale;
            m_enemyFlag = false;
        }
      
        m_player.GetComponent<Rigidbody>().useGravity = true;
        m_createManager.GetComponent<CreateManager>().m_createWindFlag = false;
        
    }
}
