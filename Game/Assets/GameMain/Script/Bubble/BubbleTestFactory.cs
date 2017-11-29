using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BubbleTestFactory : MonoBehaviour {


    public GameObject _bubblePrefab;
    public uint _bubbleCntLimit;
    uint _frame;

    

	// Use this for initialization
	void Start () {
        _frame = 0;
	}
	
	// Update is called once per frame
	void Update () {

        ++_frame;
        if(_frame%60==0)
        {
            Vector3 randpos = new Vector3(Random.Range(-5, 5), 10, Random.Range(-5, 5));
            randpos = new Vector3(Random.Range(-5, 5), Random.Range(0, 5), 0);
            GameObject obj = GameObject.Instantiate(_bubblePrefab, randpos,Quaternion.identity);
            float rand = Random.Range(1.0f, 3.0f);
            Vector3 randScale = new Vector3(rand,rand,rand);
            obj.transform.localScale=randScale;
            obj.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 10);
            //obj.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 10));
        }

        
	}

    
}
