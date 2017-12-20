using UnityEngine.SceneManagement;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    [SerializeField]
    private GameObject m_player;

    [SerializeField]
    private GameObject m_createManager;

    [SerializeField]
    private Camera m_uiCamera;

    [SerializeField]
    private Camera m_cameraManager;

    private Vector3 m_downWind;

    private int m_flip = 1;

    private bool m_tapWindFlag = false;

    private bool m_floatEnemyFlag = false;

    private bool m_stopWindFlag = false;

    private RaycastHit2D m_hitObject;

    //playerの回転
    private const int PLAYER_ROTATION = 90;
    //rayが届く距離
    private const float DISTANCE = 10f;
    //bubbleの大きさの変化の値
    private const float BUBBLE_SCALE = 0.01f;

    private readonly Color m_setColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    private readonly Color m_resetColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);

    void Update()
    {
       
        TapVector();
        TapRay();
    }

    //タップしたオブジェクトの名前を取ってくる
    private void TapRay()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = m_uiCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            
            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, DISTANCE);

            //パソコンで実行しているとバグるため置いている
            //デバック用
            if (hit.collider == null) return;

            m_hitObject = hit;
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.gameObject.name=="BubbleTap")
                {
                    m_createManager.GetComponent<CreateManager>().m_createWindFlag = false;
                    
                }
            }
            switch (hit.collider.gameObject.name)
                {
                    case "Left":
                    m_flip = -1;
                   
                    if (m_stopWindFlag==false)
                    {
                        
                        m_tapWindFlag = false;
                        hit.collider.GetComponent<SpriteRenderer>().color = m_setColor;
                        m_hitObject = hit;
                        
                        m_player.GetComponent<Player>().Move(m_flip);

                        iTween.RotateTo(m_player, iTween.Hash("y", -PLAYER_ROTATION));
                        m_player.GetComponent<Animator>().SetBool("Move", true);

                    }

                    break;
                    case "Right":
                    m_flip = 1;
                    if (m_stopWindFlag == false)
                    {
                        
                        m_tapWindFlag = false;
                        hit.collider.GetComponent<SpriteRenderer>().color = m_setColor;
                        m_hitObject = hit;
                        
                        m_player.GetComponent<Player>().Move(m_flip);
                        iTween.RotateTo(m_player, iTween.Hash("y", PLAYER_ROTATION));
                        m_player.GetComponent<Animator>().SetBool("Move", true);

                    }

                    break;
                case "BubbleTap":
                    if (m_stopWindFlag == false)
                    {
                       
                        m_tapWindFlag = false;
                        m_floatEnemyFlag = false;

                        hit.collider.GetComponent<SpriteRenderer>().color = m_setColor;
                        m_hitObject = hit;
                        m_player.transform.parent = null;

                        m_player.GetComponent<Player>().BubbleFlag();

                        m_player.GetComponent<Rigidbody>().useGravity = true;

                        m_createManager.GetComponent<CreateManager>().TapBubble(BUBBLE_SCALE, m_flip);
                    }
                    break;
                case "Wind":
                    TapUpReset();
                    TapVector();
                    m_stopWindFlag = true;

                    break;
                    //デバック用
                case "Reselt":
                    
                    SceneManager.LoadScene("GameMain");

                 
                    break;

                case "Clear":

                    SceneChanger.LoadSceneAtListAsync(SceneNameList.Title);

                    break;

                case "Decision":
                    
                    m_cameraManager.GetComponent<CameraManager>().Resurrection();

                    m_cameraManager.GetComponent<CameraManager>().End();
                    m_player.GetComponent<Player>().Down();
                 
                    break;

                case "Cancel":

                    // Select画面に移行する
                    //GameOverを表示するかも？
                    //デバッグ
                    SceneChanger.LoadSceneAtListAsync(SceneNameList.Select);

                    break;
                    
            }
        }else
        if (Input.GetMouseButtonUp(0))
        {
            TapUpReset();
            if (m_createManager.GetComponent<CreateManager>().m_createWindFlag)
            {
                
                m_tapWindFlag = true;
                if (GameObject.Find("Bubble")!=null)
                {

                    GameObject.Find("Bubble").transform.GetComponent<Bubble>().DestroyTime();
                    GameObject.Find("Bubble").transform.GetComponent<BubbleController>().BubbleVibrate();

                }
            }
        }
    }
    
    //手を離したら元に戻す
   private void TapUpReset()
    {
        {
            m_hitObject.collider.GetComponent<SpriteRenderer>().color = m_resetColor;
        }
       
        
        m_player.GetComponent<Animator>().SetBool("Move", false);
        m_stopWindFlag = false;

    }
    //シャボン玉を生成した後に風を発生させる
    private void TapVector()
    {

        if (m_tapWindFlag)
        {
            if (Input.GetMouseButtonDown(0))
            {
                 m_downWind = Input.mousePosition;

               }else
            if (Input.GetMouseButtonUp(0))
            {

                Vector3 UpWind = Input.mousePosition;

               Vector3 SetWind = (m_downWind - UpWind);
                SetWind.z = 0;
                SetWind.Normalize();
                m_createManager.GetComponent<CreateManager>().TapWind(SetWind);
                if (m_downWind!=UpWind)
                {
                    GameObject.Find("Bubble").transform.GetComponent<BubbleController>().BubbleVibrate(SetWind);

                }
                m_stopWindFlag = false;
            }
        } 
    }
}
