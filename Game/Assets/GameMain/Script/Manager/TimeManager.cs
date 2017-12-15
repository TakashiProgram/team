using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    [SerializeField]
    private GameObject m_timeLimit;

    [SerializeField]
    private GameObject m_player;

    [SerializeField]
    private GameObject[] Image;

    [SerializeField]
    private GameObject m_result;

    [SerializeField]
    private Sprite[] Rank;

    [SerializeField]
    private Sprite[] Numbers;

    [SerializeField]
    private float m_Time;

    private float m_rotationTime;

    private float m_range = 0.0f;

    private float m_resetTime;

    private int m_digitUp = 1;

    private int wTime;

    private const int TIME_MAX = 360;

    private bool m_fixed = true;

    private enum ScoreRank
    {
        S =100,
        A,
        B =49
    }


    void Start () {
        m_resetTime = m_Time;
        m_rotationTime = TIME_MAX / m_Time;
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

         wTime = (int)m_Time;

       // printf("%d", wTime / 100);
       
        //コンテニューした時のことを後で追加する
        if (m_fixed)
        {
            if (wTime >= (int)ScoreRank.S)
            {
                Image[3].GetComponent<Image>().sprite = Rank[0];

            } else if (m_Time <= (int)ScoreRank.S && m_Time >= (int)ScoreRank.B)
            {
                Image[3].GetComponent<Image>().sprite = Rank[1];
            }
            else
            {
                Image[3].GetComponent<Image>().sprite = Rank[2];
            }
        }else
        {
            Image[3].GetComponent<Image>().sprite = Rank[2];
        }
        for (int i = 0; i < 3; i++)
        {
           
           int r= wTime / m_digitUp;

            r = r % 10;
            Image[i].GetComponent<Image>().sprite = Numbers[r];
            m_digitUp = m_digitUp * 10;

        }
        //クリアランク表示
       // Image[0].GetComponent<Image>().sprite = wTime.ToString();
        Invoke("Tap", 1.0f);
    }
    public void Tap()
    {
        Image[4].SetActive(true);
    }
    public void RankFixed()
    {
        m_fixed = false;
    }
}
