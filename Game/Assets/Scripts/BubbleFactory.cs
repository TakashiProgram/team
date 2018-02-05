using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct VectorCheckers
{
   public  bool x, y, z;
}

public class BubbleFactory : MonoBehaviour
{
    private const float RANGE      = 10.0f;   // 生成範囲
    private const int   BUBBLE_MAX = 30;      // シャボン最大生成数

    [SerializeField]
    private GameObject m_prefab;

    [SerializeField,Range(0,BUBBLE_MAX)]
    private uint m_bubbleMax;

    [SerializeField,Tooltip("生成する時間")]
    private float m_createTime;

    [SerializeField,Tooltip("飛ばす方向")]
    private Vector3 m_direction;
    [SerializeField, Tooltip("飛ばすスピード(1～指定数)")]
    private float m_speed = 1;

    [SerializeField,Tooltip("生成時のランダム指定")]
    private VectorCheckers m_randomArea;

    private GameObject[] m_bubbles = new GameObject[BUBBLE_MAX];

    // バブル生成フラグ
    private bool isFactory = true;

    //作成する泡の次の保存位置
    private int m_bubbleIndex = 0;

    private float m_time;


    // Use this for initialization
    void Start()
    {
        m_time        = 0;  // 計測
        m_bubbleIndex = 0;  
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFactory)
        {
            return;  
        }
        m_time += Time.deltaTime;
        if (m_time > m_createTime)
        {
            Vector3 randpos = new Vector3(transform.position.x + (m_randomArea.x ? Random.Range(-RANGE,     RANGE)     : 0),    // 出現位置X
                                          transform.position.y + (m_randomArea.y ? Random.Range(-RANGE,     RANGE)     :-5),    // 出現位置Y
                                          transform.position.z + (m_randomArea.z ? Random.Range(-RANGE / 2, RANGE * 2) : 0));   // 出現位置Z
            GameObject obj = GameObject.Instantiate(m_prefab, randpos, Quaternion.identity);
            float rand = Random.Range(0.5f, 0.5f);              // シャボン大きさ(a～b)
            Vector3 randScale = new Vector3(rand, rand, rand);
            obj.transform.localScale = randScale;
            obj.GetComponent<Rigidbody>().velocity = m_direction.normalized * Random.Range(1,m_speed);
            if (m_bubbles[m_bubbleIndex] == null)
            {
                m_bubbles[m_bubbleIndex++] = obj;
            }
            else
            {
                Destroy(m_bubbles[m_bubbleIndex]);      // バブル削除
                m_bubbles[m_bubbleIndex++] = obj;
            }
            //obj.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 10));
            m_time = 0;
        }
        m_bubbleIndex = m_bubbleIndex % BUBBLE_MAX;
    }

    public void stopFactory()
    {
        // バブル生成停止
        Debug.Log("生成停止");
        isFactory = false;
    }
}
