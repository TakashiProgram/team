using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {
    //当たったオブジェクトの格納場所
    public GameObject m_switchingObject;

    private GameObject m_createManager;

    private GameObject m_player;

    //風の向きを保持する
    private Vector3 m_move;

    //生存時間
    [SerializeField]
    private int m_survivalTime;

    //スワイプで動かしたときに反対の値
    private int m_inverted = -1;

    //移動速度
    [SerializeField]
    private float m_bubbleMove;

    //時間がたって上昇の値
    private float m_floatingCount = 0;

    public Collider m_hitCollider;

    private bool m_enemyFlag = false;

    //自動で上昇が発動するまでの時間
    private const int RISING_TIME = 3;


    public bool test = false;

    public static int tt = 0;
    //敵に当たったらScaleを変更する値
    private readonly Vector3 m_smallerScale = new Vector3(0.2f, 0.2f, 0.2f);

    private GameObject oo;
    int s=1;
    bool testflag=false;

    public static GameObject test3;
        
    void Start () {
        m_createManager = GameObject.Find("CreateManager");
        m_player = GameObject.Find("Player");

        oo = GameObject.Find("BubbleStart");
        transform.parent.parent = GameObject.Find("BubbleStart").transform;
       
        Invoke("Rising", RISING_TIME);
        m_createManager.GetComponent<CreateManager>().m_WingMove = new Vector3(0, 0, 0);
        m_inverted = -1;
        tt++;
        if (test3!=null)
        {
            //Destroy(test3);
            //  Death();
            test3.GetComponent<BubbleController>().Burst();
        }
      //  else
        {
            test3 = this.gameObject;
        }
        
        
        testflag = true;
        //s = transform.parent.Find("Bubble(Clone)").GetSiblingIndex();
        // Debug.Log(s);
        // s = oo.transform.GetSiblingIndex();
        //   s=oo.transform.
        // 
       

    }
	
	void Update () {

       
        m_move = m_createManager.GetComponent<CreateManager>().m_WingMove;
        Debug.Log(tt);
        if (m_move== Vector3.zero)
        {
            //上昇
            this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0,m_floatingCount,0);

        }else
        {

          //  if (tt == 1 || test == true)
            {
                //風によって動く方向が変わる
                this.transform.parent.position += m_move * m_bubbleMove * m_inverted * Time.deltaTime;
                Debug.Log("fedbdvc");
                this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

            }

        }
       
    }
   
    public void rr()
    {
        Destroy(gameObject);
    }
    public void Death()
    {


        ParentRelease();
        this.GetComponent<BubbleController>().Burst();

        tt=0;

        test = false;
    }

    public void rrr()
    {
        //   m_hitCollider.GetComponent<LarvaController>().RemoveBubble();
        m_hitCollider.GetComponent<Rigidbody>().useGravity = true;
        m_hitCollider.transform.parent = null;
        LarvaController tmp = m_hitCollider.GetComponent<LarvaController>();

        if (tmp) tmp.RemoveBubble();
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
        m_player.GetComponent<Player>().BubbleFlag();
        if (m_enemyFlag)
        {
            m_hitCollider.GetComponent<Rigidbody>().useGravity = true;
            m_hitCollider.transform.parent = null;
            LarvaController tmp = m_hitCollider.GetComponent<LarvaController>();

            if (tmp) tmp.RemoveBubble();
            m_enemyFlag = false;
        }
      
        m_player.GetComponent<Rigidbody>().useGravity = true;
        m_createManager.GetComponent<CreateManager>().m_createWindFlag = false;
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        //最初にシャボン玉に入ったオブジェクトを保持する
        if (m_switchingObject == null)
        {
            m_switchingObject = collision.gameObject;

        }

        if (collision.gameObject.tag == "Player")
        {
            GameObject gameobject = collision.gameObject;
            if (m_switchingObject == gameobject)
            {

                //最大じゃない時に当たると破裂
                if (m_createManager.GetComponent<CreateManager>().m_createWindFlag == false)
                {
                    Debug.Log("m_switchingObject");
                    Death();
                }
            }
        }

        else if (collision.gameObject.tag == "Enemy" && m_switchingObject.tag == "Enemy")
        {
            GameObject gameobject = collision.gameObject;

            if (m_createManager.GetComponent<CreateManager>().m_createWindFlag)
            {
                m_createManager.GetComponent<CreateManager>().m_WingMove = new Vector3(0, 0, 0);

                collision.GetComponent<Rigidbody>().useGravity = false;
                collision.transform.position = this.transform.position;
                collision.transform.localScale = m_smallerScale;
                m_createManager.GetComponent<CreateManager>().PutInObject(collision);
                Debug.Log("efsdbvc");
                m_hitCollider = collision;
                m_enemyFlag = true;
                
            }
            else
            {

                Death();
                rrr();
                this.GetComponent<SphereCollider>().enabled = false;
            }
        }
        else if (collision.gameObject.tag == "CheackPoint")
        {
            //Bubbleを当ててチェックポイント通過にするかも
        }
        else
        {
            Debug.Log("これ");
            m_inverted = 0;
            Death();
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        //最初にシャボン玉に入ったオブジェクトを保持する
        if (m_switchingObject == null)
        {
            m_switchingObject = collision.gameObject;

        }

        if (collision.gameObject.tag == "Player")
        {
            GameObject gameobject = collision.gameObject;
            if (m_switchingObject == gameobject)
            {

                //最大じゃない時に当たると破裂
                if (m_createManager.GetComponent<CreateManager>().m_createWindFlag == false)
                {
                    Debug.Log("m_switchingObject");
                    Death();
                }
            }
        }

        else if (collision.gameObject.tag == "Enemy" && m_switchingObject.tag == "Enemy")
        {
            GameObject gameobject = collision.gameObject;

            if (m_createManager.GetComponent<CreateManager>().m_createWindFlag)
            {
                m_createManager.GetComponent<CreateManager>().m_WingMove = new Vector3(0, 0, 0);

                collision.GetComponent<Rigidbody>().useGravity = false;
                collision.transform.position = this.transform.position;
                collision.transform.localScale = m_smallerScale;
              //  m_createManager.GetComponent<CreateManager>().PutInObject(collision);
                Debug.Log("efsdbvc");
             //   m_hitCollider = collision;
               // m_enemyFlag = true;
            }
            else
            {

                Death();
                rrr();
                this.GetComponent<SphereCollider>().enabled = false;
            }
        }
        else if (collision.gameObject.tag == "CheackPoint")
        {
            //Bubbleを当ててチェックポイント通過にするかも
        }
        else
        {
            Debug.Log("これ");
            m_inverted = 0;
            Death();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

    }
}
