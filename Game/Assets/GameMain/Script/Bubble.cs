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

    private const float BUBBLE_MOVE = 1.5f;

    private const int INVERTED = -1;

    private float m_floatingCount = 0;



    void Start () {
        m_createManager = GameObject.Find("CreateManager");
        m_player = GameObject.Find("Player");

        
        m_moveDisabled = m_setMove;
        Invoke("test", 3);
        m_createManager.GetComponent<CreateManager>().m_WingMove = new Vector3(0, 0, 0);
    }
	
	void Update () {
        this.transform.position+= m_createManager.GetComponent<CreateManager>().m_WingMove *
                                  BUBBLE_MOVE * INVERTED * Time.deltaTime;

        //上昇
        Vector3 move = this.transform.position;
        move.y += m_setMove * m_moveDisabled* m_floatingCount * Time.deltaTime;
        this.transform.position = move;
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag=="Player")
        {

            m_moveDisabled = 0;

           
        }
        else
        {
            m_moveDisabled = m_setMove;
            DestroyTime();
        }
    }
    public void DestroyTime()
    {
        m_player.transform.parent = null;
        m_player.GetComponent<Player>().m_bubbleFlag = false;

        m_player.GetComponent<Rigidbody>().useGravity = true;
        m_createManager.GetComponent<CreateManager>().m_windFlag = false;
        m_createManager.GetComponent<CreateManager>().m_WingMove = new Vector3(0,0,0);
        Destroy(gameObject);
    }
    public void te()
    {
        Invoke("DestroyTime", m_survivalTime);
    }

    private void test()
    {
        m_floatingCount = 1;
    }
}
