using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour {

    //全体のUI
    [SerializeField]
    private GameObject m_DefaultUI;
    
    //コンテニュー画面のUI
    [SerializeField]
    private GameObject m_continueUI;
    
    //コンテニュー選択するかどうかのUI
    [SerializeField]
    private GameObject m_decisionUI;

    [SerializeField]
    private GameObject m_player;

    [SerializeField]
    private GameObject m_timeManager;

    //HP表示のUI
    [SerializeField]
    private GameObject[] m_hpUI;

    [SerializeField]
    private Vector2 m_playerPosMin;

    [SerializeField]
    private Vector2 m_playerPosMax;

    [SerializeField]
    private Vector2[] m_playerPosMintest;

    [SerializeField]
    private Vector2[] m_playerPosMaxtest;

    [SerializeField]
    private ParticleSystem m_particle;

    private int m_continueCount = 0;

    private bool m_switchingFlag = true;

    private bool m_zoomFlag = true;

    private bool m_stop = true;

    //playerのz軸を変更しない
    private const float FIXED = -4.75f;

    private const float ZOOM_FIXED = -1.65f;

    private const float ZOOM_POS_Y = 0.5f;
    //playerの高さを調整
    private const int SET_POS_Y = 2;

    private const int CONTINUE_MAX = 2;

    private const float RESET_TIME = 3;

    private const float CONTINUE_TIME = 2.0f;

    private readonly Vector3 CUNTINUE_SCALE = new Vector3(1.0f, 1.0f, 1.0f);

    private readonly Vector3 RESULT_POS = new Vector3(-13.68f, -1.14f, 0.67f);

    void Start () {
        //最初にカメラがプレイヤーに付いていく(デバック用)
        this.transform.position = new Vector3(m_player.transform.position.x, m_player.transform.position.y, FIXED);
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
        }
    }
    //カメラ切り替え
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
    //Result表示
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
    //何回かコンテニューしたらさせないようにする
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

        m_player.transform.position = RESULT_POS;

        m_DefaultUI.SetActive(false);
        this.GetComponent<Animator>().enabled = true;
         m_switchingFlag = false;
         m_zoomFlag = false;

    }
    //resultカメラ用
    public void AnimatorStop()
    {
        this.GetComponent<Animator>().speed = 0;
        if (m_stop)
        {
            m_particle.GetComponent<ParticleSystem>().Play();
            Invoke("ResetSpeed", RESET_TIME);
            m_stop = false;
        }
      
    }
    //アニメーションが再生し始める
    public void ResetSpeed()
    {
        this.GetComponent<Animator>().speed = 1;
    }

    private void ResultMove()
    {
        m_timeManager.GetComponent<TimeManager>().TimeResult();
    }
}
