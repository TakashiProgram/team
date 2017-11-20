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


    private bool testFlag = false;

    private readonly Vector3 m_smallerScale = new Vector3(0.2f, 0.2f, 0.2f);

    private bool m_enemyFlag = false;


    Collider col;
    void Start () {
        m_createManager = GameObject.Find("CreateManager");
        m_player = GameObject.Find("Player");

        transform.parent.parent = GameObject.Find("BubbleStart").transform;
       
        Invoke("Rising", RISING_TIME);
        m_createManager.GetComponent<CreateManager>().m_WingMove = new Vector3(0, 0, 0);
        m_inverted = -1;
        testFlag = false;

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
            //バブルが最大かどうか
            if (m_createManager.GetComponent<CreateManager>().m_createWindFlag)
            {
                m_moveDisabled = 0;
                testFlag = true;
            }
            //最大じゃない時に当たると破裂
            else{
              
                Death();

            }
            

        }else if(collision.gameObject.tag == "Enemy")
        {
            if (testFlag)
            {
                //バブルにプレイヤーが乗っている時の処理
                m_inverted *= -1;
                Debug.Log("esrfdb");
                Death();
            }else
            {
                
                m_createManager.GetComponent<CreateManager>().m_WingMove = new Vector3(0, 0, 0);
                
                collision.GetComponent<Rigidbody>().useGravity = false;
                 collision.transform.position = this.transform.position;

                collision.transform.localScale = m_smallerScale;

                col = collision;
                m_enemyFlag = true;

            }
           
        }
        else
        {
            Debug.Log(collision.gameObject.name);
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
        if (m_enemyFlag)
        {
            col.GetComponent<Rigidbody>().useGravity = true;
            col.transform.parent = null;
            col.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            m_enemyFlag = false;
        }
      
        m_player.GetComponent<Rigidbody>().useGravity = true;
        m_createManager.GetComponent<CreateManager>().m_createWindFlag = false;
        
    }
    //敵にBubbleが当たった時（プレイヤーはいない）
    public void Enemy()
    {

    }
}
