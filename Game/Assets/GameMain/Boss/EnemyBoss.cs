using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{

    [SerializeField]
    private GameObject m_player;

    [SerializeField]
    private GameObject m_goal;

    private GameObject m_material;

    private Animator m_animator;

    private Vector3 m_lockPos;

    private Vector3 m_pos;

    private Quaternion m_rotation;

    private int m_randomCount = 0;

    private int m_enemyHp = 3;

    private bool m_animatorFlag = true;

    private bool m_lockFlag = false;

    private bool m_lockReleaseFlag = true;

    private bool m_damageFlag = false;

    private bool m_attackFlag = false;

    private float m_vector = 0;

    private Vector3 t;

    private Vector3 tr;

    private float m_color=1;

    private float m_reversal=0.01f;

    private float m_moveColor;
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_randomCount = 1;
        m_pos = this.transform.position;

        m_rotation = this.transform.rotation;

        m_material = transform.Find("StoneMonster").gameObject;
    }

    void Update()
    {
        Debug.Log(m_reversal);
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
                  //  Invoke("Idle", 5);

                    break;
            }
        }
        switch (m_enemyHp)
        {
            case 0:
                m_material.GetComponent<SkinnedMeshRenderer>().material.color = new Color(1, m_color, m_color);

                m_color += m_reversal;
                if (m_color > 1)
                {
                    Debug.Log("fedd");
                    m_reversal = m_reversal * -1;
                   // m_reversal += m_moveColor;
                    //  m_color -= 0.01f;
                }
                if (m_color < 0)
                {
                    m_reversal = m_reversal * -1;
                   // m_reversal += m_moveColor;
                    Debug.Log("fedd");
                    //m_color += 0.01f;
                }
                break;

            case 1:
                m_material.GetComponent<SkinnedMeshRenderer>().material.color = new Color(1, m_color, m_color);

                m_color += m_reversal;
                if (m_color > 1)
                {
                    Debug.Log("fedd");
                    m_reversal = m_reversal * -1;
                   // m_reversal += m_moveColor;
                    //  m_color -= 0.01f;
                }
                if (m_color < 0)
                {
                    m_reversal = m_reversal * -1;
                 //   m_reversal += m_moveColor;
                    Debug.Log("fedd");
                    //m_color += 0.01f;
                }
                break;

            case 2:
                m_material.GetComponent<SkinnedMeshRenderer>().material.color = new Color(1, m_color, m_color);

                m_color += m_reversal/*+ m_moveColor*/;
                if (m_color > 1)
                {
                    Debug.Log("fedd");
                    m_reversal = m_reversal * -1;
                 //   m_reversal += m_moveColor;
                    //  m_color -= 0.01f;
                }
                if (m_color < 0)
                {
                    m_reversal = m_reversal * -1;
                 //   m_reversal += m_moveColor;
                    Debug.Log("fedd");
                    //m_color += 0.01f;
                }
               // m_moveColor = -0.02f;
                break;

        }


        if (m_lockFlag)
        {
            this.transform.LookAt(m_player.transform.position);
            Invoke("AttackStart", 5);
       
        }else if (m_attackFlag)
        {
            t = this.transform.position;
            t.x = m_lockPos.x;
            t.y = m_lockPos.y;
            t.z -= 0.5f;
            this.transform.position = t;
           
            m_attackFlag = false;

        }//噛みつき攻撃をした後に元のポジションに持っていく
        else if (this.transform.position.z <m_lockPos.z-10)
        {
            m_pos.y = 14;
            this.transform.position = m_pos;
            this.transform.rotation = m_rotation;
            m_pos.y = -1.4f;
            
            iTween.MoveTo(gameObject, iTween.Hash("position",
                                                 m_pos,
                                                 "time", 10));
            Invoke("RandomBehavior", 10);


        }else if (m_damageFlag)
        {

            if (this.transform.position.y < m_pos.y+1.4f)
            {
                m_animator.SetBool("Damage", true);
                m_enemyHp--;
                // m_moveColor-=0.01f;
                m_reversal+=0.01f; 
                Debug.Log(m_enemyHp);
                if (m_enemyHp <= 0)
                {
                    m_animator.SetBool("Damage", false);
                    m_animator.SetBool("Death", true);
                   // 


                }
                Invincible();
            }
        }

    }
   
    //Idle状態からAttackに移動
    private void Idle()
    {
        m_randomCount = 0;
    }

    //Enemyが止まってPlayerの方向に向く
    //Attackアニメーション
    private void RockOn()
    {
        this.GetComponent<Animator>().speed = 0;
        m_lockFlag = true;
        Invoke("LockRelease", 3);
       
    }
    //Attackアニメーション
    private void Reselt()
    {
        m_animator.SetBool("DashAttack", false);
    }
    //Damageアニメーション
    private void DmageStop()
    {
        m_animator.SetBool("Damage", false);
    }
    public void Damage()
    {
        m_damageFlag = true;
    }
    public void Invincible()
    {
        m_damageFlag = false;
    }

    private void LockRelease()
    {
        if (m_lockReleaseFlag)
        {
            m_lockPos = m_player.transform.position;

            m_lockPos.y = m_player.transform.position.y - 2;
            m_lockPos.z = -14;
            m_lockReleaseFlag = false;
            m_lockFlag = false;
        }
    }
    //体当たり攻撃を行う
    private void AttackStart()
    {
        m_attackFlag = true;

        this.GetComponent<Animator>().speed = 1;
       
    }
    //攻撃した後に次の攻撃は何をするかを考える
    private void RandomBehavior()
    {
        m_randomCount = Random.Range(0, 2);
        m_animatorFlag = true;
    }

    private void End()
    {
        m_goal.SetActive(true);
    }

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
