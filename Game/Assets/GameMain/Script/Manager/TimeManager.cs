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
    private float m_Time;

    private float m_rotationTime;

    private float m_range = 0.0f;

    private float m_resetTime; 

    private const int TIME_MAX = 360;

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
        TimeResult();
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
        //無理やり止めている
        m_Time += Time.deltaTime;

        int wTime = (int)m_Time;
        //コンテニューした時のことを後で追加する
        if (wTime >= (int)ScoreRank.S)
        {
            Image[1].GetComponent<Text>().text = "ランク　S";
       
        }else if (m_Time<=(int)ScoreRank.S &&m_Time>=(int)ScoreRank.B)
        {
            Image[1].GetComponent<Text>().text = "ランク　A";
        }
        else 
        {

            Image[1].GetComponent<Text>().text = "ランク　B";
        }
        
        Image[0].GetComponent<Text>().text = wTime.ToString();
        Invoke("Tap", 1.0f);
    }
    public void Tap()
    {
        Image[2].SetActive(true);
    }
}
