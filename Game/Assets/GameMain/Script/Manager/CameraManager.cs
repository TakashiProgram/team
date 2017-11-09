using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    
    [SerializeField]
    private Vector2 m_playerPosMin;

    [SerializeField]
    private Vector2 m_playerPosMax;

    [SerializeField]
    private GameObject m_player;

    private const float FIXED = -4.75f;

    private const int SET_POS_Y = 2;
   
    void Start () {
        //最初にカメラがプレイヤーに付いていく(デバック用)
        this.transform.position = new Vector3(m_player.transform.position.x, m_player.transform.position.y, FIXED);

    }
	
	void Update () {
        //カメラにplayerが付いてくる
        this.transform.position = new Vector3(m_player.transform.position.x, m_player.transform.position.y, FIXED);
        //カメラが範囲外になったら止める
        this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, m_playerPosMin.x, m_playerPosMax.x),
                                             Mathf.Clamp(this.transform.position.y + SET_POS_Y, m_playerPosMin.y, m_playerPosMax.y), FIXED);
	
    }
}
