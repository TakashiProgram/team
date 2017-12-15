using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultStop : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Display()
    {
        this.GetComponent<Animator>().enabled = true;
    }

    private void Stop()
    {
        this.GetComponent<Animator>().speed = 0;
    }
}
