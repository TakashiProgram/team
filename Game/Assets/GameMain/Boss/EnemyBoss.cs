using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour {

    [SerializeField]
    private GameObject m_player;

    private Animator m_animator;

    private int m_randomCount = 0;

    private bool m_animatorFlag = true;

    private bool m_lockFlag = false;

	void Start () {
        //Invoke("Water", 2);
        m_animator = GetComponent<Animator>();
        RandomAttack();
    }
	
	void Update () {
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
            this.transform.LookAt(m_player.transform.position);
            Invoke("test", 3);
            
        }
        
    }
    //毎回違う攻撃を実行
    public void RandomAttack()
    {
        m_randomCount = Random.Range(0, 2);
        Debug.Log(m_randomCount);
    }
    //Enemyが止まってPlayerの方向に向く
    public void RockOn()
    {
        this.GetComponent<Animator>().speed = 0;
        m_lockFlag = true;
    }
    //体当たり攻撃を行う
    public void test()
    {
        this.GetComponent<Animator>().speed = 1;
        m_lockFlag = false;
    }
    public void Reselt()
    {
        m_animator.SetBool("DashAttack", false);
    }
}
