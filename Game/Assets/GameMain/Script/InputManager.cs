using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //コントローラ
    [SerializeField]
    private GameObject stick, stickBase;
    [SerializeField]
    private GameObject leftTap, rightTap;
    //uiCamera
    [SerializeField]
    Camera uiCamera;
   
    public float distance = 100f;
    float len;
    float maxLen = 1.0f;
    bool test = true;
    string objectName;

    bool tapFlag = true;

    Color setColor = new Color(1,1,1,1);
    Color resetColor = new Color(1, 1, 1, 0.5f);
    void Start()
    {

    }
    
    void Update()
    {
      //   Move();
        TapMove();
    }
    //プレイヤーの移動処理一つ目
    void Move()
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
    //
    void TapMove()
    {
        TapRay();
        TapUpReset();
    }

  //タップしたオブジェクトの名前を取ってくる
    void TapRay()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = uiCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit, distance))
            {
                objectName = hit.collider.gameObject.name;
            }
            if (tapFlag)
            {
                switch (objectName)
                {
                    case "Left":
                        leftTap.GetComponent<SpriteRenderer>().color = setColor;
                        tapFlag = false;
                        break;
                    case "Right":
                        rightTap.GetComponent<SpriteRenderer>().color = setColor;
                        tapFlag = false;
                        break;
                }
            }
        }
    }
    //手を離したら元に戻す
    void TapUpReset()
    {
        if (Input.GetMouseButtonUp(0))
        {
            tapFlag = true;
            leftTap.GetComponent<SpriteRenderer>().color = resetColor;
            rightTap.GetComponent<SpriteRenderer>().color = resetColor;
        }
    }
}
