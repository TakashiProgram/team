using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitGround : MonoBehaviour {

    private RaycastHit hit;

    [SerializeField]
    private Vector3 pos;

    private bool m_groundFlag;

    void OnDrawGizmos()
    {
       
        var radius = transform.lossyScale.x * 0.1f;

        var isHit = Physics.SphereCast(transform.position, radius, transform.forward * 10, out hit);
        //地面に当たっているかどうか
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