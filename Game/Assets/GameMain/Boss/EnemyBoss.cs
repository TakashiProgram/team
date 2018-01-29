using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{

    [SerializeField]
    private GameObject m_player;

    private Animator m_animator;

    private Vector3 m_lockPos;

    private Vector3 m_pos;

    private Quaternion m_rotation;

    private int m_randomCount = 0;

    private bool m_animatorFlag = true;

    private bool m_lockFlag = false;

    private bool m_lockReleaseFlag = true;


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
        if (m_animatorFlag)
        {
            switch (m_randomCount)
            {
                case 0:
                    m_animator.SetBool("DashAttack", true);
                    m_animatorFlag = false;
                    break;

                case 1:

                    break;
            }
        }



        if (m_lockFlag)
        {
            if (m_lockReleaseFlag)
            {
                this.transform.LookAt(m_player.transform.position);
            }

            Invoke("LockRelease", 3);
            Invoke("AttackStart", 5);

        }


        if (this.transform.position == m_lockPos)
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
    //Enemyが止まってPlayerの方向に向く
    public void RockOn()
    {
        this.GetComponent<Animator>().speed = 0;
        m_lockFlag = true;
    }
    //体当たり攻撃を行う
    public void AttackStart()
    {

        iTween.MoveTo(gameObject, iTween.Hash("position",
                                                 m_lockPos,
                                                 "time", 5));

        this.GetComponent<Animator>().speed = 1;
        m_lockFlag = false;
        m_lockReleaseFlag = true;
    }
    public void Reselt()
    {
        m_animator.SetBool("DashAttack", false);
    }

    public void LockRelease()
    {
        m_lockPos = m_player.transform.position;
        m_lockPos.y = m_player.transform.position.y - 2;
        m_lockPos.z = -14;
        m_lockReleaseFlag = false;

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            // StartCoroutine("InvincibleTime");
            Debug.Log("efsddvc");

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

    }
}
