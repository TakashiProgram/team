using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    [SerializeField]
    private int m_PosMinX,m_PosMaxX,m_PosMinY,m_PosMaxY;

    [SerializeField]
    private GameObject player;

    private float m_PosMaxZ = -4.75f;

    void Start () {
		
	}
	
	// Update is called once per frame8.85
	void Update () {
        this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, m_PosMaxZ);
        this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, m_PosMinX, m_PosMaxX),
                                              Mathf.Clamp(this.transform.position.y+2, m_PosMinY, m_PosMaxY), m_PosMaxZ);
	}
}
