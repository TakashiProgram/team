using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InputManager : MonoBehaviour
{
   
    [SerializeField]
    private GameObject m_leftTap;
    [SerializeField]
    private GameObject m_rightTap;

    [SerializeField]
    private GameObject m_player;

    [SerializeField]
    private Camera m_mainCamera;
    [SerializeField]
    private Camera m_uiCamera;

    [SerializeField]
    private GameObject m_createManager;

    private Vector3 m_downWind;

    private readonly Color m_setColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    private readonly Color m_resetColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);

    private const int PLAYER_ROTATION = 90;
    //rayが届く距離
    private const float DISTANCE = 10f;

    private const float MOVE_COUNT = 0.05f;

    private const float BUBBLE_SCALE = 0.01f;

    public bool m_windFlag = false;
   
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
            Ray ray = m_uiCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, DISTANCE);
          
            switch (hit.collider.gameObject.name)
                {
                    case "Left":
                    m_windFlag = false;
                    m_leftTap.GetComponent<SpriteRenderer>().color = m_setColor;

                    if (m_player.GetComponent<Player>().m_bubbleFlag==false)
                    {

                        Vector3 playerMove = m_player.transform.position;
                        playerMove.x -= MOVE_COUNT;
                        m_player.transform.position = playerMove;
                    }
                
                    iTween.RotateTo(m_player, iTween.Hash("y", -PLAYER_ROTATION));
                    m_player.GetComponent<Animator>().SetBool("Move",true);

                        break;
                    case "Right":
                    m_windFlag = false;
                    m_rightTap.GetComponent<SpriteRenderer>().color = m_setColor;
                    if (m_player.GetComponent<Player>().m_bubbleFlag==false)
                    {

                        Vector3 playerMove = m_player.transform.position;
                        playerMove.x += MOVE_COUNT;
                        m_player.transform.position = playerMove;
                    }
                   
                    iTween.RotateTo(m_player, iTween.Hash("y", PLAYER_ROTATION));
                    m_player.GetComponent<Animator>().SetBool("Move",true);

                    break;
                case "Bubble":

                    m_player.transform.parent = null;

                    m_player.GetComponent<Player>().m_bubbleFlag=false;

                    m_player.GetComponent<Rigidbody>().useGravity = true;

                    m_createManager.GetComponent<CreateManager>().TapBubble(BUBBLE_SCALE);
                    break;
                case "Wind":
                    Debug.Log("sgh");
                    TapVector();


                    break;

            }
        }else
        if (Input.GetMouseButtonUp(0))
        {
            TapUpReset();
            if (m_createManager.GetComponent<CreateManager>().m_windFlag)
            {
                m_windFlag = true;
               // Invoke(GameObject.Find("Bubble(Clone)").transform.GetComponent<Bubble>().DestroyTime(),1);
                GameObject.Find("Bubble(Clone)").transform.GetComponent<Bubble>().te();
            }
            else
            {

                m_windFlag = false;
            }
        }
    }
    
    //手を離したら元に戻す
    void TapUpReset()
    {
        m_leftTap.GetComponent<SpriteRenderer>().color = m_resetColor;
        m_rightTap.GetComponent<SpriteRenderer>().color = m_resetColor;
        m_player.GetComponent<Animator>().SetBool("Move", false);
       
    }
    //シャボン玉を生成した後に風を発生させる
    private void TapVector()
    {
        if (m_windFlag)
        {
            if (Input.GetMouseButtonDown(0))
            {
                 m_downWind = Input.mousePosition;
               }
            if (Input.GetMouseButtonUp(0))
            {
                Vector3 UpWind = Input.mousePosition;

              Vector3  SetWind = (m_downWind - UpWind);
                SetWind.z = 0;
                SetWind.Normalize();
                m_createManager.GetComponent<CreateManager>().TapWind(SetWind);

                m_windFlag = false;
            }
        } 
    }
}
