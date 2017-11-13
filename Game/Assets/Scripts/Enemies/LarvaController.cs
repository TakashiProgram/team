using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarvaController : MonoBehaviour {

    [System.Serializable]
    struct LarvaMovePoint
    {
        public GameObject start;
        public GameObject end;
    }
    [SerializeField,Range(1.0f,10.0f)]
    private float m_speed;
    [SerializeField,Tooltip("巡回場所を指定")]
    private LarvaMovePoint m_movePoint;

    private Queue<Vector3> m_target;

    private float m_turnArea;

    Animator m_animator;

	void Start () {
        if (m_movePoint.start != null && m_movePoint.end != null)
        {
            Debug.Log("true");
            m_target = new Queue<Vector3>();
            m_target.Enqueue(m_movePoint.start.transform.position);
            m_target.Enqueue(m_movePoint.end.transform.position);
        }
        m_animator = GetComponent<Animator>();
        m_turnArea = m_speed / 2;
	}

    void Update()
    {
    }

    /// <summary>
    /// MoveAnimation開始時に呼ばれ、今までの速度をリセットします。
    /// </summary>
    public void MoveBegin()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0);
        Debug.Log("(m_target.Peek() - transform.position).magnitude = " + (m_target.Peek() - transform.position).magnitude);
    }

    public void Move()
    {

        GetComponent<Rigidbody>().velocity = (transform.forward * m_speed) + (new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0));

        Vector3 targetLen = m_target.Peek() - transform.position;
        targetLen.y = 0;
        if (targetLen.magnitude < m_turnArea)
        {
            UpdateTargetPoint();
        }
    }

    /// <summary>
    /// アニメーション時に呼び出される設定されたターゲットへの方向転換と移動の再スタートをする関数です
    /// </summary>
    public void ReStart()
    {

        m_animator.SetBool("isTurn", false);
        Vector3 targetVec = m_target.Peek();
        targetVec.y = transform.position.y;
        transform.LookAt(targetVec);
        m_animator.Play("Move");
    }


    private void UpdateTargetPoint()
    {
        //前回のターゲット角度と更新したターゲット角度を計算
        //前回と更新時のターゲットへ向かうベクトルの角度差が一定以上なら振り向きアニメーションへ移行するように命令
        Vector3 oldTargetVec, nowTargetVec;
        oldTargetVec = m_target.Peek() - transform.position;
        oldTargetVec.Normalize();

        m_target.Enqueue(m_target.Dequeue());
        nowTargetVec = m_target.Peek() - transform.position;
        nowTargetVec.Normalize();

        float rad = Vector3.Dot(oldTargetVec, nowTargetVec);
        rad = Mathf.Acos(rad);
        rad = rad * 180.0f / Mathf.PI;
        Debug.Log("rad = " + rad);
        Debug.Log("UpdateTarget");
      
        if (rad >= 120)
        {
            m_animator.SetBool("isTurn", true);
        }
    }

    
}
