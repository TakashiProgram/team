using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject m_inputManager;

    private Vector3 m_scale;

    private readonly Vector3 m_smallerScale = new Vector3(0.2f, 0.2f,0.2f);

    private const float CENTER = 0.1f;
    // Use this for initialization
    void Start () {
        m_scale = this.transform.localScale;

    }
	
	// Update is called once per frame
	void Update () {
       
        //ほかのフラグに変更
        if (m_inputManager.GetComponent<InputManager>().m_tapWindFlag == false)
        {
            this.GetComponent<Rigidbody>().useGravity = true;
            this.transform.localScale = m_scale;
        }
     
    }
}
