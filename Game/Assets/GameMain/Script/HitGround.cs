using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitGround : MonoBehaviour {

    RaycastHit hit;

    [SerializeField]
    private Vector3 pos;

    private bool m_groundFlag;

    private void Update()
    {

    }
    void OnDrawGizmos()
    {
       
        var radius = transform.lossyScale.x * 0.5f;

        var isHit = Physics.SphereCast(transform.position, radius, transform.forward * 10, out hit);
        if (isHit)
        {
            Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
            Gizmos.DrawWireSphere(transform.position + transform.forward * (hit.distance), radius);
            m_groundFlag = true;


        }
        else
        {
            if (m_groundFlag)
            {
                Gizmos.DrawRay(transform.position, transform.forward * 100);
                pos = this.transform.parent.position;
                this.transform.parent.GetComponent<Player>().Resurrection(pos);
                m_groundFlag = false;
            }
        }
    }
}