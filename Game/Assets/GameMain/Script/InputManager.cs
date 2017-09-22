using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //コントローラ
    [SerializeField]
    private GameObject stick, stickBase;
    //ボタン
    [SerializeField]
    private GameObject leftTap, rightTap;
    //プレイヤー
    [SerializeField]
    private GameObject player;
    //uiCamera
    [SerializeField]
    Camera uiCamera;
    //プレイヤー前方
    [SerializeField]
    private GameObject front;

    public GameObject[] Bubble;
    private float distance = 100f;

    private float len;
    private float maxLen = 1.0f;

    private string objectName;

    Color setColor = new Color(1,1,1,1);
    Color resetColor = new Color(1, 1, 1, 0.5f);
    Vector3 playerMove;
    float moveCount=0.05f;

    private bool longPressFlag = false;
    
    private GameObject test;

    float x = 0;
    float y = 0;
    float z = 0;


    void Start()
    {

    }
    
    void Update()
    {
        TapBubble();
        TapMove();
    }

  
    //プレイヤーの移動処理一つ目
    //void Move()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        stick.SetActive(true);
    //        stickBase.SetActive(true);
    //    }

    //    if (Input.GetMouseButton(0))
    //    {

    //        Vector3 tapPos = uiCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);

    //        stick.transform.position = new Vector3(tapPos.x, stick.transform.position.y, 0);

    //        Vector3 v1 = (stick.transform.position - stickBase.transform.position);

    //        float len = v1.magnitude;

    //        float maxLen = 1.0f;
    //        if (len > maxLen)
    //        {
    //            stick.transform.position = stickBase.transform.position + ((v1.normalized) * maxLen);
    //        }

    //    }

    //    if (Input.GetMouseButtonUp(0))
    //    {
    //        stick.SetActive(false);
    //        stickBase.SetActive(false);
    //    }

    //}
    //プレイヤーの移動処理二つ目
    void TapMove()
    {
        TapRay();
    }

  //タップしたオブジェクトの名前を取ってくる
    void TapRay()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = uiCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

          
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit, distance))
            {

                objectName = hit.collider.gameObject.name;
            }
            
            //UIの名前で判定
            switch (objectName)
                {
                    case "Left":
                        leftTap.GetComponent<SpriteRenderer>().color = setColor;
                        playerMove = player.transform.position;
                        playerMove.x-=moveCount;
                        player.transform.position = playerMove;

                    iTween.RotateTo(player, iTween.Hash("y", -90));
                    player.GetComponent<Animator>().SetBool("Move",true);
                        break;
                    case "Right":
                        rightTap.GetComponent<SpriteRenderer>().color = setColor;
                        playerMove = player.transform.position;
                        playerMove.x+=moveCount;
                        player.transform.position = playerMove;
                    iTween.RotateTo(player, iTween.Hash("y", 90));
                    player.GetComponent<Animator>().SetBool("Move",true);

                    break;
               
                }
        }
        if (Input.GetMouseButtonUp(0))
        {
            TapUpReset();
        }
    }
    
    //手を離したら元に戻す
    void TapUpReset()
    {
            leftTap.GetComponent<SpriteRenderer>().color = resetColor;
            rightTap.GetComponent<SpriteRenderer>().color = resetColor;
            player.GetComponent<Animator>().SetBool("Move", false);
            objectName = null;
    }

    void TapBubble()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit, distance))
            {

                objectName = hit.collider.gameObject.name;
            }

            switch (objectName)
            {
                case "front":
                    test= Instantiate(Bubble[0], new Vector3(front.transform.position.x, front.transform.position.y, 0), Quaternion.identity);

                    longPressFlag = true;
                        break;
            }
                    //   var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Camera.main.transform.forward * 10);
                    //  Instantiate(Bubble[0], new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0), Quaternion.identity);

            }
        if (longPressFlag)
        {
            x += 0.01f;
            y += 0.01f;
            z += 0.01f;
            test.transform.localScale = new Vector3(x, y, z);
            if (x>1)
            {
                x = 1;
                y = 1;
                z = 1;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            longPressFlag = false;
        }

    }
}
