using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private GameObject m_fade;

    [SerializeField]
    private GameObject m_mainCamera;

    [SerializeField]
    private GameObject m_water;

    [SerializeField]
    private GameObject m_splash;

    [SerializeField]
    private GameObject m_bossObject;

    [SerializeField]
    private GameObject m_createManager;

    [SerializeField]
    private GameObject m_soundManager;

    [SerializeField]
    private GameObject m_right;

    [SerializeField]
    private GameObject m_left;

    [SerializeField]
    private GameObject[] m_hp;

    private Vector3 m_formerPosition;

    private Animator m_animator;

    private int m_desCount;

    [SerializeField]
    private float m_invincibleTime;

    private bool m_bubbleFlag = false;

    private bool m_splashFlag = true;

    private const int DEATH_COUNT_MAX = 2;

    private const int DAMAGE_SOUND = 4;

    private const int CHEACKPOINT_SOUND = 8;

    private const int WATER_SOUND = 9;

    private const int SWITCH_SOUND = 10;

    private const int DEATH_SOUND = 5;

    private const float BACK_TIME = 1.0f;

    private const float HOLE_POS_Y = -10.0f;
    //playerのスピード
    private const float MOVE_COUNT = 0.05f;

    private float MOVE_SOUND = 0.5f;



    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_formerPosition = this.transform.position;

    }

    void Update()
    {

        //落ちる前に保持したpositionをplayerに入れる
        if (this.transform.position.y <= HOLE_POS_Y)
        {
            this.transform.position = m_formerPosition;
            m_hp[m_desCount].SetActive(false);

            m_soundManager.GetComponent<SoundManage>().Sound(DAMAGE_SOUND,0);
            if (m_desCount == DEATH_COUNT_MAX)
            {
                m_animator.SetBool("Death", true);

                m_soundManager.GetComponent<SoundManage>().Sound(DEATH_SOUND,0);
                m_hp[DEATH_COUNT_MAX].SetActive(false);

            }
            m_desCount++;
        }


        if (m_fade.GetComponent<Fader>().IsFade() && m_fade.GetComponent<Fader>().IsFadeEnd())
        {
            m_fade.GetComponent<Fader>().FadeOut();
            m_animator.SetBool("GameClear", true);
            m_mainCamera.GetComponent<CameraManager>().Result();
        }
    }

    //playerの移動
    public void Move(float flip)
    {
        Vector3 playerMove = transform.position;
        playerMove.x += MOVE_COUNT * flip;
        transform.position = playerMove;

    }

    public void BubbleFlag()
    {
        m_bubbleFlag = false;
    }

    //playerの死亡アニメーションフラグ
    public void Expiration()
    {

        m_animator.SetBool("Death", true);

        m_soundManager.GetComponent<SoundManage>().Sound(DEATH_SOUND,0);

    }
    //無敵時間

    IEnumerator InvincibleTime()
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerDamage");


        yield return new WaitForSeconds(m_invincibleTime);

        gameObject.layer = LayerMask.NameToLayer("Player");
    }


    public void Water()
    {
        m_soundManager.GetComponent<SoundManage>().Sound(WATER_SOUND,1);
        m_water.GetComponent<EllipsoidParticleEmitter>().emit = false;
        m_bossObject.GetComponent<EnemyBoss>().Invincible();

        //m_water[1].GetComponent<EllipsoidParticleEmitter>().emit = false;
    }
    //public void Splash()
    //{
      
    //    m_splash.GetComponent<EllipsoidParticleEmitter>().emit = true;
    //    m_splashFlag = true;
    //}

    private void OnCollisionStay(Collision collision)
    {//ここ追加
        if (collision.gameObject.tag == "Switch")
        {
            Debug.Log(collision.gameObject.name);
            if (m_splashFlag)
            {
                // Invoke("EnemyBossHit()", 1f);
                EnemyBossHit();
                if (collision.gameObject.name == "LeftSwitch")
                {
                    Debug.Log("左");
                    m_soundManager.GetComponent<SoundManage>().Sound(SWITCH_SOUND,1);
                    m_water.GetComponent<EllipsoidParticleEmitter>().emit = true;
                    m_splash.GetComponent<EllipsoidParticleEmitter>().emit = false;
                    collision.gameObject.GetComponent<Animator>().speed = 1;
                    Invoke("Water", 0.6f);
                //    Invoke("Splash", 20);
                    m_splashFlag = false;
                }
                else
                {
                    Debug.Log("右");
                    m_soundManager.GetComponent<SoundManage>().Sound(SWITCH_SOUND, 1);
                    m_water.GetComponent<EllipsoidParticleEmitter>().emit = true;
                    m_splash.GetComponent<EllipsoidParticleEmitter>().emit = false;
                    collision.gameObject.GetComponent<Animator>().speed = 1;
                    Invoke("Water", 0.6f);
                   // Invoke("Splash", 20);
                    m_splashFlag = false;

                }
            }

        }
    }

    public void EnemyBossHit()
    {
        //  m_bossHitObject.GetComponent<Light>().enabled = true;
        m_bossObject.GetComponent<EnemyBoss>().Damage();
    }
    //リトライ時の倒れるモーション
    public void Down()
    {
        m_animator.SetBool("Death", false);
        m_animator.SetBool("DownUp", true);

        m_desCount = 0;
    }

    //playerのダメージをくらったときにHPを減少させる
    private void Damage()
    {

        m_left.GetComponent<CircleCollider2D>().enabled = false;
        m_right.GetComponent<CircleCollider2D>().enabled = false;

        m_soundManager.GetComponent<SoundManage>().Sound(DAMAGE_SOUND,0);
        m_hp[m_desCount].SetActive(false);
        m_desCount++;

    }

    //playerがダメージを食らってある程度したら操作できるようにする
    private void MoveReturn()
    {
        m_left.GetComponent<CircleCollider2D>().enabled = true;
        m_right.GetComponent<CircleCollider2D>().enabled = true;
    }
    //リザルトの表示に移行

    public void ContinueScene()
    {
        m_mainCamera.GetComponent<CameraManager>().Death();
    }
    //起き上がりモーション

    public void GetUp()
    {
        m_animator.SetBool("DownUp", false);
    }
    //playerのダメージモーション処理が終わってIdleモーションに戻る

    public void DamageEnd()
    {
        m_animator.SetBool("Damage", false);
    }

    //GameClearのポジションに移行

    public void GameClear()
    {
        iTween.Stop(gameObject);
        this.transform.eulerAngles = new Vector3(0, 90, 0);
    }
    //当たり判定関係


    public void EnemyBoss()
    {
        StartCoroutine("InvincibleTime");


        if (m_desCount == DEATH_COUNT_MAX)
        {
            m_animator.SetBool("Death", true);

            m_soundManager.GetComponent<SoundManage>().Sound(DEATH_SOUND,0);
            m_hp[DEATH_COUNT_MAX].SetActive(false);
        }
        else
        {
            m_animator.SetBool("Damage", true);
        }


        /* iTween.MoveTo(gameObject, iTween.Hash("position",
                                               transform.position - transform.forward,
                                               "time", BACK_TIME
             ));*/
    }
    private void OnTriggerStay(Collider collider)
    {
        //Bubbleと同じ動きをする
        if (collider.gameObject.tag == "Bubble" && collider.GetComponent<Bubble>().m_switchingObject.tag == "Player")
        {
            if (m_createManager.GetComponent<CreateManager>().m_createWindFlag)
            {
                Vector3 bubblePos = collider.transform.position;

                this.transform.position = new Vector3(bubblePos.x, bubblePos.y - 0.5f, bubblePos.z);

            }
        }
        else if (collider.gameObject.tag == "CheackPoint")
        {
            collider.GetComponent<BoxCollider>().enabled = false;

            m_soundManager.GetComponent<SoundManage>().Sound(CHEACKPOINT_SOUND,0);
            m_formerPosition = this.transform.position;

        }
        else if (collider.gameObject.tag == "Goal")
        {

            collider.transform.position = transform.position;

            m_fade.GetComponent<Fader>().FadeIn();

        }
    }

    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            StartCoroutine("InvincibleTime");


            if (m_desCount == DEATH_COUNT_MAX)
            {
                m_animator.SetBool("Death", true);

                m_soundManager.GetComponent<SoundManage>().Sound(DEATH_SOUND,0);
                m_hp[DEATH_COUNT_MAX].SetActive(false);
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

    private void OnTriggerEnter(Collider collider)
    {
        //Bubbleに当たった瞬間しかいらない処理
        if (collider.gameObject.tag == "Bubble"/* && collider.GetComponent<Bubble>().m_switchingObject.tag == "Player"*/)
        {
            if (m_createManager.GetComponent<CreateManager>().m_createWindFlag)
            {
                m_bubbleFlag = true;
                this.GetComponent<Rigidbody>().useGravity = false;

                transform.parent = collider.transform.parent;

            }
        }

        //if (collider.gameObject.tag == "EnemyBos")
        //{
        //    StartCoroutine("InvincibleTime");

        //    Debug.Log("colliderDisEnabled");

        //   // collider.gameObject.GetComponent<SphereCollider>().isTrigger = false;
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
        //}

    }

}
