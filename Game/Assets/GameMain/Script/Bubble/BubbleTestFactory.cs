using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BubbleTestFactory : MonoBehaviour {


    public GameObject _bubblePrefab;
    public uint _bubbleCntLimit;
    [SerializeField]
    uint _createCnt;
    uint _frame;

    GameObject _canvas;
    GameObject _textObj;

    Vector3 _randpos;
    Vector3 _randScale;
    Vector3 _initDir;

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
        if (_bubbleCntLimit > _createCnt)
        {
            if (_frame % 3 == 0)
            {
                ++_createCnt;
                
                _randpos.Set(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(-2, 2));
                GameObject obj = GameObject.Instantiate(_bubblePrefab, _randpos, Quaternion.identity);
                float rand = Random.Range(0.3f, 2.0f);
                _randScale.Set(rand, rand, rand);
                obj.transform.localScale = _randScale;

                _initDir.Set(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
                _initDir = _initDir.normalized;
                obj.GetComponent<BubbleController>().Move(_initDir);
                //obj.GetComponent<Rigidbody>().velocity = v;
            }
        }


        //_textObj.GetComponent<Text>().text = "生成数 = "+_createCnt;
        
	}

    
}
