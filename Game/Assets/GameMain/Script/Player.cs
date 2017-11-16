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
    private GameObject m_mainCamera;

    [SerializeField]
    private GameObject m_bubblePos;
    

    [SerializeField]
    private float m_invincibleTime;

    private int m_desCount;

    private Vector3 m_formerPosition;

    private const int DEATH_COUNT_MAX = 2; 

    private const float BACK_TIME = 1.0f;
    
    void Start()
    {
       m_animator= GetComponent<Animator>();
       
    }
    
    void Update()
    {
        //落ちる前に保持したpositionをplayerに入れる
        if (this.transform.position.y<=-10)
        {
            this.transform.position = m_formerPosition;
        }
    }

    //playerのダメージをくらったときにHPを減少させて
    //0になったらplayerは死ぬ
    private void Damage()
    {
        
        m_left.GetComponent<CircleCollider2D>().enabled = false;
        m_right.GetComponent<CircleCollider2D>().enabled = false;

        Destroy(m_hp[m_desCount]);
        m_desCount++;

    }

    //playerがダメージを食らってある程度したら操作できるようにする
   private void MoveReturn()
    {
        m_left.GetComponent<CircleCollider2D>().enabled = true;
        m_right.GetComponent<CircleCollider2D>().enabled = true;
    }

    //playerのダメージモーション処理が終わってIdleモーションに戻る
    void DamageEnd()
    {
     
            m_animator.SetBool("Damage", false);
    }
   

    private void OnTriggerEnter(Collider collision)
    {
        //Bubbleに当たった瞬間しかいらない処理
        if (collision.gameObject.tag == "Bubble")
        {
            if (m_createManager.GetComponent<CreateManager>().m_createWindFlag)
            {
                m_formerPosition = this.transform.position;
                
                m_bubbleFlag = true;
                this.GetComponent<Rigidbody>().useGravity = false;
                transform.parent = GameObject.Find("BubbleStart").transform;

            }

        }
        if (collision.gameObject.tag == "Enemy")
        {
            StartCoroutine("CreateCube");


            if (m_desCount == DEATH_COUNT_MAX)
            {
                m_animator.SetBool("Death", true);
                m_mainCamera.GetComponent<CameraManager>().Death();
                Destroy(m_hp[DEATH_COUNT_MAX]);
                
            }
            else
            {
                m_animator.SetBool("Damage", true);
            }
            

            iTween.MoveTo(gameObject, iTween.Hash("position",
                                                  transform.position - transform.forward,
                                                  "time", BACK_TIME
                ));

        }
    }

    private void OnTriggerStay(Collider collision)
    {
        //Bubbleと同じ動きをする
        if (collision.gameObject.tag == "Bubble")
        {
            if (m_createManager.GetComponent<CreateManager>().m_createWindFlag)
            {

                Vector3 bubblePos = GameObject.Find("Bubble").transform.position;

                this.transform.position = new Vector3(bubblePos.x, bubblePos.y - 0.5f, bubblePos.z);
            } 
        }
    }


    //地面を離れる前に今いるpositionを保持する
    public void Resurrection(Vector3 pos)
    {
        m_formerPosition = pos;
    }

    //無敵時間
    IEnumerator CreateCube()
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
       
       
            yield return new WaitForSeconds(m_invincibleTime);
        
        gameObject.layer = LayerMask.NameToLayer("Player");
    }


}
