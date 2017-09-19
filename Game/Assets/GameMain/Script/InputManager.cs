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
   
    private float distance = 100f;

    private float len;
    private float maxLen = 1.0f;

    private string objectName;

    Color setColor = new Color(1,1,1,1);
    Color resetColor = new Color(1, 1, 1, 0.5f);
    Vector3 playerMove;
    float moveCount=0.05f;
    void Start()
    {

    }
    
    void Update()
    {
         //Move();
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
    //プレイヤーの移動処理二つ目
    void TapMove()
    {
        TapRay();
     //   TapUpReset();
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
                case "TapStop":
                    TapUpReset();
                    break;
                }
        }
        if (Input.GetMouseButtonUp(0))
        {;
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
}
