using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool m_bubbleFlag = false;

    private Animator m_animator;

    [SerializeField]
    private GameObject m_right;
    [SerializeField]
    private GameObject m_left;

    [SerializeField]
    private GameObject[] m_hp;

    [SerializeField]
    private GameObject m_createManager;

    [SerializeField]
    private GameObject m_bubblePos;

    [SerializeField]
    private float INVINCIBLE_TIME;

    private int m_desCount;

    private const int DES_COUNT_MAX = 2;

    private const float BACK_TIME = 1.0f;

    private Vector3 m_formerPosition;

    private GameObject rrr;
    void Start()
    {
       m_animator= GetComponent<Animator>();
       
    }
    
    void Update()
    {
        Debug.Log(m_formerPosition);
       // Debug.Log(this.transform.position + "player");
        if (this.transform.position.y<-10)
        {
            this.transform.position = m_formerPosition;
        }

        if (Physics.CheckSphere(transform.position, 10))
        {
            //audioSource.Play();
         //   Debug.Log("当たっている");
        }else
        {
           // Debug.Log("離れている");
        }
    }

    //playerのダメージをくらったときに呼ばれる
    private void Damage()
    {
        if (m_desCount == DES_COUNT_MAX)
        {

            Destroy(gameObject);
        }
        m_left.GetComponent<CircleCollider2D>().enabled = false;
        m_right.GetComponent<CircleCollider2D>().enabled = false;

        Destroy(m_hp[m_desCount]);
        m_desCount++;
        
    }

    //playerのダメージ処理が終わった時に呼ばれる
   private void MoveReturn()
    {
        m_left.GetComponent<CircleCollider2D>().enabled = true;
        m_right.GetComponent<CircleCollider2D>().enabled = true;
    }

    //playerのダメージモーション処理が終わって呼ばれる
    void DamageEnd()
    {
        
        m_animator.SetBool("Damage", false);
        
    }
   
    private void OnCollisionEnter(Collision collision)
    {
        //ノックバック
        if (collision.gameObject.tag=="Enemy")
        {
            StartCoroutine("CreateCube");

            m_animator.SetBool("Damage", true);

            iTween.MoveTo(gameObject, iTween.Hash("position", 
                                                  transform.position - transform.forward,
                                                  "time", BACK_TIME
                ));

        }
    }

    private void OnTriggerEnter(Collider collision)
    {//Bubbleとの親子関係
        if (collision.gameObject.tag == "Bubble")
        {
            if (m_createManager.GetComponent<CreateManager>().m_createWindFlag)
            {
                m_formerPosition = this.transform.position;
                m_bubblePos.transform.GetComponent<test>().isEnable = false;
                m_bubbleFlag = true;
                this.GetComponent<Rigidbody>().useGravity = false;
                transform.parent = GameObject.Find("BubbleStart").transform;
                //this.transform.position = GameObject.Find("Bubble(Clone)").transform.position;

            }

        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Bubble")
        {
            if (m_createManager.GetComponent<CreateManager>().m_createWindFlag)
            {
               // m_formerPosition = this.transform.position;
                Vector3 tet = GameObject.Find("Bubble(Clone)").transform.position;
                Vector3 localpos = this.transform.localPosition;
                // this.transform.position = GameObject.Find("Bubble(Clone)").transform.position;
                this.transform.position = new Vector3(tet.x, tet.y - 0.5f, tet.z);
                //this.transform.position = this.transform.localPosition;
            } 
        }
    }

    private void OnCollisionExit(Collider collision)
    {
        if (collision.gameObject.tag == "Bubble")
        {
            m_bubblePos.transform.GetComponent<test>().isEnable = true;
        }
        }

    public void Resurrection(Vector3 pos)
    {
        m_formerPosition = pos;
    }

    //無敵時間
    IEnumerator CreateCube()
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
       
       
            yield return new WaitForSeconds(INVINCIBLE_TIME);
        
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
}
