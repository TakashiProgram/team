using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagController : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider _coll)
    {
        Debug.Log(_coll.tag);
        if (_coll.tag == "Player")
        {
            this.GetComponent<Animator>().SetTrigger("StandTrigger");
        }
    }
}
