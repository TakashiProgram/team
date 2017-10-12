using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {

    [SerializeField]
    private float m_StartMove, m_SetMove;
    
    private Vector3 Move;

    void Start () {
		
	}
	
	void Update () {

        Move = this.transform.position;
        Move.y += m_StartMove * Time.deltaTime;
        this.transform.position = Move;
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            Move = this.transform.position;
            Move.y += m_SetMove * Time.deltaTime;
            this.transform.position = Move;
        }
    }
}
