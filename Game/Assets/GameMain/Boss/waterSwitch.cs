using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterSwitch : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.GetComponent<Animator>().speed = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void stop()
    {
        this.GetComponent<Animator>().speed = 0;
        Invoke("Reset", 5);
    }
    private void stop2()
    {
        this.GetComponent<Animator>().speed = 0;
    }

    public void Reset()
    {
        this.GetComponent<Animator>().speed = 1;
    }
}
