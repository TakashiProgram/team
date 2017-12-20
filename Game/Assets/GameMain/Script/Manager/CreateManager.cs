using UnityEngine;

public class CreateManager : MonoBehaviour {
    //m_bubbleの動き
    public Vector3 m_WingMove;
    //Bubbleが最大の時
    public bool m_createWindFlag = false;

    //プレイヤー前方
    [SerializeField]
    private GameObject m_playerFront;

    [SerializeField]
    private GameObject m_bubble;

    [SerializeField]
    private GameObject m_wind;

    //Bubbleを生成したときに入れておく箱
    private GameObject m_bubbleCreateBox;

    private Collider m_object;

    private float m_bubbleScale;
    
    //m_bubbleの最大scale
    private const float SCALE_MAX = 0.7f;

    private void Update()
    {
       
    }

    //BubbleボタンをTapした時の処理
    public void TapBubble(float scale,float flip)
    {
        if (Input.GetMouseButtonDown(0))
        {
            //シャボン玉に何かが入っているときに処理する
            if (m_object != null)
            {
                m_object.GetComponent<Rigidbody>().useGravity = true;
                m_object.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
            
            m_bubbleScale = scale;
            Destroy(m_bubbleCreateBox);
            m_bubbleCreateBox = Instantiate(m_bubble, new Vector3(m_playerFront.transform.position.x, m_playerFront.transform.position.y, 0), Quaternion.identity);
            
        }
            m_bubbleScale+=Time.deltaTime* flip;

            m_bubbleCreateBox.transform.localScale = new Vector3(m_bubbleScale, m_bubbleScale, m_bubbleScale);
        //絶対値
        float value = Mathf.Abs(m_bubbleScale);
        if (value >= SCALE_MAX)
        {
            m_bubbleScale = SCALE_MAX* flip;

            m_createWindFlag = true;
        }
    }
    //風作成
    public void TapWind(Vector3 vector)
    {
        if (GameObject.Find("Bubble") != null)
        {
            if (vector != Vector3.zero)
            {
                GameObject m_windBox; m_windBox= Instantiate(m_wind, m_bubbleCreateBox.transform.position, Quaternion.identity);
                Destroy(m_windBox, 1);
            }
            
            m_WingMove = vector;
        }
    }
    public void PutInObject(Collider collider)
    {
        m_object = collider;
    }
}
