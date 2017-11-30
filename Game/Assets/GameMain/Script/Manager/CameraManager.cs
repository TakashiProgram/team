using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    [SerializeField]
    private GameObject m_continueUI;

    [SerializeField]
    private GameObject m_DefaultUI;

    [SerializeField]
    private GameObject m_player;

    [SerializeField]
    private Vector2 m_playerPosMin;

    [SerializeField]
    private Vector2 m_playerPosMax;

    private bool m_switchingFlag = true;

    private bool m_zoomFlag = true;

    //playerのz軸を変更しない
    private const float FIXED = -4.75f;

    private const float ZOOM_FIXED = -1.65f;

    private const float ZOOM_POS_Y = 0.5f;
    //playerの高さを調整
    private const int SET_POS_Y = 2;

    private const float CONTINUE_TIME = 2.0f;

    private readonly Vector3 CUNTINUE_SCALE = new Vector3(1.0f, 1.0f, 1.0f);
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
                iTween.MoveTo(gameObject, iTween.Hash(
                              "position", new Vector3(this.transform.position.x,this.transform.position.y- ZOOM_POS_Y, ZOOM_FIXED)));

                Invoke("Cuntinue", CONTINUE_TIME);
                m_zoomFlag = false;
            }
        }
    }
    public void Death()
    {
        m_switchingFlag = false;
    }
    public void Cuntinue()
    {

        iTween.ScaleTo(m_continueUI, iTween.Hash("scale", CUNTINUE_SCALE));
    }
    public void Resurrection()
    {

    }
}
