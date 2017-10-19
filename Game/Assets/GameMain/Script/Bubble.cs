using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {

    [SerializeField]
    private float m_StartMove, m_SetMove;

    [SerializeField]
    private GameObject wind;

    private Vector3 Move;

    private GameObject create;

    private float m_BubbleMove = 1.5f;

    private int m_Flip = -1;

    void Start () {
        create = GameObject.Find("CreateManager");
    }
	
	void Update () {
        this.transform.position+= create.GetComponent<CreateManager>().WingMove * m_BubbleMove * m_Flip * Time.deltaTime;
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
