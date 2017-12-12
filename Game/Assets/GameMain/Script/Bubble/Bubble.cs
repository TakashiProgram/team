using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {
    //当たったオブジェクトの格納場所
    public GameObject m_switchingObject;

    //プランナーが指定
    //生存時間
    [SerializeField]
    private int m_survivalTime;
    //移動速度
    [SerializeField]
    private float m_bubbleMove;

    private GameObject m_createManager;

    private GameObject m_player;
    //風の向きを保持する
    private Vector3 m_move;

    private Collider m_hitCollider;

    //スワイプで動かしたときに反対の値
    private int m_inverted = -1;
    //時間がたって上昇の値
    private float m_floatingCount = 0;
   
    private bool m_enemyFlag = false;

    //自動で上昇が発動するまでの時間
    private const int RISING_TIME = 3;
    //敵に当たったらScaleを変更する値
    private readonly Vector3 m_smallerScale = new Vector3(0.2f, 0.2f, 0.2f);

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
        if (m_move==new Vector3(0,0,0))
        {
            //上昇
            this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0,m_floatingCount,0);

        }else
        {
            //風によって動く方向が変わる
            this.transform.parent.parent.position += m_move * m_bubbleMove * m_inverted * Time.deltaTime;

            this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        }
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
               //最大じゃない時に当たると破裂
                if (m_createManager.GetComponent<CreateManager>().m_createWindFlag==false)
                {
                    Death(collision);
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
                collision.transform.localScale = m_smallerScale;
                m_createManager.GetComponent<CreateManager>().PutInObject(collision);

                m_hitCollider = collision;
                m_enemyFlag = true;
            }
            else
            {
                Death(collision);
                this.GetComponent<SphereCollider>().enabled = false;
            }
        }
        else if(collision.gameObject.tag=="CheackPoint")
        {
           //Bubbleを当ててチェックポイント通過にするかも
        }
        else
        {
            m_inverted *= -1;
            Death(collision);
        }
    }

    public void Death(Collider collision)
    {
        this.GetComponent<BubbleController>().Burst(collision);
        
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
            m_hitCollider.GetComponent<Rigidbody>().useGravity = true;
            m_hitCollider.transform.parent = null;
            m_enemyFlag = false;
        }
      
        m_player.GetComponent<Rigidbody>().useGravity = true;
        m_createManager.GetComponent<CreateManager>().m_createWindFlag = false;
        
    }
}
