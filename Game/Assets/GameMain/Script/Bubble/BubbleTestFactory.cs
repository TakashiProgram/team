﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BubbleTestFactory : MonoBehaviour {


    public GameObject _bubblePrefab;
    public uint _bubbleCntLimit;
    uint _createCnt;
    uint _frame;

    GameObject _canvas;
    GameObject _textObj;

	// Use this for initialization
	void Start () {
        _frame = 0;
        _createCnt = 0;

        _canvas = GameObject.Find("Canvas");
        _textObj = _canvas.transform.Find("TestText").gameObject;
	}
	
	// Update is called once per frame
	void Update () {

        ++_frame;
        if(_frame%3==0)
        {
            ++_createCnt;

            Vector3 randpos = new Vector3(Random.Range(-5, 5), 10, Random.Range(-5, 5));
            randpos = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(-2, 2));
            GameObject obj = GameObject.Instantiate(_bubblePrefab, randpos,Quaternion.identity);
            float rand = Random.Range(0.3f, 2.0f);
            Vector3 randScale = new Vector3(rand,rand,rand);
            obj.transform.localScale=randScale;

            Vector3 v = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
            Debug.Log("v =" + v);
            obj.GetComponent<BubbleController>().Move(v);
            //obj.GetComponent<Rigidbody>().velocity = v;
        }


        _textObj.GetComponent<Text>().text = "生成数 = "+_createCnt;
        
	}

    
}
