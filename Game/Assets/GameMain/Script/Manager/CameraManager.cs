using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    //コンテニュー画面のUI
    [SerializeField]
    private GameObject m_continueUI;
    //コンテニュー選択するかどうかのUI
    [SerializeField]
    private GameObject m_decisionUI;
    //全体のUI
    [SerializeField]
    private GameObject m_DefaultUI;
    //HP表示のUI
    [SerializeField]
    private GameObject[] m_hpUI;

    [SerializeField]
    private GameObject m_player;

    [SerializeField]
    private GameObject m_timeManager;

    [SerializeField]
    private Vector2 m_playerPosMin;

    [SerializeField]
    private Vector2 m_playerPosMax;

    [SerializeField]
    private ParticleSystem particle;

    private bool m_switchingFlag = true;

    private bool m_zoomFlag = true;

    //playerのz軸を変更しない
    private const float FIXED = -4.75f;

    private const float ZOOM_FIXED = -1.65f;

    private const float ZOOM_POS_Y = 0.5f;
    //playerの高さを調整
    private const int SET_POS_Y = 2;

    private int m_continueCount = 0;

    private const int CONTINUE_MAX = 2;

    private const float CONTINUE_TIME = 2.0f;

    private readonly Vector3 CUNTINUE_SCALE = new Vector3(1.0f, 1.0f, 1.0f);

    private bool stop = true;
    
    void Start () {
        //最初にカメラがプレイヤーに付いていく(デバック用)
        this.transform.position = new Vector3(m_player.transform.position.x, m_player.transform.position.y, FIXED);
        //m_switchingFlag = false;
        //m_zoomFlag = false;
    }
	
	void Update () {
        if (m_switchingFlag)
        {
            //カメラにplayerが付いてくる
            this.transform.position = new Vector3(m_player.transform.position.x, m_player.transform.position.y, FIXED);
            //カメラが範囲外になったら止める
            this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, m_playerPosMin.x, m_playerPosMax.x),
                                                 Mathf.Clamp(this.transform.position.y + SET_POS_Y, m_playerPosMin.y, m_playerPosMax.y), FIXED);

          
        } else{
            if (m_zoomFlag)
            {
                
                m_DefaultUI.SetActive(false);
                //カメラをplayerに寄せる
                iTween.MoveTo(gameObject, iTween.Hash(
                              "position", new Vector3(this.transform.position.x,this.transform.position.y- ZOOM_POS_Y, ZOOM_FIXED)));

                Invoke("Cuntinue", CONTINUE_TIME);
                m_zoomFlag = false;
            }
         //   m_timeManager.GetComponent<TimeManager>().m_Time = 300;
        }
    }
    public void Death()
    {
        m_switchingFlag = false;
        m_continueCount++;
    }

    //Contione表示
    public void Cuntinue()
    {
        iTween.ScaleTo(m_continueUI, iTween.Hash("scale", CUNTINUE_SCALE));
        m_timeManager.GetComponent<TimeManager>().RankFixed();
    }

    public void Resurrection()
    {
     iTween.ScaleTo(m_continueUI, iTween.Hash("scale", new Vector3(0.0f,0.0f,0.0f)));
        m_switchingFlag = true;
       
       
        for (int i = 0; i < 3; i++)
        {
            m_hpUI[i].SetActive(true);
        }
         m_DefaultUI.SetActive(true);
        m_timeManager.GetComponent<TimeManager>().TimeReset();
    }

    public void End()
    {
        m_zoomFlag = true;
        if (m_continueCount == CONTINUE_MAX)
        {
            m_decisionUI.GetComponent<BoxCollider2D>().enabled = false;
            m_decisionUI.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f,0.5f);
        }
    }

    public void Result()
    {
        Debug.Log("ewsrgdhj");
       // this.transform.position = new Vector3(-13.24f,0.75f,-4.75f);
        m_player.transform.position = new Vector3(-13.68f, -1.24f, 0);
      //  m_player.transform.localRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        
        m_DefaultUI.SetActive(false);
        this.GetComponent<Animator>().enabled = true;
        //this.GetComponent<CameraManager>().enabled = false;
         m_switchingFlag = false;
         m_zoomFlag = false;

    }
    //resultカメラ用
    public void AnimatorStop()
    {
        this.GetComponent<Animator>().speed = 0;
        if (stop)
        {
            particle.GetComponent<ParticleSystem>().Play();
            Invoke("test", 3f);
            stop = false;
        }
      
    }
    public void test()
    {
        this.GetComponent<Animator>().speed = 1;
    }
    private void ResultMove()
    {
        m_timeManager.GetComponent<TimeManager>().TimeResult();
    }
}
