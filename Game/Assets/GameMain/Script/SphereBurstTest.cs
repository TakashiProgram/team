using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBurstTest : MonoBehaviour {
    Material _material;
    //オブジェクトとのヒット位置（ワールド座標系）
    Vector3 _hitPosition;
	// Use this for initialization
	void Start () {
        _material = GetComponent<Renderer>().material;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Damage")
        {
            /*Collider.ClosestPointOnBounds(Vector3)
             * 設定した座標に一番近いColliderオブジェクトの座標を返す
             *
             */
            _hitPosition = col.ClosestPointOnBounds(this.transform.position);
            Debug.Log(_hitPosition);
            Vector4 hitPos = new Vector4(_hitPosition.x,_hitPosition.y,_hitPosition.z,1);
            _material.SetVector("_HitPosition", hitPos);
        }
        //とりあえず何か当たったら止まるようにしとく（テスト用
        GetComponent<Rigidbody>().isKinematic = true;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Damage")
        {
            foreach (ContactPoint point in col.contacts)
            {
                _hitPosition = point.point;
                Debug.Log(_hitPosition);
                Vector4 hitPos = new Vector4(_hitPosition.x, _hitPosition.y, _hitPosition.z, 1);
                _material.SetVector("_HitPosition", hitPos);
            }
        }
        //とりあえず何か当たったら止まるようにしとく（テスト用
        GetComponent<Rigidbody>().isKinematic = true;
    }
}
