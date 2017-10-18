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

    void Start () {
        create = GameObject.Find("CreateManager");
    }
	
	void Update () {

        Move = this.transform.position;
        Move.y += m_StartMove * Time.deltaTime;
        this.transform.position = Move;

        this.transform.position+= create.GetComponent<CreateManager>().dir*2 * Time.deltaTime;
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
