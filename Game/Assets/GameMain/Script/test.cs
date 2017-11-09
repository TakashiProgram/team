using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {


    RaycastHit hit;

    [SerializeField]
    bool isEnable = false;

    [SerializeField]
    private Vector3 pos;

    bool teds;
    void OnDrawGizmos()
    {
        //if (isEnable == false)
          //  return;

         var radius = transform.lossyScale.x * 0.5f;
        Debug.Log(radius);
        var isHit = Physics.SphereCast(transform.position, radius, transform.forward * 10, out hit);
        if (isHit)
        {
            Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
            Gizmos.DrawWireSphere(transform.position + transform.forward * (hit.distance), radius);
            teds = true;
            
            Debug.Log("当たり");
        }
        else
        {
            if (teds)
            {
                Gizmos.DrawRay(transform.position, transform.forward * 100);
                pos = this.transform.parent.position;
                this.transform.parent.GetComponent<Player>().Resurrection(pos);
                teds = false;
            }
            

            Debug.Log("外れ");
            
        }
    }
}