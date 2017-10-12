using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {
    private Vector3 playerMove;
    [SerializeField]
    private float m_StartMove;
    [SerializeField]
    private float m_SetMove;
    void Start () {
		
	}
	
	void Update () {

        playerMove = this.transform.position;
        playerMove.y += m_StartMove * Time.deltaTime;
        this.transform.position = playerMove;
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            playerMove = this.transform.position;
            playerMove.y += m_SetMove * Time.deltaTime;
            this.transform.position = playerMove;
        }
    }
}
