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
    Camera uiCamera;

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
  

    void Start()
    {

    }
    
    void Update()
    {

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

                    break;
                case "Null":

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
    
   
}
