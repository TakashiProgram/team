using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    bool changeFlag;  // シーン遷移可能かどうか
    


	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        GameObject fade = GameObject.Find("FadeCamera");     // 
        
        if (fade.GetComponent<Fader>().IsFadeEnd())
        {
            
            // 画面クリック/タップで遷移
            if (Input.GetMouseButtonDown(0))
            {
                SceneChanger.LoadSceneAtListAsync(SceneNameList.GameMain);
            }
        }
    }
}
