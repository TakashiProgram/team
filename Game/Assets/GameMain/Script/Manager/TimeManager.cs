using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour {

    [SerializeField]
    private GameObject m_timeLimit;

    [SerializeField]
    private GameObject m_player;

    [SerializeField]
    private GameObject m_result;

    [SerializeField]
    private GameObject m_clear;

    [SerializeField]
    private GameObject[] m_image;

    [SerializeField]
    private Sprite[] m_rank;

    [SerializeField]
    private Sprite[] m_numbers;

    private int m_digitUp = 1;

    private int m_rankTime;

    [SerializeField]
    private float m_Time;

    private float m_rotationTime;

    private float m_range = 0.0f;

    private float m_resetTime;

    private bool m_fixed = true;

    private const int TIME_MAX = 360;

    private const int RANK_COUNT = 3;

    private const int TAP_COUNT = 4;

    private StageData resources;

    private enum ScoreRank
    {
        S =100,
        A,
        B =49
    }

    private enum Rank
    {
        S=0,
        A,
        B
    }


    void Start () {
        m_resetTime = m_Time;
        m_rotationTime = TIME_MAX / m_Time;
        
        resources = Resources.Load<StageData>("StagesData");
    }
	
	void Update () {
        Disable();
        if (m_Time<=0)
        {
            m_Time = 0;
            m_range = 0;
            m_player.GetComponent<Player>().Expiration();
        }
    }
    private void Disable()
    {
        m_range = m_range + (m_rotationTime * Time.deltaTime);
        m_timeLimit.transform.rotation = Quaternion.Euler(0,0, -m_range);
        m_Time -= Time.deltaTime;

    }
    public void TimeReset()
    {
        m_Time = m_resetTime;
        m_range = 0;
    }
    public void TimeResult()
    {
        m_result.GetComponent<ResultStop>().Display();
        //無理やり止めている
        //後で変更
        m_Time += Time.deltaTime;

        m_rankTime = (int)m_Time;

      
        //コンテニューした時のことを後で追加する
        if (m_fixed)
        {
            if (m_rankTime >= (int)ScoreRank.S)
            {
                m_image[RANK_COUNT].GetComponent<Image>().sprite = m_rank[(int)Rank.S];

                resources.SetClearRank(SceneManager.GetActiveScene().name, ClearRank.rank_S);

            } else if (m_Time <= (int)ScoreRank.S && m_Time >= (int)ScoreRank.B)
            {
                m_image[RANK_COUNT].GetComponent<Image>().sprite = m_rank[(int)Rank.A];

                    resources.SetClearRank(SceneManager.GetActiveScene().name, ClearRank.rank_A);
            }
            else
            {
                m_image[RANK_COUNT].GetComponent<Image>().sprite = m_rank[(int)Rank.B];

                  resources.SetClearRank(SceneManager.GetActiveScene().name, ClearRank.rank_B);
            }
        }
        else
        {
            m_image[RANK_COUNT].GetComponent<Image>().sprite = m_rank[(int)Rank.B];

               resources.SetClearRank(SceneManager.GetActiveScene().name, ClearRank.rank_B);
        }
        for (int i = 0; i < 3; i++)
        {
           
           int r= m_rankTime / m_digitUp;

            r = r % 10;
            m_image[i].GetComponent<Image>().sprite = m_numbers[r];
            m_digitUp = m_digitUp * 10;

        }
        
        Invoke("Tap", 1.0f);
    }
    public void Tap()
    {
        m_clear.SetActive(true);
        m_image[TAP_COUNT].SetActive(true);
    }
    public void RankFixed()
    {
        m_fixed = false;
    }
}
