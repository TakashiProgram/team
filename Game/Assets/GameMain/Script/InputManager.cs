using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //コントローラ
    [SerializeField]
    private GameObject stick, stickBase;
    //uiCamera
    [SerializeField]
    Camera uiCamera;
   
    public float distance = 100f;
    float len;
    float maxLen = 1.0f;
    void Start()
    {

    }
    
    void Update()
    {
        TapMove();
    }
    //プレイヤーの移動処理
    void TapMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            stick.SetActive(true);
            stickBase.SetActive(true);
        }

        if (Input.GetMouseButton(0))
        {

            Vector3 tapPos = uiCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);

            stick.transform.position = new Vector3(tapPos.x, stick.transform.position.y, 0);

            Vector3 v1 = (stick.transform.position - stickBase.transform.position);

            float len = v1.magnitude;

            float maxLen = 1.0f;
            if (len > maxLen)
            {
                stick.transform.position = stickBase.transform.position + ((v1.normalized) * maxLen);
            }
        
        }

        if (Input.GetMouseButtonUp(0))
        {
            stick.SetActive(false);
            stickBase.SetActive(false);
        }

    }
  
}
