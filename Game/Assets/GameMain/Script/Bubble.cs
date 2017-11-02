using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {

    [SerializeField]
    private float m_setMove;

    [SerializeField]
    private int m_survivalTime;

    private GameObject m_createManager;

    private GameObject m_player;

    private const float BUBBLE_MOVE = 1.5f;

    private const int INVERTED = -1;
  

    void Start () {
        m_createManager = GameObject.Find("CreateManager");
        m_player = GameObject.Find("Player");

        Invoke("DestroyTime", m_survivalTime);
    }
	
	void Update () {
        this.transform.position+= m_createManager.GetComponent<CreateManager>().m_WingMove *
                                  BUBBLE_MOVE * INVERTED * Time.deltaTime;
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            Vector3 m_move = this.transform.position;
            m_move.y += m_setMove * Time.deltaTime;
            this.transform.position = m_move;
        }else
        {
            DestroyTime();
        }
    }
    private void DestroyTime()
    {
        m_player.transform.parent = null;
        m_player.GetComponent<Player>().m_bubbleFlag = false;

        m_player.GetComponent<Rigidbody>().useGravity = true;
        m_createManager.GetComponent<CreateManager>().m_windFlag = false;
        m_createManager.GetComponent<CreateManager>().m_WingMove = new Vector3(0,0,0);
        Destroy(gameObject);
    }
}
