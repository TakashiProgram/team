using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public Vector3 m_scale;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject m_inputManager;

    private readonly Vector3 m_smallerScale = new Vector3(0.2f, 0.2f,0.2f);
    
    void Start () {
        m_scale = this.transform.localScale;

    }
	
	void Update () {
       
        if (m_inputManager.GetComponent<InputManager>().m_floatEnemyFlag == false)
        {
            this.GetComponent<Rigidbody>().useGravity = true;
            this.transform.localScale = m_scale;
            m_inputManager.GetComponent<InputManager>().m_floatEnemyFlag = true;
        }
    }

    public void WindStop()
    {
        m_inputManager.GetComponent<InputManager>().m_tapWindFlag = false;
    }
}
