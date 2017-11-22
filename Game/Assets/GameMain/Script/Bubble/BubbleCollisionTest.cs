using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleCollisionTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag != "Player")
        {
            Debug.Log("Burst(col)を呼ぶ");
            gameObject.GetComponent<BubbleController>().Burst(col);
        }
    }
}
