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

    void Start () {
        this.transform.position = new Vector3(m_player.transform.position.x, m_player.transform.position.y, FIXED);
    }
	
	// Update is called once per frame8.85
	void Update () {
        this.transform.position = new Vector3(m_player.transform.position.x, m_player.transform.position.y, FIXED);

        this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, m_playerPosMin.x, m_playerPosMax.x),
                                              Mathf.Clamp(this.transform.position.y+2, m_playerPosMin.y, m_playerPosMax.y), FIXED);
	
    }
}
