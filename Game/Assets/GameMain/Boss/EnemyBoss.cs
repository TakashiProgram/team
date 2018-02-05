using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{

    [SerializeField]
    private GameObject m_player;

    [SerializeField]
    private GameObject m_goal;

    private Animator m_animator;

    private Vector3 m_lockPos;

    private Vector3 m_pos;

    private Quaternion m_rotation;

    private int m_randomCount = 0;

    private int m_enemyHp = 3;

    private bool m_animatorFlag = true;

    private bool m_lockFlag = false;

    private bool m_lockReleaseFlag = true;

    private bool m_testFlag = false;

    private bool m_attackFlag = false;

    private float m_vector = 0;

    private Vector3 t;

    private Vector3 tr;

    void Start()
    {
        //Invoke("Water", 2);
        m_animator = GetComponent<Animator>();
        RandomAttack();
        m_pos = this.transform.position;

        m_rotation = this.transform.rotation;
    }

    void Update()
    {
        Debug.Log(m_randomCount);
        Debug.Log(m_pos);
        if (m_animatorFlag)
        {
            switch (m_randomCount)
            {
                case 0:
                    m_animator.SetBool("DashAttack", true);
                    m_lockReleaseFlag = true;
                    m_animatorFlag = false;
                    break;

                case 1:
                 //   Invoke("IdleAttack", 5);

                    break;
            }
        }



        if (m_lockFlag)
        {
            if (m_lockReleaseFlag)
            {

                this.transform.LookAt(m_player.transform.position);
                Invoke("LockRelease", 3);
                Invoke("AttackStart", 5);

            }


        }
        if (m_attackFlag)
        {
            t = this.transform.position;
            t.x = m_lockPos.x;
            t.y = m_lockPos.y;
            t.z -= 0.5f;
            this.transform.position = t;
            //this.transform.position += new Vector3(m_lockPos.x,
            //                                       m_lockPos.y,
            //                                       m_lockPos.z - m_vector);

            //this.transform.position += new Vector3(6.86f,
            //                                      -2.94f,
            //                                      10.49f + m_vector);
            //  m_vector+=0.01f;
            m_attackFlag = false;
            
        }

        if (this.transform.position.z <m_lockPos.z-10)
        {

            
            m_pos.y = 14;
            this.transform.position = m_pos;
            this.transform.rotation = m_rotation;
            m_pos.y = -1.4f;
            
            iTween.MoveTo(gameObject, iTween.Hash("position",
                                                 m_pos,
                                                 "time", 10));
            Invoke("RandomBehavior", 10);


        }
        if (m_testFlag)
        {

            if (this.transform.position.y < m_pos.y+1.4f)
            {
                m_animator.SetBool("Damage", true);
                m_enemyHp--;
                Debug.Log(m_enemyHp);
                if (m_enemyHp <= 0)
                {
                    m_animator.SetBool("Damage", false);
                    m_animator.SetBool("Death", true);
                    m_goal.SetActive(true);


                }
                test2();
            }
        }

    }
    public void RandomBehavior()
    {
        m_randomCount = Random.Range(0, 2);
        m_animatorFlag = true;
    }
    //毎回違う攻撃を実行
    public void RandomAttack()
    {
        m_randomCount = Random.Range(0, 2);


    }
    public void IdleAttack()
    {
        m_randomCount = 0;
    }
    //Enemyが止まってPlayerの方向に向く
    public void RockOn()
    {
        this.GetComponent<Animator>().speed = 0;
        m_lockFlag = true;
    }
    //体当たり攻撃を行う
    public void AttackStart()
    {
        m_attackFlag = true;
       
        this.GetComponent<Animator>().speed = 1;
        m_lockFlag = false;
        //m_lockReleaseFlag = true;
    }
    public void Reselt()
    {
        m_animator.SetBool("DashAttack", false);
    }

    public void LockRelease()
    {
        if (m_lockReleaseFlag)
        {
            m_lockPos = m_player.transform.position;

            m_lockPos.y = m_player.transform.position.y - 2;
            m_lockPos.z = -14;
            m_lockReleaseFlag = false;
        }
       

    }

    public void test()
    {
        m_testFlag = true;
    }
    public void test2()
    {
        m_testFlag = false;
    }
    public void DmageStop()
    {
        m_animator.SetBool("Damage", false);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Move")
    //    {
    //        Debug.Log("fdsdv");
    //    }
    //}
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            // StartCoroutine("InvincibleTime");
            // this.GetComponent<SphereCollider>().enabled = false;

            m_player.GetComponent<Player>().EnemyBoss();

            //    if (m_desCount == DEATH_COUNT_MAX)
            //    {
            //        m_animator.SetBool("Death", true);

            //        m_soundManager.GetComponent<SoundManage>().sound(DEATH_SOUND);
            //        m_hp[DEATH_COUNT_MAX].SetActive(false);
            //    }
            //    else
            //    {
            //        m_animator.SetBool("Damage", true);
            //    }


            //    iTween.MoveTo(gameObject, iTween.Hash("position",
            //                                          transform.position - transform.forward,
            //                                          "time", BACK_TIME
            //        ));
        }
        //if (collider.gameObject.tag == "Move")
        //{
        //    Debug.Log("fdsdv");
        //}

    }
}
