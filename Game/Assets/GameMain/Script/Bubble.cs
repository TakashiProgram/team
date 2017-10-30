using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {

    [SerializeField]
    private float m_setMove;

    private GameObject create;

    private const float BUBBLE_MOVE = 1.5f;

    private const int INVERTED = -1;

    void Start () {
        create = GameObject.Find("CreateManager");
    }
	
	void Update () {
        this.transform.position+= create.GetComponent<CreateManager>().m_WingMove *
                                  BUBBLE_MOVE * INVERTED * Time.deltaTime;
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            Vector3 m_move = this.transform.position;
            m_move.y += m_setMove * Time.deltaTime;
            this.transform.position = m_move;
        }
    }
}
