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

    private GameObject rrr;

    private Vector3 m_move;

    [SerializeField]
    private  float m_bubbleMove;

    private const int INVERTED = -1;

    private const int RISING_TIME = 3;

    private float m_floatingCount = 0;
    
    void Start () {
        m_createManager = GameObject.Find("CreateManager");
        m_player = GameObject.Find("Player");

        // testobj = GameObject.Find("testobj");
        //  transform.parent = GameObject.Find("t").transform;

        transform.parent = GameObject.Find("BubbleStart").transform;
        //this.GetComponent<Collider>().isTrigger = false;
        //   m_moveDisabled = m_setMove;
        Invoke("Rising", RISING_TIME);
        m_createManager.GetComponent<CreateManager>().m_WingMove = new Vector3(0, 0, 0);
    }
	
	void Update () {
        m_move = m_createManager.GetComponent<CreateManager>().m_WingMove;
        //風によって動く方向が変わる
        this.transform.parent.position += m_move * m_bubbleMove * INVERTED * Time.deltaTime;
        
        // this.gameObject.GetComponent<Rigidbody>().velocity= m_move * BUBBLE_MOVE * INVERTED;

        //上昇
        //Vector3 move = this.transform.position;
        //   move.y += /*m_setMove **/ m_moveDisabled* m_floatingCount * Time.deltaTime;
        //  move.y = 1 * Time.deltaTime;
        //this.transform.position = move;

        this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0,
                                                                         m_floatingCount,
                                                                         0);

    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            if (m_createManager.GetComponent<CreateManager>().m_createWindFlag)
            {

                m_moveDisabled = 0;
              //  m_player.transform.position += m_move * m_bubbleMove * INVERTED * Time.deltaTime;
            }
            else
            {
                //渋谷君がきてから
                //foreach (ContactPoint point in collision.contacts)
                //{
                //    //w要素は1にしておく
                //    Vector4 hitpos = point.point;
                //    hitpos.w = 1;
                //    gameObject.transform.GetComponent<BubbleController>().SetVector("_HitPosition", hitpos);

                //}
                Death();
            }
            

        }
       else if(collision.gameObject.tag == "Enemy")
        {
        }else
        {
            m_moveDisabled = m_setMove;
            //foreach (ContactPoint point in collision.contacts)
            //{
            //    //w要素は1にしておく
            //    Vector4 hitpos = point.point;
            //    hitpos.w = 1;
            //    gameObject.transform.GetComponent<BubbleController>().SetVector("_HitPosition", hitpos);

            //}
            Death();
        }
    }

    public void Death()
    {
        m_player.transform.parent = null;
        m_player.GetComponent<Player>().m_bubbleFlag = false;

        m_player.GetComponent<Rigidbody>().useGravity = true;
        m_createManager.GetComponent<CreateManager>().m_createWindFlag = false;
         Destroy(gameObject);

        
        this.GetComponent<BubbleController>().Burst(0.2f);
    }
    public void DestroyTime()
    {
        
        Invoke("Death", m_survivalTime);
    }

    private void Rising()
    {
        m_floatingCount =0.5f;
    }
}
