using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //ボタン
    [SerializeField]
    private GameObject leftTap, rightTap;
    
    [SerializeField]
    private GameObject player;

    [SerializeField]
    Camera uiCamera, mainCamera;

    [SerializeField]
    private GameObject createManager;

    private float m_Distance = 10f;

    private string objectName;

    private bool createrFlag = false;

    Color setColor = new Color(1,1,1,1);

    Color resetColor = new Color(1, 1, 1, 0.5f);

    Vector3 playerMove;

    float moveCount=0.05f;
    
    private float m_BubbleScale = 0.01f;

    Vector3 test;

    Vector3 te;

    Vector3 tesssss;
    bool windflag = false;

    bool testflag = false;
    public float angle;


    private Vector3 touchStartPos;
    private Vector3 touchEndPos;

    string Direction;




    void Start()
    {

    }
    
    void Update()
    {
        TapVector();
        TapRay();
     
    }

    //タップしたオブジェクトの名前を取ってくる
    void TapRay()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = uiCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, m_Distance);
          
            switch (hit.collider.gameObject.name)
                {
                    case "Left":
                        leftTap.GetComponent<SpriteRenderer>().color = setColor;

                    if (player.GetComponent<Player>().bubbleFlag==false)
                    {
                        
                        playerMove = player.transform.position;
                        playerMove.x -= moveCount;
                        player.transform.position = playerMove;
                    }
                
                    iTween.RotateTo(player, iTween.Hash("y", -90));
                    player.GetComponent<Animator>().SetBool("Move",true);

                        break;
                    case "Right":

                        rightTap.GetComponent<SpriteRenderer>().color = setColor;
                    if (player.GetComponent<Player>().bubbleFlag==false)
                    {
                        
                        playerMove = player.transform.position;
                        playerMove.x += moveCount;
                        player.transform.position = playerMove;
                    }
                   
                    iTween.RotateTo(player, iTween.Hash("y", 90));
                    player.GetComponent<Animator>().SetBool("Move",true);

                    break;
                case "Bubble":

                    player.transform.parent = null;

                    player.GetComponent<Player>().bubbleFlag=false;

                    player.GetComponent<Rigidbody>().useGravity = true;

                    createManager.GetComponent<CreateManager>().TapBubble(m_BubbleScale);
                   // windflag = true;
                    break;
                case "Wind":
                   



                    break;

            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            TapUpReset();
            if (createManager.GetComponent<CreateManager>().windflagtest)
            {
                windflag = true;
            }
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

    private void TapVector()
    {
        if (windflag)
        {
            if (Input.GetMouseButtonDown(0))
            {

                //test = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition + mainCamera.transform.forward * 10);
                test = Input.mousePosition;
                //test = Vector3.forward;
                Debug.Log(test);
                //test.z = 0;

               // test = mainCamera.ScreenToWorldPoint(Input.mousePosition + mainCamera.transform.forward * 10);
            }
            if (Input.GetMouseButtonUp(0))
            {

                //  te = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition + mainCamera.transform.forward * 10);
                te = Input.mousePosition;
                Debug.Log(te);
                //  angle = Vector3.Angle(test, te);

                //  float dx = te.x - test.x;
                //float dy = te.y - test.y;
                //   Vector3 vec = new Vector3(dx, dy).normalized;

                //float rot = Mathf.Atan2(vec.y, vec.x) * 180 / Mathf.PI;
                //if (rot > 180) rot -= 360;
                //if (rot < -180) rot += 360;
                //   angle = Mathf.Atan2(dx, dy);
                tesssss = (test - te);
                tesssss.z = 0;
                tesssss.Normalize();
                createManager.GetComponent<CreateManager>().TapWind(tesssss);

              //  Debug.Log(rot);
                windflag = false;
            }
        }
      
    }

  
}
