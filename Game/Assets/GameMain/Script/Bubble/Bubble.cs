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

    private bool tes = false;

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
        Debug.Log(test);
      
    }
    private void OnTriggerEnter(Collider other)
    {
        ///  test = other.gameObject;
        if (m_switchingObject == null)
        {
            m_switchingObject = other.gameObject;
            Debug.Log("入った");
        }
        else
        {
            tes = true;
            // if (m_switchingObject != null)
            //{
            //  Death();
            //}
        }

        
        //if (m_switchingFlag)
        //{
        //    if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
        //    {
        //      //  Death();
        //    }
        //    else if (other.gameObject.tag == "Enemy")
        //    {

        //    }
        //}
    }

    private void OnTriggerStay(Collider collision)
    {
       
        if (collision.gameObject.tag=="Player")
        {
            test = collision.gameObject;
            if (m_switchingObject == test)
            {

                Debug.Log("fdsbvn");
                // if (m_switchingObject==test)
                //{
                //バブルが最大かどうか
                if (m_createManager.GetComponent<CreateManager>().m_createWindFlag)
                {
                    m_moveDisabled = 0;
                    m_switchingFlag = true;
                    //   m_switchingObject = collision.gameObject;

                }
                //最大じゃない時に当たると破裂
                else
                {

                    Death();

                }
            }
        }
        //else
        //{
        //    m_inverted *= -1;
        //    Death();
        //}

        else if (collision.gameObject.tag=="Enemy" && m_switchingObject.tag=="Enemy")
        {
            test = collision.gameObject;

            if (m_createManager.GetComponent<CreateManager>().m_createWindFlag)
            {
                m_createManager.GetComponent<CreateManager>().m_WingMove = new Vector3(0, 0, 0);

                collision.GetComponent<Rigidbody>().useGravity = false;
                collision.transform.position = this.transform.position;
                collision.GetComponent<Enemy>().WindStop();
                collision.transform.localScale = m_smallerScale;

                col = collision;
                m_enemyFlag = true;

                //  m_switchingObject = collision.gameObject;
                // m_switchingFlag = true;
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

        //} if(collision.gameObject.tag == "Enemy")
        //    {
        //        /*  if (m_switchingFlag)
        //          {
        //              //バブルにプレイヤーが乗っている時の処理
        //              m_inverted *= -1;
        //              Death();
        //          }else*/
        //        // {
        //        //if (m_switchingObject != null)
        //        //{
        //        //    Death();
        //        //}
        //       // if (m_switchingObject == test)
        //        //{


        //            //最大なら
        //            if (m_createManager.GetComponent<CreateManager>().m_createWindFlag)
        //            {
        //                m_createManager.GetComponent<CreateManager>().m_WingMove = new Vector3(0, 0, 0);

        //                collision.GetComponent<Rigidbody>().useGravity = false;
        //                collision.transform.position = this.transform.position;
        //                collision.GetComponent<Enemy>().WindStop();
        //                collision.transform.localScale = m_smallerScale;

        //                col = collision;
        //                m_enemyFlag = true;

        //              //  m_switchingObject = collision.gameObject;
        //                // m_switchingFlag = true;
        //            }
        //            else
        //            {
        //                Death();
        //                this.GetComponent<SphereCollider>().enabled = false;
        //            }
        //        }/*else
        //        {
        //            Death();
        //        }*/

        //       // }

        // //   }
        //   /* else
        //    {

        //        m_inverted *= -1;
        //        m_moveDisabled = m_setMove;

        //        Death();

        //    }*/
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
    //敵にBubbleが当たった時（プレイヤーはいない）
    public void Enemy()
    {

    }
}
